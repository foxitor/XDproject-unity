using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class HudBehavior : MonoBehaviour {
    //0-3 plr, 3-7 opp, 8-12 - addon;
    public GameObject[] Notes, NotesParents;
    //Takes 4 notes.
    public GameObject[] InteractiveNotes;
    public GameRythm MainGame;
    public Text SongName;

    void Start() { }
    void Update() {
        ;
    }
    public void SpawnNote(int NoteId) {
        GameObject newNote = Instantiate(
            InteractiveNotes[NoteId - (NoteId > 3 ? 4 : 0)], 
            new Vector3(Notes[NoteId].transform.position.x, Notes[NoteId].transform.position.y - 30, Notes[NoteId].transform.position.z), 
            Notes[NoteId].transform.rotation, 
            this.transform
        );
        newNote.GetComponent<InteractiveNote>().Hud = this;
        newNote.GetComponent<InteractiveNote>().NoteId = NoteId;
        newNote.GetComponent<InteractiveNote>().Game = MainGame;
    }
    public void FlashNote(int NoteId) {
        Notes[NoteId].transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
    }
    public void UnflashNote(int NoteId) {
        Notes[NoteId].transform.localScale = new Vector3(10, 10, 10);
    }
    public void SetSongName(string Name) {
        SongName.text = Name;
    }
}
