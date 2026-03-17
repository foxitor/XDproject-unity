using System.Collections; using System.Collections.Generic; using UnityEngine;

public class PlayerNoteManage : MonoBehaviour {
    public List<GameObject> CurIteractNote = new List<GameObject>();
    public GameRythm Game;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Item")) {
            if (!CurIteractNote.Contains(other.gameObject)) {
                CurIteractNote.Add(other.gameObject);
               // Debug.Log("Note added. Total: " + CurIteractNote.Count);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Item")) {
            if (CurIteractNote.Contains(other.gameObject)) {
                Game.Characters[0].PlayAnimation("Miss" + other.GetComponent<InteractiveNote>().ShareAnimation());
                Destroy(other.gameObject); Game.PlayerVoiceSpeaker.PlayMiss();
                CurIteractNote.Remove(other.gameObject);
                //Debug.Log("Note removed. Total: " + CurIteractNote.Count);
            }
        }
    }

    public void RemoveNote(GameObject note) {
        if (CurIteractNote.Contains(note)) {
            CurIteractNote.Remove(note);
            Destroy(note);
        }
    }
}
