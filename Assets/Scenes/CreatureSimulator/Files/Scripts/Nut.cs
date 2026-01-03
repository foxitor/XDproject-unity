using UnityEngine; using DG.Tweening;

public class Nut : MonoBehaviour {
    public bool DebugMode = false, AI = true, PreyDetected, PartnerFinded, InstantDeath, SearchingPartner, Hungry;
    public float JumpPrepare, JumpSpeed, JumpLength, AfterJumpRest, ViewRadius, Squize;
    public string State; public Sprite[] SpriteLib; private GameObject PreyTarget, PartnerTarget;
    public int BreedCounter; public GameObject SporeSeed;
    private SpriteRenderer Render; private Transform Form, WaterTarget;
    private float CurrentThirst, CurrentHunger, MaxThirst, MaxHunger, CurrentJumpPrepare, CurrentAfterJumpRest,
    ThirstReaction, HungerReaction; private bool DangerDetected = false; private Vector2 DangerPosition;
    GameObject DangerObject;
    [Header("-Sounds-")]
    private AudioSource AudioOrigin; public AudioClip[] AudioLib;

    private LayerMask waterLayer; private LayerMask obstacleLayer;
    private Vector2 Target; private Vector2 JumpStartPos; private float DistanceTraveled;
    private bool SearchingDrink;
    void Start() {
        AudioOrigin = this.gameObject.GetComponent<AudioSource>();
        Render = this.gameObject.GetComponent<SpriteRenderer>(); Form = this.gameObject.GetComponent<Transform>();
        MaxThirst = 100f; MaxHunger = 100f; ThirstReaction = 50f; HungerReaction = 80f;
        waterLayer = LayerMask.GetMask("Water"); obstacleLayer = LayerMask.GetMask("Obstacle");
        CurrentThirst = MaxThirst; CurrentHunger = MaxHunger;
        FindPrey();
    }
    void Update() {
        Form.localScale = new Vector3(1, 1 * Squize, 1);
        CurrentThirst -= 1f * Time.deltaTime; if (CurrentThirst <= 0) { 
            if (InstantDeath) { Death(); } } //else { ApplyDamage(); } }
        CurrentHunger -= 0.3f * Time.deltaTime; if (CurrentHunger <= 0) { 
            if (InstantDeath) { Death(); } } //else { ApplyDamage(); } }
        if (CurrentThirst <= ThirstReaction) { FindWater(); }
        if (CurrentHunger <= HungerReaction) { FindPrey(); Hungry = true; }
        if (SearchingPartner && !PartnerFinded) { FindPartner(); }
        if (SearchingPartner && PartnerFinded && PartnerTarget != null) { Target = PartnerTarget.transform.position; }
        DetectDanger(); ManageMove(); InstictionManage();
    } void DetectDanger() {
        Collider2D[] dangers = Physics2D.OverlapCircleAll(transform.position, ViewRadius);
        DangerDetected = false;
        foreach (var danger in dangers) {
            if (danger.CompareTag("Cookie")) {
                DangerDetected = true; DangerPosition = danger.transform.position; DangerObject = danger.gameObject;
                break;
            }
        }
    } private void FindPrey() {
        Collider2D[] Prey = Physics2D.OverlapCircleAll(transform.position, ViewRadius);
        PreyDetected = false; foreach (var NomNom in Prey) {
            if (NomNom.CompareTag("Cookie")) {
                PreyDetected = true; Target = NomNom.transform.position; 
                PreyTarget = NomNom.gameObject; break;
            }
        }
    } private void ManageMove() {
        if (AI) {
        switch (State) {
            case "PrepareJump":
                CurrentJumpPrepare -= Time.deltaTime;
                Render.sprite = SpriteLib[1];
                if (CurrentJumpPrepare <= 0) {
                    State = "Jumping"; JumpStartPos = transform.position; DistanceTraveled = 0f;
                } break;
                if (DangerDetected) {
            Vector2 dangerDirection = ((Vector2)transform.position - DangerPosition).normalized;
            Vector2 avoidTarget = (Vector2)transform.position + dangerDirection * ViewRadius; Target = avoidTarget;
            RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, dangerDirection, ViewRadius, obstacleLayer);
            if (hitObstacle.collider != null) { ChooseTarget(); } return;
        }
            case "Jumping":
                Render.sprite = SpriteLib[0];
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                Vector2 newPos = Target;//Vector2.MoveTowards(pos, Target, JumpSpeed * Time.deltaTime);
                //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
                transform.DOMove(Target, JumpSpeed/20f).SetEase(Ease.OutQuad);
                //transform.DOLocalMove(new Vector3(0, 0.5f, 0),JumpSpeed/10f).SetLoops(2, LoopType.Yoyo);
                float deltaDistance = Vector2.Distance(pos, newPos); DistanceTraveled += deltaDistance;
                
                if (DistanceTraveled >= JumpLength || Vector2.Distance(newPos, Target) < 0.01f) {
                    CurrentAfterJumpRest = AfterJumpRest; State = "Resting";
                    if (WaterTarget != null && Vector2.Distance(WaterTarget.position, transform.position) < 0.01f) {
                        CurrentThirst += 30; Destroy(WaterTarget.gameObject); WaterTarget = null; PlaySound(1);
                    }
                    if (PreyTarget != null && Vector2.Distance(PreyTarget.transform.position, transform.position) < 0.01f) { 
                        PreyTarget.GetComponent<CookieBrain>().Moving = false;
                        Destroy(PreyTarget); PreyTarget = null; PlaySound(0); Hungry = false;
                        BreedCounter++; if (!SearchingPartner) { BreedCollection(); }
                    }
                    if (PartnerTarget != null && Vector2.Distance(PartnerTarget.transform.position, transform.position) < 0.01f) {
                        SearchingPartner = false; PartnerFinded = false; PartnerTarget = null;
                        Instantiate(this.gameObject, transform.position, transform.rotation, transform.parent); 
                    }
                } break;
            case "Resting":
                //SpreadSpore();
                CurrentAfterJumpRest -= Time.deltaTime;
                if (CurrentAfterJumpRest <= 0) { 
                    if ((Vector2)transform.position == Target) { ChooseTarget(); } 
                    if (Target == null || Vector2.Distance(transform.position, Target) > ViewRadius) {
                        ChooseTarget();
                    } CurrentJumpPrepare = JumpPrepare; State = "PrepareJump";
            } break;
            default: break;
        }
        }
    } void FindWater() {
        Collider2D[] waterColliders = Physics2D.OverlapCircleAll(transform.position, ViewRadius, waterLayer);
        if (waterColliders.Length > 0) { 
            WaterTarget = waterColliders[0].transform; Target = WaterTarget.position;
        } else { WaterTarget = null; }
    } public void ChooseTarget() {
        for (int i = 0; i < 10; i++) {
            Vector2 candidate = (Vector2)transform.position + Random.insideUnitCircle * ViewRadius;
            Vector2 direction = candidate - (Vector2)transform.position; float distance = direction.magnitude;
            RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);
            if (hitObstacle.collider == null) { Target = candidate; return; }
        } Target = transform.position;
    } void Death() { Destroy(this.gameObject); } 
    void PlaySound(int SoundSetting) { AudioOrigin.PlayOneShot(AudioLib[SoundSetting]); }
    void BreedCollection() { if (BreedCounter >= 1) { SearchingPartner = true; BreedCounter = 0; } }
    void FindPartner() {
        Collider2D[] Partners = Physics2D.OverlapCircleAll(transform.position, ViewRadius);
        PartnerFinded = false; foreach (var Partner in Partners) {
            if (Partner.CompareTag("Danger") && Partner.gameObject != this.gameObject) {
                PartnerFinded = true; Target = Partner.transform.position; PartnerTarget = Partner.gameObject; break;
            } else { SpreadSpore();}//ChooseTarget(); }
        }
    }
    void SpreadSpore() {
        int randSporeChance = Random.Range(0, 5);
        if (BreedCounter >= 1 && randSporeChance == 3) {
            Render.sprite = SpriteLib[0]; Instantiate(SporeSeed, transform.position, transform.rotation, transform.parent);
            BreedCounter = 0;
        }
    }
    void InstictionManage() {
        GameTool GameManage = GameObject.Find("Main Camera").GetComponent<GameTool>();
        int cookieCount = GameManage.CookiesShelf.transform.childCount;
        if (cookieCount < 8) {
            HungerReaction = 30;
        } else if (cookieCount > 35) {
            HungerReaction = 90;
        } else if (cookieCount < 35 && cookieCount > 8) {
            HungerReaction = 70;
        }
    }
}
