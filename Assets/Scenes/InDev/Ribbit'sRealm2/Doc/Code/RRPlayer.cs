using UnityEngine; using UnityEngine.UI; using UnityEngine.InputSystem;

enum RRPose {
    walk,
    run,
    crouch
}

public class RRPlayer : MonoBehaviour {

    public float CurSpeed = 2.3f, JumpStrength = 3.6f,
        WalkSpeed = 2.3f, SprintSpeed = 4.2f, CrouchSpeed = 1.6f,
        CurJumpMultiplier = 1f,
        CurSpeedMultiplier = 1f,
        CurCrouchSpeedMultipler = 1f, FallCrouchSpeed = 1f;

    float lookRotation;
    RibbitsRealmInput inputActions;
    Vector2 move, look; Vector3 velocity;
    RRPose curPos = RRPose.walk;

    Transform plrGroup, headGroup, cam;
    Rigidbody Phy;
    bool isGrounded;

    float jumpTimeOut;

    void Awake() {
        inputActions = new RibbitsRealmInput();
    }
    public void OnLook(InputAction.CallbackContext Ctx) { look = Ctx.ReadValue<Vector2>(); }
    public void OnMove(InputAction.CallbackContext Ctx) { move = Ctx.ReadValue<Vector2>(); }
    //public void OnJump(InputAction.CallbackContext Ctx) { Jump(); }

    void OnEnable() { inputActions.Enable(); }
    void OnDisable() { inputActions.Disable(); }

    void Start() {
        plrGroup = this.transform.parent;
        headGroup = this.transform.GetChild(0);
        cam = headGroup.GetChild(0);
        Phy = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        if (inputActions.Player.Jump.ReadValue<float>() > 0) { Jump(); }

        if (jumpTimeOut > 0) { jumpTimeOut -= 0.1f * Time.deltaTime; }
    }
    void FixedUpdate() {
        HandleMovement();
        HandleCameraMovement();
        if (isGrounded) {
            if (Phy.velocity.y < 0) {
                Phy.velocity = new Vector3(Phy.velocity.x, 0, Phy.velocity.z);
            }
        }
    }
    void HandleMovement() {
        if (inputActions.Player.Crouch.ReadValue<float>() > 0) { curPos = RRPose.crouch; }
        else if (inputActions.Player.Sprint.ReadValue<float>() > 0) { curPos = RRPose.run; }
        else { curPos = RRPose.walk; }

        switch (curPos) {
            case RRPose.crouch : CurSpeed = (CrouchSpeed * CurCrouchSpeedMultipler) * FallCrouchSpeed; break;
            case RRPose.run : CurSpeed = SprintSpeed; break;
            default : CurSpeed = WalkSpeed; break;
        }
        VerifyGrounded();
        if (curPos == RRPose.crouch && !isGrounded && Phy.velocity.y == 0f) {
            FallCrouchSpeed = 0;
        } else { FallCrouchSpeed = 1; }

        //Debug.Log(transform.localEulerAngles + "");

        Vector3 CurVel = Phy.velocity;
        Vector3 TargVel = new Vector3(move.x, 0, move.y);
        TargVel *= CurSpeed * CurSpeedMultiplier;

        TargVel = transform.TransformDirection(TargVel);
        Vector3 VelChange = (TargVel - CurVel);
        VelChange = new Vector3(VelChange.x, 0, VelChange.z);

        Phy.AddForce(VelChange, ForceMode.VelocityChange);
    }
    void HandleCameraMovement() {
        transform.localEulerAngles += (Vector3.up * look.x * 0.1f);
        lookRotation -= look.y * 0.1f;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        cam.localEulerAngles = new Vector3(lookRotation, 0, 0);
    }
    void Jump() {
        Vector3 jumpForce = Vector3.zero;
        VerifyGrounded();
        if (isGrounded && jumpTimeOut <= 0f) {
            jumpTimeOut = 0.01f;
            jumpForce = Vector3.up * JumpStrength * CurJumpMultiplier;
        }
        Phy.AddForce(jumpForce, ForceMode.VelocityChange);
    }
    bool VerifyGrounded() {
        RaycastHit hit;
        isGrounded = 
            Physics.Raycast(this.transform.position, this.transform.TransformDirection(-Vector3.up), 
                out hit, 0.501f, LayerMask.GetMask("Default")) ? true : false;
        return isGrounded;
    }
}
