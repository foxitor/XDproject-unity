using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.InputSystem;

public class RRPlayer : MonoBehaviour {
    public float CurrentSpeed = 0f, WalkSpeed = 2.6f, RunSpeed = 3f, CrouchSpeed = 1.6f, mouseSensitivity = 0.1f, gravity = -9.81f;

    CharacterController CharControll;
    public Camera Cam; RibbitsRealmInput inputActions;

    Vector2 moveInput; Vector2 lookInput;
    float rotationX = 0f;
    Vector3 velocity; bool isGrounded, isRunning, isCrouching;

    void Awake() {
        CharControll = GetComponent<CharacterController>();

        inputActions = new RibbitsRealmInput();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        //inputActions.Player.Jump.performed += ctx => Jump();
    }
    void Start() {
        Cam = Camera.main;
    }

    void OnEnable() { inputActions.Enable(); }
    void OnDisable() { inputActions.Disable(); }

    void Update() {
        CurrentSpeed = WalkSpeed;
        if (inputActions.Player.Jump.ReadValue<float>() > 0) {
            Jump();
        }
        HandleMovement(); HandleMouseLook();
        Cursor.lockState = CursorLockMode.Locked;
        CharControll.Move(velocity * Time.deltaTime);
    }
    void FixedUpdate() {
        isGrounded = CharControll.isGrounded;
        if(!isGrounded) {
            velocity.y += gravity * Time.deltaTime;
            if (velocity.y > 10) {
                velocity.y = 10;
            }
        } else { velocity.y = 0; }
    }
    void Jump() {
        if (isGrounded) {
            velocity.y = 3f;
        }
    }

    void HandleMovement() {
        if (inputActions.Player.Run.ReadValue<float>() > 0) { isRunning = true; } else { isRunning = false; }
        if (inputActions.Player.Crouch.ReadValue<float>() > 0) { isCrouching = true; } else { isCrouching = false; }
        float FinalSpeed;
        if (isRunning) { FinalSpeed = RunSpeed; }
        else if (isCrouching) { FinalSpeed = CrouchSpeed; }
        else { FinalSpeed = WalkSpeed; }
        CurrentSpeed = FinalSpeed;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y; CharControll.Move(move * CurrentSpeed * Time.deltaTime);
    }

    void HandleMouseLook() {
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        rotationX -= lookInput.y * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        Cam.transform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }
}
