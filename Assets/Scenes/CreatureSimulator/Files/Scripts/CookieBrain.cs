using UnityEngine; using UnityEngine.UI; using DG.Tweening;

public class CookieBrain : MonoBehaviour {
    [Header("-Controll-")]
    public bool DebugMode; public bool InstantDeath; public bool Brain = true;
    [Header("-Properties-")]
    public float ThirstReaction; public float HungerReaction;
    public int Health;
    [Header("-Movement-")]
    public float Radius = 2.5f; public float MovementColdown = 2f;
    public float ViewRadius = 2.5f; public float MoveSpeed = 3.5f;
    private float MoveTimer; 
    [Header("-States-")]
    public bool SearchingFood; public bool SearchingDrink;
    [Header("-Origin-")]
    public int ParentGen; public Sprite[] SpriteLib;
    [Header("-Sounds-")]
    private AudioSource AudioOrigin; public AudioClip[] AudioLib;

    Vector2 Target; public bool Moving = false;
    private LayerMask waterLayer; private LayerMask bushLayer; private LayerMask obstacleLayer;
    private GameTool GameSetting; private GameObject DangerObject;
    private int MaxHealth; private Transform WaterTarget;
    private Vector2 DangerPosition; private bool DangerDetected = false;
    private float MaxThirst, MaxHunger, CurrentThirst, CurrentHunger;
    private string CurrentThink;
    private SpriteRenderer SpriteService;

    void Start() {
        SpriteService = this.gameObject.GetComponent<SpriteRenderer>();
        GameSetting = GameObject.Find("Main Camera").GetComponent<GameTool>();
        MaxThirst = 100f; MaxHunger = 100f; MaxHealth = 7; Health = MaxHealth;
        CurrentThirst = MaxThirst; CurrentHunger = MaxHunger; MoveTimer = MovementColdown;
        ParentGen = Random.Range(1,4); SpriteService.sprite = SpriteLib[ParentGen - 1];
        AudioOrigin = gameObject.AddComponent<AudioSource>();
        waterLayer = LayerMask.GetMask("Water"); bushLayer = LayerMask.GetMask("Bush");
        obstacleLayer = LayerMask.GetMask("Obstacle");
        DangerPosition = Vector2.zero;
        ChooseTarget(); 
        if (DebugMode) {
            Debug.Log("System: DebugMode Enabled");
            if (GameSetting != null) { Debug.Log("SearchResult: 'GameSetting' Sucsessfully Finded."); } else { 
                Debug.LogError("SearchResult: 'GameSetting' Failed To Find!"); }
            Debug.Log("DataInit: Maximal Food & Thirst Parameters = "+MaxHunger+", "+MaxThirst);
            if (!InstantDeath) { Debug.Log("DataInit: Maximal Health Parameter = "+MaxHealth); }
            Debug.Log("DataInit: ParentGen Parameter = "+ParentGen);
        }
    } void Update() {
        CurrentThirst -= 1.4f * Time.deltaTime; if (CurrentThirst <= 0) { 
            if (InstantDeath) { Death(); } } //else { ApplyDamage(); } }
        CurrentHunger -= 1f * Time.deltaTime; if (CurrentHunger <= 0) { 
            if (InstantDeath) { Death(); } } //else { ApplyDamage(); } }
        if (CurrentThirst <= ThirstReaction) { SearchingDrink = true; }
        if (CurrentHunger <= HungerReaction) { SearchingFood = true; }
        DetectDanger(); MovementManage(); CalculateSpeed();
    } void DetectDanger() {
        Collider2D[] dangers = Physics2D.OverlapCircleAll(transform.position, ViewRadius);
        DangerDetected = false;
        foreach (var danger in dangers) {
            if (danger.CompareTag("Danger")) { //&& danger.GetComponent<Nut>().Hungry) {
                DangerDetected = true; DangerPosition = danger.transform.position; DangerObject = danger.gameObject;
                break;
            }
        }
    }
    void Death() { Destroy(this.gameObject); } 
    public void Breed() {
        if (ParentGen > 1 && Random.Range(1,5) == 3) { 
            Instantiate(this.gameObject, transform.position, 
            transform.rotation, transform.parent); PlaySound(2);
        } if (ParentGen > 2 && Random.Range(1,4) == 3) { 
            Instantiate(this.gameObject, transform.position, 
            transform.rotation, transform.parent); PlaySound(3);
        }
    } void MovementManage() {
        if (Brain) {
        float distanceToTarget = Vector2.Distance(transform.position, Target);
        GameTool GameManage = GameObject.Find("Main Camera").GetComponent<GameTool>();
        int cookieCount = GameManage.CookiesShelf.transform.childCount;
        if (DangerDetected) {
            if (ParentGen == 1 && cookieCount > 9) {
                Vector2 directionToDanger = ((Vector2)transform.position - DangerPosition).normalized;
                float distanceToDanger = Vector2.Distance(transform.position, DangerPosition);
                if (distanceToDanger > 0.1f) {
                    Target = DangerPosition;
                    //Vector2 newPos = Vector2.MoveTowards(transform.position, DangerPosition, MoveSpeed * Time.deltaTime);
                    //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
                } else {
                    if (DangerObject != null && DangerObject.CompareTag("Danger") && Random.Range(0,50) == 3) {
                        Destroy(DangerObject); PlaySound(0); DangerDetected = false; ChooseTarget();
                    }
                }
            } else {
                Vector2 avoidDirection = ((Vector2)transform.position - DangerPosition).normalized;
                Vector2 avoidTarget = (Vector2)transform.position + avoidDirection * Radius; 
                RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, avoidDirection, Radius, obstacleLayer);  
                if (hitObstacle.collider == null) { Target = avoidTarget; Moving = true; } else { ChooseTarget(); }
            }
        }
        if (!Moving || distanceToTarget < 0.1f) {
            if (distanceToTarget < 0.1f && Moving) {
                if (IsBushAtPosition(Target)) { EatBush(Target); }
                if (WaterTarget != null && IsWaterAtPosition(WaterTarget.position)) { DrinkWater(); }
                Moving = false; MoveTimer = MovementColdown;
            } MoveTimer -= Time.deltaTime;
            if (MoveTimer <= 0 && !Moving) {
                if (SearchingFood) { FindFood(); } else if (SearchingDrink) { FindWater(ViewRadius); }
                else { if (MoveTimer < 0) { ChooseTarget(); } } Moving = true;
            }
        } else {
            //Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            //Vector2 newPos = Vector2.MoveTowards(pos, Target, MoveSpeed * Time.deltaTime);
            //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            transform.DOMove(Target, (MoveSpeed * 10 * Time.deltaTime)).SetEase(Ease.InSine);
        }
        }
    } public void ChooseTarget() {
        for (int i = 0; i < 10; i++) {
            Vector2 candidate = (Vector2)transform.position + Random.insideUnitCircle * Radius;
            bool isDangerNearCandidate = false;
            Collider2D[] dangersNearCandidate = Physics2D.OverlapCircleAll(candidate, 1f);
            foreach(var danger in dangersNearCandidate) { 
                if(danger.CompareTag("Danger")) { isDangerNearCandidate = true; break; }
            }
            if (ParentGen != 1) { if (isDangerNearCandidate) continue; }
            Vector2 direction = candidate - (Vector2)transform.position; float distance = direction.magnitude;
            RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);
            if (hitObstacle.collider == null) { Target = candidate; return; }
        } Target = transform.position; Moving = false;
    } void FindWater(float Radius) {
        Collider2D[] waterColliders = Physics2D.OverlapCircleAll(transform.position, Radius, waterLayer);
        if (waterColliders.Length > 0) { 
            WaterTarget = waterColliders[0].transform; Target = WaterTarget.position; Moving = true;
        } else { WaterTarget = null; ChooseTarget(); }
    } void FindFood() {
        Collider2D[] bushes = Physics2D.OverlapCircleAll(transform.position, Radius,bushLayer);
        float ClosestDist = Mathf.Infinity; Vector2 ClosestBushPos= transform.position;
        foreach(var bush in bushes){
            Bush BushScript = bush.GetComponent<Bush>();
            if (BushScript != null && BushScript.Stage == 3) {
                float Distancion = Vector2.Distance(transform.position,bush.transform.position); 
                bool dangerNearBush = false;
                Collider2D[] dangersNearBush = Physics2D.OverlapCircleAll(bush.transform.position, 1f);
                foreach(var danger in dangersNearBush) {  if (danger.CompareTag("Danger")) { 
                        dangerNearBush = true; break; 
                    } 
                }
                if(dangerNearBush) continue;
                if (Distancion < ClosestDist) {  ClosestDist = Distancion; ClosestBushPos = bush.transform.position; }
            }
        }
        if (ClosestDist != Mathf.Infinity) { Target = ClosestBushPos; Moving = true; } else {  ChooseTarget(); }
    }
    bool IsWaterAtPosition(Vector2 pos) { 
        Collider2D hitWater = Physics2D.OverlapPoint(pos, waterLayer); return hitWater != null;
    } bool IsBushAtPosition(Vector2 pos) {
        Collider2D hitBushPoint = Physics2D.OverlapPoint(pos, bushLayer);
        return hitBushPoint != null && hitBushPoint.GetComponent<Bush>() != null && hitBushPoint.GetComponent<Bush>().Stage == 3;
    }

    void EatBush(Vector2 position) {
        Collider2D[] bushesAtPos = Physics2D.OverlapCircleAll(position, 0.1f, bushLayer);
        foreach(var col in bushesAtPos){
            Bush BushScript = col.GetComponent<Bush>();
            if (BushScript != null && BushScript.Stage == 3) {
                CurrentHunger += 50f; if (CurrentHunger < 0) CurrentHunger = 0;
                BushScript.Eat(); SearchingFood = false; PlaySound(0); break; 
            }
        } ChooseTarget(); Moving = true; Breed();
    }
    void DrinkWater() {
        Vector2 directionToWater = (WaterTarget.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, WaterTarget.position, MoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, WaterTarget.position) < 0.1f) {
            CurrentThirst += 50f; if (CurrentThirst > MaxThirst) CurrentThirst = MaxThirst;
            Destroy(WaterTarget.gameObject); WaterTarget = null; SearchingDrink = false; ChooseTarget(); PlaySound(1);
        } return;
    }
    void CalculateSpeed() {
        GameTool GameManage = GameObject.Find("Main Camera").GetComponent<GameTool>();
        int cookieCount = GameManage.CookiesShelf.transform.childCount;
        if (cookieCount < 10) {
            MoveSpeed = 4f;
        } if (cookieCount > 10) {
            MoveSpeed = 3.5f;
        }
    }
    void PlaySound(int SoundSetting) { AudioOrigin.PlayOneShot(AudioLib[SoundSetting]); }
}
