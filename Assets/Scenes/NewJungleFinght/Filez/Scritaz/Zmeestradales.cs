using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.SceneManagement; using UnityEngine.InputSystem;

public class Zmeestradales : MonoBehaviour {
    //#Protection
    int MaxHealth = 3, CurHealth;
    public Transform ShieldPivot, Shield;
    public EgoricalGame Game;
    public AudioClip[] AudioLib;

    //#Movement
    public float offset, zDistance = 10f;
    public LayerMask obstacleLayer;
    public bool Move = true; public Camera Cam;
    bool OneshotDamaged = false;

    //#Controlls
    JungleFightControlls Controls; Gamepad gamepad;
    public Vector2 moveInput = Vector2.zero;
    bool isJoystickConnected = Gamepad.all.Count > 0;

    //GamePad
    void Awake() { 
        Controls = new JungleFightControlls();
        Controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        Controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
        //Controls.Gameplay.Impulse.performed += Ctx => ImpulseJump(true);
        //Controls.Gameplay.Dash.performed += Ctx => StartCoroutine(Dash());
    }
    void OnEnable() { Controls.Gameplay.Enable();} 
    void OnDisable() { Controls.Gameplay.Disable(); }
    //-

    void Start() {
        gamepad = Gamepad.current;
        CurHealth = MaxHealth;
        this.gameObject.AddComponent<AudioSource>();
    }
    void Update() { 
        if (CurHealth < 1) { Game.ReloadScene(); } 
        if (Move) {
            Vector3 thispos = transform.position; Vector3 directionToGoal = Shield.position - thispos;
            RaycastHit2D hit = Physics2D.Raycast(thispos, directionToGoal.normalized, directionToGoal.magnitude, obstacleLayer);
            if (hit.collider != null) {
                if (!OneshotDamaged) {
                    StartCoroutine(CooldownedDamage());
                    OneshotDamaged = true;
                }
            } else {
                Vector3 newPos = Vector3.MoveTowards(thispos, Shield.position, 4f * Time.deltaTime);
                transform.position = newPos;
            }
            //Gamepad
            if (isJoystickConnected) {
                Vector2 input = moveInput;
                if (input.sqrMagnitude > 0.1f) {
                    float rotateZ = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
                } 
            } else {
                //Mouse
                Vector3 mouseScreenPos = Input.mousePosition; mouseScreenPos.z = zDistance;
                Vector3 worldPos = Cam.ScreenToWorldPoint(mouseScreenPos);
                Vector3 difference = worldPos - transform.position;
                float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
            }
        }
        Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
    }
    public void Damage() { 
        VibrateController(0.35f, 0.35f, 0.25f); CurHealth--; 
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(AudioLib[0]); 
    }
    public int ShareHealthValue() { return CurHealth; }
    IEnumerator CooldownedDamage() {
        Damage();
        yield return new WaitForSeconds(1);
        OneshotDamaged = false;
    }
    public void VibrateController(float leftMotor, float rightMotor, float duration) {
        if (gamepad != null) { gamepad.SetMotorSpeeds(leftMotor, rightMotor); Invoke("StopVibration", duration); }
    }
    public void StopVibration() {
        if (gamepad != null) { gamepad.SetMotorSpeeds(0, 0); }
    }
}
