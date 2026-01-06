using System.Collections; using System.Collections.Generic; using UnityEngine;
public enum StatesParent {
    IdleSingPack,
    IdleSingMissPack,
    DoubleIdleSingPack,
    DoubleIdleSingMissPack,
    DoubleIdle,
    Idle
}
public class CharacterVizual : MonoBehaviour {
    public StatesParent StateParent;
    public string[] AddonStates;
    [Space]
    public Sprite[] IdleStateLib;
    public Sprite[] SecondIdleStateLib;
    [Space]
    public Sprite[] SingUpStateLib;
    public Sprite[] SingDownStateLib;
    public Sprite[] SingLeftStateLib;
    public Sprite[] SingRightStateLib;
    [Space]
    public Sprite[] MissSingUpStateLib;
    public Sprite[] MissSingDownStateLib;
    public Sprite[] MissSingLeftStateLib;
    public Sprite[] MissSingRightStateLib;

    [Space]
    public float animationFPS = 24f;

    SpriteRenderer spriteOutput;
    int currentFrameIndex = 0;
    float timer = 0f;
    Sprite[] currentAnimationFrames; 

    public bool isSinging = false;

    void Start() {
        spriteOutput = GetComponent<SpriteRenderer>();
        PlayAnimation("Idle");
    }

    void Update() {
        if (currentAnimationFrames != null && currentAnimationFrames.Length > 0) {
            timer += Time.deltaTime;
            if (timer >= 1f / animationFPS) {
                timer -= 1f / animationFPS;
                if (currentFrameIndex < currentAnimationFrames.Length) { currentFrameIndex++; }
                else if (currentFrameIndex >= currentAnimationFrames.Length && isSinging) { StartCoroutine(StopSingAnimation()); }
                spriteOutput.sprite = currentAnimationFrames[currentFrameIndex - 1];
            }
        }
    }
    public void PlayAnimation(string State) {
        switch (State) {
            case "Idle" : 
                currentAnimationFrames = IdleStateLib;
            break;
            case "Up" : 
                currentAnimationFrames = SingUpStateLib;  isSinging = true;
            break;
            case "Down" : 
                currentAnimationFrames = SingDownStateLib; isSinging = true;
            break;
            case "Left" : 
                currentAnimationFrames = SingLeftStateLib; isSinging = true;
            break;
            case "Right" : 
                currentAnimationFrames = SingRightStateLib; isSinging = true;
            break;

            case "MissUp" : 
                currentAnimationFrames = MissSingUpStateLib;  isSinging = true;
            break;
            case "MissDown" : 
                currentAnimationFrames = MissSingDownStateLib; isSinging = true;
            break;
            case "MissLeft" : 
                currentAnimationFrames = MissSingLeftStateLib; isSinging = true;
            break;
            case "MissRight" : 
                currentAnimationFrames = MissSingRightStateLib; isSinging = true;
            break;
        }
        currentFrameIndex = 0; spriteOutput.sprite = currentAnimationFrames[currentFrameIndex];
    }
    IEnumerator StopSingAnimation() {
        yield return new WaitForSeconds(0.05f);
        isSinging = false;
    }
}
