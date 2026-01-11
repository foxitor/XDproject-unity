using UnityEngine; using System.Collections; using UnityEngine.UI; using UnityEngine.SceneManagement; using UnityEngine.Video;

public class EgoricalGame : MonoBehaviour {
    //#Difficulty
    public float DifficultyPowerMultiplier, StageTime, SnakeSpawnTime;
    public int Stages, MusicState;
    string Tajkost;
    public int Kills, KillQuotaGoal;
    //#Season
    [Space]
    public EgoricalSeasonManage SeasonManage;
    //#Game
    [Space]
    public Zmeestradales Player; public EgoricalCamp Camp;
    public AudioSource Music;
    public AudioSource SnakeSounds;
    public Text StatText, UpgradePopUpText;
    public GameObject startGuide, ReadyToLeave;
    float CurTime, PopupTextAlpha;
    public EgoricalGameAttributes Attributaje;
    public AudioClip[] Musics;
    bool ended = false, downgradesComplited = false;
    //#SnakesBehavior
    [Space] 
    public bool SnakesOn;
    public GameObject SnakeInstateiate;
    public Transform[] SnakeSpawns;
    float CurSnakeTime;
    //#Controlls
    JungleFightControlls Controls;

    //GamePad
    void Awake() { 
        Controls = new JungleFightControlls();
        Controls.Gameplay.Simbit.performed += Ctx => SimbitGamePad();
    }
    void OnEnable() { Controls.Gameplay.Enable();} 
    void OnDisable() { Controls.Gameplay.Disable(); }
    //-

    void SimbitGamePad() {
        if (Kills >= KillQuotaGoal && !ended) { 
            Attributaje.EndSession(); ended = true;
            Player.Move = false; SnakesOn = false;
        } else if (Kills >= KillQuotaGoal && ended) {
            ReloadScene();
        } 
    }

    void Start() {
        StartCoroutine(UpgradeStage());
        startGuide.SetActive(true);
        StartCoroutine(HideGuide());
        SeasonManage.DefineSeason();
        if ((Musics.Length - 1) >= MusicState) { Music.Stop(); Music.clip = Musics[MusicState]; Music.Play(); }
    }

    public void SetupConfiguredDifficulty(float Power, int LapQuota, float LapLength, int KillQuota, string DisplayText, float SnakeRareness, string Feature) {
        DifficultyPowerMultiplier = Power; Stages = LapQuota; StageTime = LapLength; Tajkost = DisplayText;
        SnakeSpawnTime = SnakeRareness; KillQuotaGoal = KillQuota;
        //switch (Feature) {
        //    case "Storm" : AcсessStorm = true; CurrentEvent = "Storm"; break;
        //    default: break;
        //}
        //Debug.Log(Tajkost);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReloadScene();
        }
        
        CurTime += Time.deltaTime;
        float CurTimeLeftOver = (CurTime - StageTime) * -1;
        StatText.text = (" " + (!downgradesComplited ? CurTimeLeftOver.ToString("F1") + " сек. до Ухудшения; " : "") + "Потужность : " + Tajkost + 
        "\n Трезвости : " + Player.ShareHealthValue() + "/3 | " + Camp.CurBaseDurability +
        "/3\n Фраги : " + Kills + "/" + KillQuotaGoal);

        UpgradePopUpText.color = new Color (UpgradePopUpText.color.r, UpgradePopUpText.color.g, UpgradePopUpText.color.b, PopupTextAlpha);
        if (PopupTextAlpha > 0) {
            PopupTextAlpha = PopupTextAlpha - 0.5f * Time.deltaTime;
        }   

        CurSnakeTime += Time.deltaTime;
        if (CurSnakeTime > SnakeSpawnTime) { CurSnakeTime = 0; SpawnSnakes(); }

        if (Kills >= KillQuotaGoal) {
            ReadyToLeave.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Return) && Kills >= KillQuotaGoal && !ended) {
            Attributaje.EndSession(); ended = true;
            Player.Move = false; SnakesOn = false;
        } else if (Input.GetKeyDown(KeyCode.Return) && Kills >= KillQuotaGoal && ended) {
            ReloadScene();
        }
    }
    void SpawnSnakes() { 
        int index = Random.Range(0, SnakeSpawns.Length);
        Instantiate(SnakeInstateiate, SnakeSpawns[index].transform.position, Quaternion.identity);
    }
    IEnumerator UpgradeStage() { 
        yield return new WaitForSeconds(StageTime);
        if (MusicState < Stages) {
            CurTime = 0; MusicState++; StartCoroutine(UpgradeStage()); 
            if ((Musics.Length - 1) >= MusicState) { Music.Stop(); Music.clip = Musics[MusicState]; Music.Play(); }
            UpgradePopUpText.text = "Ухудщение " + MusicState + "/" + Stages;
            PopupTextAlpha = 1f;
            //Debug.Log("Upgrade! = " + MusicState);
        } else {
            downgradesComplited = true;
        }
    }
    IEnumerator HideGuide() {
        yield return new WaitForSeconds(3); startGuide.SetActive(false);
    }
    public void playSnakeSound(AudioClip Sound) {
        SnakeSounds.PlayOneShot(Sound);
    }
    public void ReloadScene() {
        Player.StopVibration();
        SceneManager.LoadScene("NewJungleFight");
    }
}
