using UnityEngine;

public class InteractiveNote : MonoBehaviour {
    public HudBehavior Hud;
    public int NoteId;
    public bool CanInteract;
    public GameRythm Game;
    string AnimationSuplier;

    void Start() {
        //Transform NoteId to Animation
        switch (NoteId) {
            case 0 : AnimationSuplier = "Left"; break;
            case 1 : AnimationSuplier = "Down"; break;
            case 2 : AnimationSuplier = "Up"; break;
            case 3 : AnimationSuplier = "Right"; break;

            case 4 : AnimationSuplier = "Right"; break;
            case 5 : AnimationSuplier = "Down"; break;
            case 6 : AnimationSuplier = "Up"; break;
            case 7 : AnimationSuplier = "Left"; break;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Finish")) {
            Game.Characters[1].PlayAnimation(AnimationSuplier);
            Hud.FlashNote(NoteId);
            Destroy(this.gameObject);
            if (Game.MicroGame == MicroGames.Random) {
                Game.OpponentVoiceSpeaker.SingNote(NoteId);
            }
        }
        if (other.CompareTag("Player")) {
            CanInteract = true;
        }
    }

    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Game.ScrollSpeed * 3) * Time.deltaTime, transform.position.z);
    }
    public string ShareAnimation() { return AnimationSuplier; }
}
