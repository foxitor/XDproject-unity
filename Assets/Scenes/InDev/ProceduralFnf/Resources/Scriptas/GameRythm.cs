using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public enum MicroGames {
    Random,
    Song
}
public class GameRythm : MonoBehaviour {
    public MicroGames MicroGame;
    //---
    public float bpm = 120f, 
    ScrollSpeed = 2.0f;

    public HudBehavior Hud; 
    public string SongName;
    [Space]
    public JsonLoader JsonInteraction;
    [Space]
    public VoiceContainerData PlayerVoiceSpeaker, OpponentVoiceSpeaker;
    public InstrumentalData InstrumentalStorage;

    public bool DoVoices;

    [Space]
    public CharacterVizual[] Characters;
    float beatInterval, stepInterval, sectionInterval;
    
    float timer = 0f;
    int currentStep = 0, TotalStep = 0;
    Dictionary<float, bool> notesSpawnedFlags = new Dictionary<float, bool>(); 

    Camera mainCamera;
    Color defaultColor;
    bool flashTrigger = false, MustHitSection;
    float flashDuration = 0.2f, flashTimer = 0f;
    bool isFlashing = false;
    Color flashColor = Color.white;
    public string CurPatternType;
    int beatCount = 0, curSection = 0, SectionCounter = 0;
    List<NoteData> notesData;
    
    void Start() {
        beatInterval = 60f / bpm; stepInterval = beatInterval / 4f;  sectionInterval = beatInterval * 4f;
        mainCamera = Camera.main; defaultColor = mainCamera.backgroundColor;
        Hud.MainGame = this;
    }

    void Update() {
        Hud.SetSongName(SongName);
        timer += Time.deltaTime;

        if (timer >= stepInterval) { timer -= stepInterval; AdvanceStep(); ShareTotalStep(); }
        if (currentStep == 4) { currentStep = 0; OnBeat(); }
        if (beatCount == 4) { beatCount = 0; HitSection(); }
        if (isFlashing) {
            flashTimer += Time.deltaTime; float t = flashTimer / flashDuration;
            mainCamera.backgroundColor = Color.Lerp(flashColor, defaultColor, t);
            if (flashTimer >= flashDuration) { mainCamera.backgroundColor = defaultColor; isFlashing = false; }
        }
    }

    void AdvanceStep() {
        currentStep++; TotalStep++;
        //
        if (MicroGame == MicroGames.Random) { ManagePattern(); ManageInstrumental(); }
    }

    void OnBeat() {
        beatCount++;
        //Debug.Log("Beat count: " + beatCount);
        if (Characters[0].isSinging == false) { Characters[0].PlayAnimation("Idle"); } 
        if (Characters[1].isSinging == false) { Characters[1].PlayAnimation("Idle"); }
        if (beatCount % 2 == 0) { TriggerFlash(); }
    }
    void HitSection() { 
        curSection++; SectionCounter++; if (SectionCounter > 3) { SectionCounter = 0; }

        //
        if (MicroGame == MicroGames.Random) {
            if (SectionCounter == 0) {
                int Pattern = Random.Range(0, 4);
                switch (Pattern) {
                    case 0 : CurPatternType = "Curtain"; break;
                    case 1 : CurPatternType = "Stair"; break;
                    case 2 : CurPatternType = "Random"; break;
                    case 3 : CurPatternType = "Sticks"; break;
                }
            }
        }
    }

    void TriggerFlash() { mainCamera.backgroundColor = flashColor; isFlashing = true; flashTimer = 0f; }
    public class Section { public List<Beat> beats = new List<Beat>(); } public class Beat { public List<Step> steps = new List<Step>(); }
    public class Step {  }
    //
    public void ShareTotalStep() {
        JsonInteraction.getTotalStep(TotalStep);
    }

    //
    public void ManagePattern() {
        if (SectionCounter % 2 == 0) { MustHitSection = false; } else { MustHitSection = true; }

        if (CurPatternType == "Curtain") {
            if (beatCount % 2 == 0) {
                if (currentStep == 1) {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
                if (currentStep == 2) {
                    Hud.SpawnNote(MustHitSection ? 0 : 3 + 4);
                }
                if (currentStep == 3) {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
                if (currentStep == 4) {
                    Hud.SpawnNote(MustHitSection ? 1 : 2 + 4);
                }
            } else {
                if (currentStep == 1) {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
                if (currentStep == 2) {
                    Hud.SpawnNote(MustHitSection ? 2 : 1 + 4);
                }
            }
        }
        if (CurPatternType == "Stair") {
            if (beatCount % 2 == 0) {
                if (currentStep == 1) {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
                if (currentStep == 2) {
                    Hud.SpawnNote(MustHitSection ? 2 : 1 + 4);
                }
                if (currentStep == 3) {
                    Hud.SpawnNote(MustHitSection ? 1 : 2 + 4);
                }
                if (currentStep == 4) {
                    Hud.SpawnNote(MustHitSection ? 0 : 3 + 4);
                }
            } else {
                if (currentStep == 1) {
                    Hud.SpawnNote(MustHitSection ? 1 : 2 + 4);
                }
                if (currentStep == 2) {
                    Hud.SpawnNote(MustHitSection ? 2 : 1 + 4);
                }
                if (currentStep == 3) {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
                if (currentStep == 4) {
                    Hud.SpawnNote(MustHitSection ? 2 : 1 + 4);
                }
            }
        }
        if (CurPatternType == "Random") {
            if (MustHitSection) {
                if (Random.Range(0,4) > 0) 
                Hud.SpawnNote(Random.Range(0,4));
            } else {
                if (Random.Range(0,4) > 0) 
                Hud.SpawnNote(Random.Range(4,8));
            }
        }
        if (CurPatternType == "Sticks") {
            if (beatCount % 2 == 0) {
                if (currentStep % 2 == 0) {
                    Hud.SpawnNote(MustHitSection ? 0 : 3 + 4);
                } else {
                    Hud.SpawnNote(MustHitSection ? 2 : 1 + 4);
                }
            } else {
                if (currentStep % 2 == 0) {
                    Hud.SpawnNote(MustHitSection ? 1 : 2 + 4);
                } else {
                    Hud.SpawnNote(MustHitSection ? 3 : 0 + 4);
                }
            }
        }
    }
    void ManageInstrumental() {
        if (InstrumentalStorage.Type == "BeatBox") {
            if (beatCount % 2 == 0) {
                if (currentStep == 0) {
                    InstrumentalStorage.Play(0);
                    InstrumentalStorage.Play(3);
                }
                if (currentStep == 1) {
                    InstrumentalStorage.Play(2);
                }
                if (currentStep == 2) {
                    InstrumentalStorage.Play(1);
                    InstrumentalStorage.Play(3);
                }
                if (currentStep == 3) {

                }
            } else {
                if (currentStep == 0) {
                    InstrumentalStorage.Play(2);
                    InstrumentalStorage.Play(3);
                }
                if (currentStep == 1) {
                    InstrumentalStorage.Play(0);
                    InstrumentalStorage.Play(2);
                }
                if (currentStep == 2) {
                    InstrumentalStorage.Play(3);
                }
                if (currentStep == 3) {
                    InstrumentalStorage.Play(1);
                }
            }
        }
        if (InstrumentalStorage.Type == "Sexy") {
            if (currentStep % 2 == 0) {
                InstrumentalStorage.Play(2);
            }
            switch (beatCount) {
                case 0:
                    if (currentStep == 2) {
                        InstrumentalStorage.Play(0);
                    }
                    if (currentStep == 4) {
                        InstrumentalStorage.Play(0);
                    }
                break;
                case 1:
                    if (currentStep == 1) {
                        InstrumentalStorage.Play(1);
                    }
                    if (currentStep == 3) {
                        InstrumentalStorage.Play(0);
                    }
                    if (currentStep == 4) {
                        InstrumentalStorage.Play(1);
                    }
                break;
                case 2:
                    if (currentStep == 2) {
                        InstrumentalStorage.Play(0);
                    }
                    if (currentStep == 4) {
                        InstrumentalStorage.Play(0);
                    } 
                break;
                case 3:
                    if (currentStep == 1) {
                        InstrumentalStorage.Play(1);
                    }
                    if (currentStep == 3) {
                        InstrumentalStorage.Play(0);
                    }
                    if (currentStep == 4) {
                        InstrumentalStorage.Play(1);
                    }
                break;
                case 4 : Debug.Log("IDI NAXUI"); break;
            } 
        }
        if (InstrumentalStorage.Type == "Test") {
            if (beatCount % 2 == 0) {
                if (currentStep == 0) {
                    InstrumentalStorage.Play(0);
                }
                if (currentStep == 2) {
                    InstrumentalStorage.Play(1);
                }
                
            } else {
                if (currentStep == 0) {
                    InstrumentalStorage.Play(0);
                }
                if (currentStep == 2) {
                    InstrumentalStorage.Play(1);
                }
            }
        } 
    }
}
