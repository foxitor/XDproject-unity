using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RRPlayerModel : MonoBehaviour {
    public GameObject[] BodyParts;
    public GameObject[] FpsBodyParts;

    RRPlayer Player; CharacterController PlayerController;

    float LookBoarder = 30;
    bool isPlayerMoving; Vector2 PastPlayerPos;
    float idleTimeLoop = 0f, idleAnimationSpeed = 0.5f, TimeAfterWalk = 0, PlayerLastSpeed = 0f;
    float walkTimeLoop = 0f, walkAnimationSpeed = 2f, stepSoundTimer = 0f; bool stepOneShot = false;
    public float StepFreq = 0.5f;
    bool isSwinging = false; float SwingTimeOut = 0f;

    void Start() {
        Player = transform.parent.GetComponent<RRPlayer>();
        PlayerController = Player.gameObject.GetComponent<CharacterController>();
    }
    void Update() {
        float xRotation = Player.Cam.transform.localEulerAngles.x;

        if (xRotation > 180f) {
            xRotation -= 360f;
        }

        float clampedX = Mathf.Clamp(xRotation, -LookBoarder, LookBoarder);
        BodyParts[0].transform.localEulerAngles = new Vector3(-clampedX, 0, 0);

        if (SwingTimeOut > 0) { SwingTimeOut -= Time.deltaTime; } else { isSwinging = false; }

        bool isPlayerMoving = PastPlayerPos != new Vector2(Player.transform.position.x, Player.transform.position.z);
        PastPlayerPos = new Vector2(Player.transform.position.x, Player.transform.position.z);

        if (isPlayerMoving) {
            ManageWalk();
        } else {
            ManageIdle(); stepSoundTimer = 0f; stepOneShot = true;
        }
    }

    public void SwingHand(int Hand) {
        FpsBodyParts[0].GetComponent<Animator>().speed = 1f;
        FpsBodyParts[Hand].GetComponent<Animator>().Play("Swing");
        SwingTimeOut = 0.375f; isSwinging = true;
    }
    void ManageIdle() {
        float CrouchOffset = Player.CurrentSpeed == Player.CrouchSpeed ? 0.05f : 0;
        //-Anim-
        Vector3 idleSwingAnimationFrame1 = new Vector3(0, 0.255f - CrouchOffset, 0);
        Vector3 idleSwingAnimationFrame2 = new Vector3(0, 0.245f - CrouchOffset, 0);
        //---
        
        if (TimeAfterWalk < 1f) { TimeAfterWalk += 0.25f * Time.deltaTime; } else { TimeAfterWalk = 1f; } 

        idleTimeLoop += idleAnimationSpeed * Mathf.Lerp(PlayerLastSpeed/2.5f, 1f, TimeAfterWalk) 
            * Time.deltaTime; if (idleTimeLoop > 1f) idleTimeLoop = 0f;

        float lerpValue = Mathf.Sin(idleTimeLoop * Mathf.PI);
        Player.Cam.transform.localPosition = Vector3.Lerp(idleSwingAnimationFrame1, idleSwingAnimationFrame2, lerpValue);
        FpsBodyParts[0].GetComponent<Animator>().speed = Mathf.Lerp(PlayerLastSpeed/2.6f, 1f, TimeAfterWalk); 
    }
    void ManageWalk() {
        float CrouchOffset = Player.CurrentSpeed == Player.CrouchSpeed ? 0.05f : 0;
        //-Anim-
        Vector3 walkSwingAnimationFrame1 = new Vector3(0.015f, 0.255f - CrouchOffset, 0);
        Vector3 walkSwingAnimationFrame2 = new Vector3(-0.015f, 0.245f - CrouchOffset, 0);
        //---

        walkTimeLoop += walkAnimationSpeed * Player.CurrentSpeed/2.6f * Time.deltaTime; if (walkTimeLoop > 1f) walkTimeLoop = 0f;

        float lerpValue = Mathf.Sin(walkTimeLoop * Mathf.PI);
        Player.Cam.transform.localPosition = Vector3.Lerp(walkSwingAnimationFrame1, walkSwingAnimationFrame2, lerpValue);
        if (!isSwinging) {
            FpsBodyParts[0].GetComponent<Animator>().speed = Player.CurrentSpeed/2.6f; 
            FpsBodyParts[0].GetComponent<Animator>().Play("WalkCycle");
        }

        TimeAfterWalk = 0f; PlayerLastSpeed = Player.CurrentSpeed;

        stepSoundTimer += Time.deltaTime;
        if (Player.CurrentSpeed != Player.CrouchSpeed) {
            if (stepSoundTimer >= StepFreq / (Player.CurrentSpeed/2.6f) || stepOneShot == true) {
                RaycastHit hit;
                if (Physics.Raycast(Player.transform.position, Player.transform.TransformDirection(-Vector3.up), out hit, 1f)) {
                    if (hit.collider.gameObject.GetComponent<RRBlock>() != null) {
                        RRBlock rrBlockComp = hit.collider.gameObject.GetComponent<RRBlock>();
                        hit.collider.gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                        hit.collider.gameObject.GetComponent<AudioSource>().PlayOneShot(GameObject.Find("BlockLib")
                            .GetComponent<RRBlockLibrary>().GetSoundBlockTypeSound(rrBlockComp.BlockType, "Hit"));
                    }
                } stepSoundTimer = 0f;
                stepOneShot = false;
            }
        }
    }
}
