using UnityEngine; using System.Collections; using UnityEngine.UI; using UnityEngine.SceneManagement; using UnityEngine.Video;

public class GamManag : MonoBehaviour {
    public Baebrickxxxx Chertolet;

    public string CurrentEvent;
    public float CurTime, LoopTime; 
    public Text timeText; 
    public bool DoPPspawn, p1, p2;
    public GameObject CumImg, PituxImg, Pitux, Storm, Pitux1a, Pitux2a, Egg, PituxsHat;
    public VideoPlayer RankingVidPlayer;
    public Transform attck11spawn, attck12spawn, attck21spawn;
    AudioSource source; 
    public AudioClip[] AudioLib;
    public int Laps, MaxLaps;
    bool ShowingAttack = false, ended = false, endOneShot = false, AcсessStorm = false;
    string Tajkost; public float DifficultyPowerMultiplier;
    public GameObject Music;
    public ChertGameAttributes Attributaje; public ChertSeasonalFeatures SeasonManage;
    ChertoletControls Controls;

    //GamePad
    void Awake() { 
        Controls = new ChertoletControls();
        Controls.GamePlay.Simbit.performed += Ctx => Simbit();
    }
    void OnEnable() { Controls.GamePlay.Enable();} 
    void OnDisable() { Controls.GamePlay.Disable(); }
    //-


    public void SetupConfiguredDifficulty(float Power, int LapQuota, float LapLength, string DisplayText, string Feature) {
        DifficultyPowerMultiplier = Power; MaxLaps = LapQuota; LoopTime = LapLength; Tajkost = DisplayText;
        switch (Feature) {
            case "Storm" : AcсessStorm = true; CurrentEvent = "Storm"; break;
            default: break;
        }
    }
    void Start() {
        source = this.gameObject.GetComponent<AudioSource>();
        timeText.gameObject.SetActive(true); Music.SetActive(true);
        SeasonManage.DefineSeason();
    }
    void Update() {
        if (!ended) {
            CurTime += Time.deltaTime;
            if (Laps >= MaxLaps && !endOneShot) {
                Attributaje.EndSession(); ended = true; endOneShot = true;
                Chertolet.Move = false;
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Return)) { ReloadScene(); }
        } if (Input.GetKeyDown(KeyCode.Escape)) { ReloadScene(); }
        timeText.text = CurTime.ToString("F1") + " сек. чертолётсвтва; " + ((Laps - MaxLaps) * -1) + "км До точки тулла;" +
        " ПОТУЖНОСТЬ : " + Tajkost;
        if (CurTime > LoopTime && CurTime < LoopTime + 1) {
            Chertolet.VibrateController(0.25f, 0.25f, 0.1f);
            int randomings = Random.Range(0,5); CurTime = 0; 
            bool onetab = false; Laps++;

            if (randomings == 4 && Tajkost == "Тяжко." && !onetab) { CurrentEvent = "Storm"; 
            onetab = true; Storm.SetActive(true); }

            if (randomings == 2 && !onetab) { CurrentEvent = "Cum"; source.PlayOneShot(AudioLib[0]);
            CumImg.SetActive(true); StartCoroutine(HideCum()); onetab = true; } 

            if (randomings == 1 && !onetab) { CurrentEvent = "Pitux"; source.PlayOneShot(AudioLib[1]);
            PituxImg.SetActive(true); StartCoroutine(HidePitux()); onetab = true; }

            if (randomings == 3 && !onetab) { CurrentEvent = ""; 
            onetab = true; }
        }
        if (CurrentEvent == "Pitux" && Tajkost != "Тяжко.") { DoPPspawn = false; } else { DoPPspawn = true; }
        HandleEvent();
    }
    public void ReloadScene() {
        Chertolet.StopVibration();
        SceneManager.LoadScene("Chertolet");
    }
    void Simbit() {
        if (ended) ReloadScene();
    }
    IEnumerator HidePitux() { yield return new WaitForSeconds(3); PituxImg.SetActive(false); PituxsHat.SetActive(false); }
    IEnumerator HideCum() { yield return new WaitForSeconds(3); CumImg.SetActive(false); }
    void HandleEvent() {
        if (AcсessStorm) { if (CurrentEvent == "Storm") { Storm.SetActive(true); } else { Storm.SetActive(false); } }
        if (CurrentEvent == "Pitux") {
            Pitux.SetActive(true);
            int randomAttack = Random.Range(0,3);
            if (!ShowingAttack) {
                if (randomAttack == 1) {
                    Pitux1a.SetActive(true); ShowingAttack = true; p1 = true; p2 = false;
                    StartCoroutine(HideAttack1());
                    StartCoroutine(StrikeEggs(1f)); StartCoroutine(StrikeEggs(1.5f)); StartCoroutine(StrikeEggs(2f));
                }
                if (randomAttack == 2) {
                    Pitux2a.SetActive(true); ShowingAttack = true; p2 = true; p1 = false;
                    StartCoroutine(HideAttack2());
                    StartCoroutine(StrikeEggs(1f)); StartCoroutine(StrikeEggs(1.5f)); StartCoroutine(StrikeEggs(2f));
                }
            } if (SeasonManage.Season == Seasons.Winter) { PituxsHat.SetActive(true); }
        }
        else {
            Pitux.SetActive(false);
            PituxsHat.SetActive(false);
            ShowingAttack = false;
        }
    }
    IEnumerator HideAttack1() { yield return new WaitForSeconds(3); Pitux1a.SetActive(false); ShowingAttack = false; }
    IEnumerator HideAttack2() { yield return new WaitForSeconds(3); Pitux2a.SetActive(false); ShowingAttack = false; }
    IEnumerator StrikeEggs(float waitTime) { yield return new WaitForSeconds(waitTime); if (p1) {
        Instantiate(Egg, attck11spawn.position, Quaternion.identity); 
        Instantiate(Egg, attck12spawn.position, Quaternion.identity);
    } if (p2) { Instantiate(Egg, attck21spawn.position, Quaternion.identity); } }
    bool TrimFloats(float f1, float f2) {
        return ((f1 > f2) && (f1 < f2 + 0.1f)) ? true : false;
    }
}
