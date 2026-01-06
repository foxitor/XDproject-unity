using System.Collections; using System.Collections.Generic; using UnityEngine;

public class FnfPlayerInput : MonoBehaviour {
    public GameRythm GameMaster;
    public HudBehavior Hud;
    public PlayerNoteManage[] PlayerNotes;
    void Start() {
        Hud = GameMaster.Hud;
        GameMaster = this.gameObject.GetComponent<GameRythm>();
        foreach (PlayerNoteManage note in PlayerNotes) {
            note.Game = GameMaster;
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { 
            Hud.FlashNote(3); var notes = PlayerNotes[3].CurIteractNote;
            if (notes.Count > 0) {
                for (int i = notes.Count - 1; i >= 0; i--) { Destroy(notes[i]); }
                notes.Clear();
                GameMaster.Characters[0].PlayAnimation("Right");
                if (GameMaster.MicroGame == MicroGames.Random) { GameMaster.PlayerVoiceSpeaker.SingNote(3); }
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { 
            Hud.FlashNote(2); var notes = PlayerNotes[2].CurIteractNote;
            if (notes.Count > 0) {
                for (int i = notes.Count - 1; i >= 0; i--) { Destroy(notes[i]); }
                notes.Clear();
                GameMaster.Characters[0].PlayAnimation("Up");
                if (GameMaster.MicroGame == MicroGames.Random) { GameMaster.PlayerVoiceSpeaker.SingNote(2); }
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { 
            Hud.FlashNote(1); var notes = PlayerNotes[1].CurIteractNote;
            if (notes.Count > 0) {
                    for (int i = notes.Count - 1; i >= 0; i--) { Destroy(notes[i]); }
                notes.Clear();
                GameMaster.Characters[0].PlayAnimation("Down");
                if (GameMaster.MicroGame == MicroGames.Random) { GameMaster.PlayerVoiceSpeaker.SingNote(1); }
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { 
            Hud.FlashNote(0); var notes = PlayerNotes[0].CurIteractNote;
            if (notes.Count > 0) {
                for (int i = notes.Count - 1; i >= 0; i--) { Destroy(notes[i]); }
                notes.Clear();
                GameMaster.Characters[0].PlayAnimation("Left");
                if (GameMaster.MicroGame == MicroGames.Random) { GameMaster.PlayerVoiceSpeaker.SingNote(0); }
            }
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) { Hud.UnflashNote(3); }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) { Hud.UnflashNote(2); }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) { Hud.UnflashNote(1); }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) { Hud.UnflashNote(0); }
    }
}
