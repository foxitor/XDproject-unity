using System.Collections; using System.Collections.Generic; using UnityEngine;

public class ShigimaJumpalka : MonoBehaviour {
    public TulevoGame Game;
    [Space]
    //#MoveSetup
    public bool DoMove = true;
    public LayerMask ObsticaleLayer;
    //public float Slowdown, SecondSlowdown;
    public float RotateSpeed, JumpStreingth;
    
    Rigidbody2D rb;
    public float CurrentSpeed, DefaultSpeed = 8.6f, SuperPowerSpeed = 10.4f;
    bool SuperPowered = false, grounded, FrontObsticale, OrbActive = false, GravityOrbActive = false; 
    Camera GameCamera;
    GameObject CurBox = null;
    int Gravity = 1;
    public bool Direction;

    public float GroundCheckRadius;
    public GameObject CheerMsg;
    float DashCoolDown;
    Color ActiveColor = new Color(0.75f, 0.45f, 0.45f, 1f);
    Color DullColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    SpriteRenderer ShigimaVisual; Transform ShigimaVisualTransform;
    [Space]
    //#Death
    bool oneShotDead = false;
    public AudioClip DeathSound;

    void Start() {
        ShigimaVisual = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        ShigimaVisualTransform = transform.GetChild(0).transform;
        GameCamera = Game.Attributaje.gameObject.GetComponent<Camera>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        CurrentSpeed = DefaultSpeed;
    }
    void Update() {
        ManageSpeed(); ManageJumpos();
        if (SuperPowered) { 
            if (DashCoolDown > 0) {
                DashCoolDown -= Time.deltaTime;
                float t = 1 - (DashCoolDown / 2.5f);
                ShigimaVisual.color = Color.Lerp(DullColor, ActiveColor, t);
            } else { ShigimaVisual.color = ActiveColor; } 
        }
        if (DashCoolDown > 0) {
            DashCoolDown = DashCoolDown - 1 * Time.deltaTime;
        }

        if (DoMove && !FrontObsticale) { 
            transform.Translate(transform.right * (Direction ? -CurrentSpeed : CurrentSpeed) * Time.deltaTime); 
            GameCamera.transform.position = new Vector3(transform.position.x+(Direction ? -0.5f : 0.5f), transform.position.y, GameCamera.transform.position.z);
        } if (!grounded) {
            float FinalRotation = 0;
            if (!Direction && Gravity == 1) { FinalRotation = RotateSpeed; }
            if (!Direction && Gravity == -1) { FinalRotation = RotateSpeed; }
            if (Direction && Gravity == -1) { FinalRotation = RotateSpeed; }
            if (Direction && Gravity == 1) { FinalRotation = -RotateSpeed; }
            ShigimaVisualTransform.Rotate(0, 0, FinalRotation * Time.deltaTime);
        } else { SnapToNearestAngle(); }
    } 
    void FixedUpdate() {
        if(rb.velocity.y < -24.2f) { rb.velocity = new Vector2(rb.velocity.x, -24.2f); }

        grounded = Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 
            0, ObsticaleLayer);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Direction ? -Vector2.right : Vector2.right), 0.55f, ObsticaleLayer);
        FrontObsticale = hit.collider != null;
        
        Vector3 startPoint = transform.position;
        Vector2 direction2D = Direction ? -Vector2.right : Vector2.right;
        Vector3 direction3D = new Vector3(direction2D.x, direction2D.y, 0);
        Vector3 endPoint = startPoint + direction3D * 0.55f;
        Debug.DrawLine(startPoint, endPoint, Color.red);

        if (FrontObsticale && !oneShotDead) {
            oneShotDead = true;
            StartCoroutine(Dying());
        }
    }
    void SnapToNearestAngle() {
        if (ShigimaVisualTransform != null) {
            Vector3 Rotation = ShigimaVisualTransform.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
            ShigimaVisualTransform.rotation = Quaternion.Euler(Rotation);
        }
    }
    void ManageSpeed() {
        if (Game.ShareSearchedLayer() > 4) { SuperPowered = true; } else { SuperPowered = false; }
    }
    void ManageJumpos() {
        if (DoMove) {
            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))) {
                if (!FrontObsticale && CurBox == null) {
                    if (grounded) {
                        rb.velocity = Vector2.zero; rb.AddForce(Vector2.up * (JumpStreingth) * Gravity, ForceMode2D.Impulse);
                    }
                    if (OrbActive) {
                        rb.velocity = Vector2.zero; rb.AddForce(Vector2.up * (JumpStreingth) * Gravity, ForceMode2D.Impulse);
                        OrbActive = false;
                    }
                    if (GravityOrbActive) {
                        rb.velocity = Vector2.zero;
                        Gravity = Gravity == 1 ? -1 : 1; rb.gravityScale = rb.gravityScale * -1;
                        GravityOrbActive = false;
                    }
                } else if (CurBox != null) {
                    CurBox.GetComponent<TulevoObjectModifier>().OpenBox();
                    CurBox = null;
                }
            }
            if (SuperPowered) {
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Z)) {
                    if (DashCoolDown <= 0) {
                        StartCoroutine(Dash());
                    }
                }
            }
        }
    } public void cheerUp() { CheerMsg.SetActive(true); StartCoroutine(HideCheer()); }

    //#interact
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Interactive")) {
            TulevoObjectModifier Modifier = collision.gameObject.GetComponent<TulevoObjectModifier>();
            if (Modifier != null) {
                switch (Modifier.ObjectType) {
                    case TulevoObjectTypes.InnerPortal : 
                        transform.position = Modifier.PortalLink.position;
                        if (Modifier.AddsLayer) { Game.AddLayer(); }
                    break;
                    case TulevoObjectTypes.Spike :
                        StartCoroutine(Dying()); 
                    break;
                    case TulevoObjectTypes.Saw :
                        StartCoroutine(Dying()); 
                    break;
                    case TulevoObjectTypes.Orb :
                        OrbActive = true;
                    break;
                    case TulevoObjectTypes.Interactive :
                        if (Modifier.CustomAlghoritm == "Box") {
                            CurBox = collision.gameObject;
                        }
                    break;
                    case TulevoObjectTypes.ReversePortal :
                        Direction = !Direction;
                    break;
                    case TulevoObjectTypes.GravityOrb :
                        GravityOrbActive = true;
                    break;
                    case TulevoObjectTypes.GravityPortal :
                        rb.velocity = Vector2.zero;
                        Gravity = Gravity == 1 ? -1 : 1; rb.gravityScale = rb.gravityScale * -1;
                    break;
                    case TulevoObjectTypes.Item :
                        if (Modifier.CustomAlghoritm == "Bullet") {
                            Game.CurRevolverAmmo++;
                            Destroy(Modifier.gameObject);
                        }
                    break;
                }
            }
        }
    } void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Interactive")) {
            TulevoObjectModifier Modifier = collision.gameObject.GetComponent<TulevoObjectModifier>();
            if (Modifier != null) {
                switch (Modifier.ObjectType) {
                    case TulevoObjectTypes.Orb :
                        OrbActive = false;
                    break;
                    case TulevoObjectTypes.GravityOrb :
                        GravityOrbActive = false;
                    break;
                    case TulevoObjectTypes.Interactive :
                        CurBox = null;
                    break;
                }
            }
        }
    }
    IEnumerator HideCheer() {
        yield return new WaitForSeconds(1f);
        CheerMsg.SetActive(false);
    }
    IEnumerator Dash() {
        CurrentSpeed = SuperPowerSpeed;
        DashCoolDown = 2.5f; ShigimaVisual.color = DullColor;
        yield return new WaitForSeconds(0.25f);
        CurrentSpeed = DefaultSpeed;
    }
    IEnumerator Dying() {
        rb.bodyType = RigidbodyType2D.Static; Game.Music.Stop();
        DoMove = false; Game.DeadMessange.SetActive(true);
        Game.Music.clip = DeathSound; Game.Music.Play();
        yield return new WaitForSeconds(2.5f);
        Game.RestartScene();
    } 
}
