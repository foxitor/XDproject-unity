using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.InputSystem;

public class RRPlayer : MonoBehaviour {
    public float CurrentSpeed = 0f, WalkSpeed = 2.6f, RunSpeed = 3f, CrouchSpeed = 1.6f, JumpHeight = 3.5f, 
    mouseSensitivity = 0.1f, InteractionLength = 3.6f, InteractionCooldown = 0.375f, gravity = -9.81f;
    CharacterController CharControll;
    public Camera Cam; RibbitsRealmInput inputActions;
    public RRPlayerModel ModelAnimator;

    Vector2 moveInput; Vector2 lookInput;
    float rotationX = 0f, curIntractCooldown = 0f;
    Vector3 velocity; bool isGrounded, isRunning, isCrouching;
    
    public GameObject yourBlockPrefab;
    //public RRChunk targetChunk;

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
        curIntractCooldown = InteractionCooldown;
        //Chunker = GameObject.Find("BlockLib").GetComponent<RRChunkManager>();
    }

    void OnEnable() { inputActions.Enable(); }
    void OnDisable() { inputActions.Disable(); }

    void Update() {
        CurrentSpeed = WalkSpeed;

        if (inputActions.Player.Jump.ReadValue<float>() > 0) { Jump(); }

        if (inputActions.Player.Punch.ReadValue<float>() > 0) { Punch(); }
        
        if (inputActions.Player.Place.ReadValue<float>() > 0) { PlaceBlock(); }

        if (curIntractCooldown > 0) { 
            curIntractCooldown -= Time.deltaTime;
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
        if (isGrounded) { velocity.y = JumpHeight; }
    }
    void Punch() {
        if (curIntractCooldown < 0.1) {
            ModelAnimator.SwingHand(0);
            RaycastHit hit;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward), out hit, InteractionLength)) {
                Debug.DrawRay(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.collider.gameObject.GetComponent<RRBlock>().Punch(1);
            } else {
                Debug.DrawRay(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward) * InteractionLength, Color.white);
            }
            curIntractCooldown = InteractionCooldown;
        }
    }
    void PlaceBlock() {
        if (curIntractCooldown < 0.1f) {
            ModelAnimator.SwingHand(0);
            RaycastHit hit;
            Vector3 placementPosition;

            if (Physics.Raycast(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward), out hit, InteractionLength)) {
                Debug.DrawRay(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                placementPosition = hit.point + hit.normal * 0.5f;
            } else {
                Debug.DrawRay(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward) * InteractionLength, Color.white);
                return;
            }

            Vector3Int blockPos = Vector3Int.RoundToInt(placementPosition);

            GameObject blockParent = GameObject.Find("BlockLib/Chunk");
            if (Vector3.Distance(transform.position, blockPos) > 0.75) {
                GameObject newBlock = Instantiate(yourBlockPrefab, blockPos, Quaternion.identity);
                AudioSource BlockSource = newBlock.GetComponent<AudioSource>();
                newBlock.transform.parent = blockParent.transform;
                RRBlock rrBlock = newBlock.GetComponent<RRBlock>(); rrBlock.BlockType = Blocks.gray_rock;

                BlockSource.PlayOneShot(
                    GameObject.Find("BlockLib").GetComponent<RRBlockLibrary>().GetSoundBlockTypeSound(rrBlock.BlockType, "Impact")
                );

                curIntractCooldown = InteractionCooldown;
            }
        }
    }

    void HandleMovement() {
        if (inputActions.Player.Run.ReadValue<float>() > 0) { isRunning = true; } else { isRunning = false; }
        if (inputActions.Player.Crouch.ReadValue<float>() > 0) { isCrouching = true; } else { isCrouching = false; }

        float FinalSpeed = WalkSpeed;
        if (isRunning) {  if (!isCrouching) { FinalSpeed = RunSpeed; } }
        else if (isCrouching) { if (!isRunning) { FinalSpeed = CrouchSpeed; } }

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