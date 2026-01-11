using UnityEngine; using UnityEngine.SceneManagement; using System.Collections; using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Baebrickxxxx : MonoBehaviour {
    public float flapForce = 5f; public float tiltSmooth = 2f; 
    public float maxRotation = 30f; public float minRotation = -90f;
    public GamManag Game;
    private Rigidbody2D rb; private float currentRotation;
    public GameObject StartGuideLine; public bool Move;
    ChertoletControls Controls; Gamepad gamepad;
    
    //GamePad
    void Awake() { 
        Controls = new ChertoletControls();
        Controls.GamePlay.Impulse.performed += Ctx => Flap();
    }
    void OnEnable() { Controls.GamePlay.Enable();} 
    void OnDisable() { Controls.GamePlay.Disable(); }
    //-

    void Start() { gamepad = Gamepad.current; rb = GetComponent<Rigidbody2D>(); currentRotation = 0f; StartGuideLine.SetActive(true);
    StartCoroutine(HideGuide()); }
    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) { Flap(); }
        float targetRotation = Mathf.Lerp(minRotation, maxRotation, (rb.velocity.y + 5) / 10);
        targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, tiltSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentRotation, currentRotation, currentRotation);
        if (!Move) { rb.simulated = false; } else { rb.simulated = true; }
    } 
    void Flap() { 
        if (Move) { rb.velocity = Vector2.up * flapForce; } 
    }
    IEnumerator HideGuide() { yield return new WaitForSeconds(3); StartGuideLine.SetActive(false); }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground")) {
            Game.ReloadScene();
        }
    }
    public void VibrateController(float leftMotor, float rightMotor, float duration) {
        if (gamepad != null) { gamepad.SetMotorSpeeds(leftMotor, rightMotor); Invoke("StopVibration", duration); }
    }
    public void StopVibration() {
        if (gamepad != null) { gamepad.SetMotorSpeeds(0, 0); }
    }
}