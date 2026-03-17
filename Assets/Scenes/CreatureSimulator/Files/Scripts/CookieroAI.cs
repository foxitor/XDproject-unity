using UnityEngine; using UnityEngine.UI;

public class CookieroAI : MonoBehaviour {
    public GameTool GameSetting;
    public float Radius = 2.5f; public float speed = 3.5f; 
    public float WaitTime = 2f; private float viewRadius = 2.5f;
    public float P_Gen; public GameObject Clone;
    public Sprite NullParenting; public Sprite Parenting; public Sprite StrongParenting;

    public LayerMask obstacleLayer; public LayerMask waterLayer; public LayerMask bushLayer;

    public AudioClip drinkSound; public AudioClip eatSound; 
    private AudioSource audioSource;

    Vector2 Target; bool Moving = false; float Timer;

    float Thirst = 50f; public float thirstDecayRate = 1f; public float thirstThreshold = 50f;

    float hunger = 0f; public float hungerDecayRate = 0.5f;
    public float hungerThreshold = 50f;
    private Transform WaterTarget;

    public string CurDialogue;

    public float PreviousP_Gen; public int page = 1;

    void Start() {
        GameSetting = GameObject.Find("Main Camera").GetComponent<GameTool>();
        ChooseTarget(); Timer = WaitTime; 
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); }
        WaterTarget = null; P_Gen = Random.Range(1,4); 
        SpriteRenderer SpriteService = GetComponent<SpriteRenderer>();
        switch (P_Gen) {
            case 1: SpriteService.sprite = NullParenting; break;
            case 2: SpriteService.sprite = Parenting; break;
            case 3: SpriteService.sprite = StrongParenting; break;
        }
    }

    void Update() {
        if (hunger >= 100f || Thirst <= 10f) { Die(); return; }
        Thirst -= thirstDecayRate * Time.deltaTime; if (Thirst < 0) Thirst = 0;
        hunger += hungerDecayRate * Time.deltaTime; if (hunger > 100) hunger = 100;
        if (WaterTarget != null) {
            Vector2 directionToWater = (WaterTarget.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, WaterTarget.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, WaterTarget.position) < 0.1f) {
                Thirst = 100f; PlayDrinkSound(); Destroy(WaterTarget.gameObject); WaterTarget = null;
            }
            return;
        } float distanceToTarget = Vector2.Distance(transform.position, Target);
        if (!Moving || distanceToTarget < 0.1f) {
            if (distanceToTarget < 0.1f && Moving) {
                if (IsBushAtPosition(Target)) { EatBush(Target); }
                Moving = false; Timer = WaitTime;
            }
            Timer -= Time.deltaTime;
            if (Timer <= 0 && !Moving) {
                if (hunger >= hungerThreshold) { FindFood(); } 
                else if (Thirst <= thirstThreshold) { FindWater(viewRadius); } else { ChooseTarget(); }
                Moving = true;
            }
        } else {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 newPos = Vector2.MoveTowards(pos, Target, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        } ManageDialogue(CurDialogue);
    }

    void ChooseTarget() {
        for (int i = 0; i < 10; i++) {
            Vector2 candidate = (Vector2)transform.position + Random.insideUnitCircle * Radius;
            Vector2 direction = candidate - (Vector2)transform.position;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);
            if (hit.collider == null) {
                Target = candidate; return;
            }
        }
        Target = transform.position; Moving = false;
    }

    void FindWater(float Radius) {
        Collider2D[] waterColliders = Physics2D.OverlapCircleAll(transform.position, Radius, waterLayer);
        if (waterColliders.Length > 0) { WaterTarget = waterColliders[0].transform; } else { 
            WaterTarget = null; ChooseTarget(); }
    }

    void FindFood() {
        Collider2D[] bushes = Physics2D.OverlapCircleAll(transform.position, Radius, bushLayer);
        float ClosestDist = Mathf.Infinity; Vector2 ClosestBushPos = transform.position;
        foreach(var bush in bushes) {
            Bush BushScript = bush.GetComponent<Bush>();
            if(BushScript != null && BushScript.Stage == 3) {
                float Distancion = Vector2.Distance(transform.position,bush.transform.position);
                if(Distancion < ClosestDist) { ClosestDist = Distancion; ClosestBushPos = bush.transform.position; }
            }
        } if(ClosestDist != Mathf.Infinity) { Target = ClosestBushPos; Moving = true; } else { ChooseTarget(); }
    }
    bool IsWaterAtPosition(Vector2 pos) { Collider2D hitWater = Physics2D.OverlapPoint(pos, waterLayer); 
        return hitWater != null;
    }
    bool IsBushAtPosition(Vector2 pos) {
        Collider2D hitBushPoint = Physics2D.OverlapPoint(pos, bushLayer);
        return hitBushPoint != null && hitBushPoint.GetComponent<Bush>() != null && hitBushPoint.GetComponent<Bush>().Stage == 3;
    }
    void EatBush(Vector2 position) {
        Collider2D[] bushesAtPos = Physics2D.OverlapCircleAll(position, 0.1f, bushLayer);
        foreach(var col in bushesAtPos){
            Bush BushScript = col.GetComponent<Bush>();
            if (BushScript != null && BushScript.Stage == 3) {
                hunger -= 50f; if(hunger < 0) hunger = 0;
                BushScript.Eat(); PlayEatSound();
                break;
            }
        }
        ChooseTarget();
        Moving = true;
        if (P_Gen > 1 && Random.Range(1,5) == 3) { 
            Instantiate(Clone, this.gameObject.GetComponent<Transform>().position, 
            this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>().parent); 
        } if (P_Gen > 2 && Random.Range(1,4) == 3) { 
            Instantiate(Clone, this.gameObject.GetComponent<Transform>().position, 
            this.gameObject.GetComponent<Transform>().rotation, this.gameObject.GetComponent<Transform>().parent); 
        }
    }
    void Die() { Destroy(gameObject); }
    void PlayDrinkSound() { if (drinkSound != null && audioSource != null) audioSource.PlayOneShot(drinkSound); }
    void PlayEatSound() { if(eatSound != null && audioSource != null) audioSource.PlayOneShot(eatSound); }
    public void RainyBehavior(bool enable) { if (enable) { viewRadius = viewRadius/2; Radius = Radius/2; } else {
        viewRadius = 2.5f; Radius = 2.5f;
    }}
    public void GenderInteract() {
        GameObject GCenv = GameObject.Find("Main Camera/Canvas/GenderChangeEnv"); 
        Text GCenvText = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/DialogueWindow/Text").GetComponent<Text>(); 
        GameObject GCenv1 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting1"); 
        GameObject GCenv2 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting2");
        GameObject GCenv3 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting3");
        GCenv1.SetActive(false); GCenv2.SetActive(false); GCenv3.SetActive(false);
        switch (P_Gen) {
            case 1: GCenv1.SetActive(true); break; case 2: GCenv2.SetActive(true); break; 
            case 3: GCenv3.SetActive(true); break;
        }
        GCenv.SetActive(true);
        CurDialogue = "ChangeGender";
        SpriteRenderer SpriteService = GetComponent<SpriteRenderer>();
    }
    public void ManageDialogue(string Name) {
        SpriteRenderer SpriteService = GetComponent<SpriteRenderer>();
        GameObject GCenv = GameObject.Find("Main Camera/Canvas/GenderChangeEnv"); 
        Text GCenvActions = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/DialogueWindow/Actions").GetComponent<Text>(); 
        Text GCenvText = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/DialogueWindow/Text").GetComponent<Text>(); 
        GameObject GCenv1 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting1"); 
        GameObject GCenv2 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting2");
        GameObject GCenv3 = GameObject.Find("Main Camera/Canvas/GenderChangeEnv/Parenting3"); 
        if (Name == "ChangeGender") {
            if (page == 1) {
                PreviousP_Gen = P_Gen;
                GCenvText.text = "Hey God!";
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (page == 1) {
                page = 2; P_Gen = 1;
                SpriteService.sprite = NullParenting;
                GCenv1.SetActive(false); GCenv2.SetActive(false); GCenv3.SetActive(false);
                switch (P_Gen) {
                    case 1: GCenv1.SetActive(true); break; case 2: GCenv2.SetActive(true); break; 
                    case 3: GCenv3.SetActive(true); break;
                } if (PreviousP_Gen == 2) {
                    GCenvText.text = "TF YOU TURN'D ME INTO A BOY!?";
                } if (PreviousP_Gen == 3) {
                    GCenvText.text = "TF YOU TURN'D ME INTO A BOY!? Am i was.. preety hot.?";
                } }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                if (page == 1) {
                page = 2; P_Gen = 2;
                SpriteService.sprite = Parenting;
                GCenv1.SetActive(false); GCenv2.SetActive(false); GCenv3.SetActive(false);
                switch (P_Gen) {
                    case 1: GCenv1.SetActive(true); break; case 2: GCenv2.SetActive(true); break; 
                    case 3: GCenv3.SetActive(true); break;
                } if (PreviousP_Gen == 1) {
                    GCenvText.text = "TF YOU TURN'D ME INTO A GIRL!?";
                } if (PreviousP_Gen == 3) {
                    GCenvText.text = "AHHH TF YOU MAKED TO ME?";
                }}
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (page == 1) {
                page = 2; P_Gen = 3;
                SpriteService.sprite = StrongParenting;
                GCenv1.SetActive(false); GCenv2.SetActive(false); GCenv3.SetActive(false);
                switch (P_Gen) {
                    case 1: GCenv1.SetActive(true); break; case 2: GCenv2.SetActive(true); break; 
                    case 3: GCenv3.SetActive(true); break;
                } if (PreviousP_Gen == 1) {
                    GCenvText.text = "WHAT IS THIS BODY!?!? IM SO HOT!!";
                } if (PreviousP_Gen == 2) {
                    GCenvText.text = "HEY... what did you.?";
                }}
            }
            if (Input.GetKeyDown(KeyCode.Return)) {
                Name = ""; GCenv.SetActive(false); CurDialogue = ""; 
                GameSetting.DialogueActive = false; page = 1;
            } if (page == 1) {
                GCenvActions.text = "Press:\n1,2,3 to change P-gen.";
            } if (page == 2) {
                GCenvActions.text = "Press:\nEnter to exit";
            }
        }
    }
}