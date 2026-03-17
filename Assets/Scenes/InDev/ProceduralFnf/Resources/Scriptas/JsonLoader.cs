using UnityEngine; using System.IO; using System.Collections.Generic;

[System.Serializable]
public class NoteData {
    public bool gfSection;
    public bool altAnim;
    public List<List<int>> sectionNotes;
    public int bpm;
    public int sectionBeats;
    public bool changeBPM;
    public bool mustHitSection;
}

[System.Serializable]
public class SongData {
    public float speed;
    public string stage;
    public string player1;
    public string player2;
    public List<object> events;
    public List<NoteData> notes;
    public string gfVersion;
    public string format;
    public int bpm;
    public bool needsVoices;
    public string song;
    public int offset;
}
public class JsonLoader : MonoBehaviour {
    public GameRythm GameMaster;
    public string jsonFilePath = "Assets/YourFolder/yourfile.json";
    Dictionary<float, bool> notesSpawnedFlags = new Dictionary<float, bool>();
    List<float> noteTimes = new List<float>();
    int globalTotalStep;

    NoteData currentNoteData;

    void Start() {
        GameMaster = this.gameObject.GetComponent<GameRythm>(); LoadJson();
    }

    void Update() { 
        //getTotalStep(); 
    }

    void LoadJson() {
        if (File.Exists(jsonFilePath)) {
            string jsonString = File.ReadAllText(jsonFilePath);
            SongData data = JsonUtility.FromJson<SongData>(jsonString);

            Debug.Log("Song: " + data.song);
            GameMaster.SongName = data.song;
            Debug.Log("Speed: " + data.speed);
            GameMaster.ScrollSpeed = data.speed;
            Debug.Log("BPM: " + data.bpm);
            GameMaster.bpm = data.bpm;

            if (data.notes != null && data.notes.Count > 0) {
                Debug.Log($"Notes found: {data.notes.Count}");
                currentNoteData = data.notes[0];
                foreach (var note in data.notes) {
                    //if (note == null) { Debug.Log("Note is null"); continue; }
                    //if (note.sectionNotes != null && note.sectionNotes.Count > 0) {
                        foreach (var sectionNote in note.sectionNotes) {
                            float noteStep = sectionNote[0] / 150;
                            //if (!notesSpawnedFlags.ContainsKey(noteStep)) {
                                //if (globalTotalStep == noteStep) {
                                    GameMaster.Hud.SpawnNote(sectionNote[1]);
                                //}
                            //}
                        }
                    //} else { Debug.Log("No notes for this note element"); }
                }
            } else { Debug.Log("Nothing to press."); }
        } else { Debug.LogError("File not found at " + jsonFilePath); }
    }
    public int getTotalStep(int LoadedStep) {
        globalTotalStep = LoadedStep;
        //Debug.Log(LoadedStep);
        return LoadedStep;
    }
}