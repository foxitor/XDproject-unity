using UnityEngine; using System.Collections; using UnityEngine.UI; using UnityEngine.SceneManagement; using UnityEngine.Video;

public class EgorGameManage : MonoBehaviour {
    public Text timeText, EndingText; GameObject Player, Camp; public int Kills;
    public int MusicState, UpgradeTime; public AudioClip[] Musics; public AudioClip CrazySigimaS; public int[] bpmSpeeds;
    public Camera Cam, MinimapCam; public float bpm = 180f; int lastBeat = -1; double startTime;
    private bool isBupActive = false, ended; public float bupScale = 1.1f, DifPower; public float bupDuration = 0.1f;
    private float bupTimer = 0f, CurTime; string Difficulty, Potugjnost;
    public GameObject[] SnakeSpawns; public GameObject Snake, startGuide, RankScreen, minimapGrass;
    public string[] Endings; public bool SnakesOn = true; public AudioSource EndingSource;
    public GameObject[] WinterFeatures;
    public VideoClip[] RankVideoClips; public VideoPlayer RankingVidPlayer; public GameObject[] Boards;
    void Start() {
        Camp = GameObject.Find("Fight/Camp"); Player = GameObject.Find("Fight/Player");
        startTime = AudioSettings.dspTime; StartCoroutine(UpgradeStage());
        bpm = bpmSpeeds[MusicState]; StartCoroutine(HideGuide());
        this.gameObject.GetComponent<AudioSource>().clip = Musics[MusicState];
        this.gameObject.GetComponent<AudioSource>().Play();
        if (Difficulty == "Hard") { DifPower = 1.5f; UpgradeTime = 30; Potugjnost = "Змиздец"; }
        if (Difficulty == "Norm") { DifPower = 1f; UpgradeTime = 10; Potugjnost = "Нормусь"; }
        if (Difficulty == "Easy") { DifPower = 0.8f; UpgradeTime = 5; Potugjnost = "Размокщая печенька"; }
        if (IsWinter()) {
            Cam.backgroundColor = new Color(0.62f, 0.85f, 0.92f, 1f); MinimapCam.backgroundColor = new Color(0.62f, 0.85f, 0.92f, 1f);
            minimapGrass.GetComponent<Image>().color = new Color(0.62f, 0.85f, 0.92f, 1f);
            foreach(GameObject board in Boards) { board.GetComponent<SpriteRenderer>().color = new Color(0.42f, 0.65f, 0.72f, 1f); }
        } foreach(GameObject feature in WinterFeatures) { feature.SetActive(IsWinter()); }
    }
    void Update() { 
        if (Input.GetKey("escape")) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        Cam.transform.position = new Vector3(Player.transform.position.x, 
            Player.transform.position.y, Cam.transform.position.z);
        CurTime += Time.deltaTime;
        float CurTimeLeftOver = (CurTime - UpgradeTime) * -1;
        int PlayerHps = Player.GetComponent<PlayerProtection>().CurHP;
        int CampHurts = Camp.GetComponent<SniffersBase>().Hurten;
        timeText.text = CurTimeLeftOver.ToString("F0") + "сек. до пездеца; " + Kills + " фрагов; " 
            + MusicState  + "/6 Стадия критинизма\n" + "Вы трезвы на " + PlayerHps * 10 +"%;" + 
            "процент змей на базе " + CampHurts * 10 +"%\nПотужность: " + Potugjnost;
        double elapsed = AudioSettings.dspTime - startTime;
        double secondsPerBeat = 60.0 / bpm; int currentBeat = (int)(elapsed / secondsPerBeat);
        float bopFreq; if (bpm == 200 ) { bopFreq = 1; } else { bopFreq = 2; }
        if (currentBeat != lastBeat) { 
            lastBeat = currentBeat; if (currentBeat % bopFreq == 0) { StartBup(); } 
            if (currentBeat % (MusicState + 2) == 0) { SpawnSnakes(); } 
        }
        if (isBupActive) {bupTimer += Time.deltaTime; float halfDuration = bupDuration / 2f;
            if (bupTimer < halfDuration) { Cam.fieldOfView = bupScale;
            } else if (bupTimer < bupDuration) {
                float t = (bupTimer - halfDuration) / halfDuration; float scale = Mathf.Lerp(bupScale, 50f, t); 
                Cam.fieldOfView = scale;
            } else { Cam.fieldOfView = 50f; isBupActive = false; }
        }
        if (Input.GetKeyDown(KeyCode.Return) && ended) {
                SceneManager.LoadScene("JungleFight");
            }
    }
    bool IsWinter() {
        if (PlayerPrefs.HasKey("LastSeason")) {
            int CurSeason = PlayerPrefs.GetInt("LastSeason");bool winterBool = false;
            if (CurSeason == 1) { winterBool = true; } return winterBool;
        } else { return false; }
    }
    void StartBup() { isBupActive = true; bupTimer = 0f; }
    void SpawnSnakes() { 
        int index = Random.Range(0, SnakeSpawns.Length);
        Instantiate(Snake, SnakeSpawns[index].transform.position, Quaternion.identity);
    }
    IEnumerator UpgradeStage() { 
        yield return new WaitForSeconds(UpgradeTime);
        if (MusicState < 5) {
            CurTime = 0; bpm = bpmSpeeds[MusicState]; MusicState++; StartCoroutine(UpgradeStage());
            this.gameObject.GetComponent<AudioSource>().clip = Musics[MusicState];
            this.gameObject.GetComponent<AudioSource>().Play();
        } else {
            RankScreen.SetActive(true); Player.GetComponent<FollowMouse>().Move = false; SnakesOn = false;
            VideoClip RankVideoClip;
            if (Player.GetComponent<SpriteRenderer>().sprite.name == "SnakeKiller") { RankVideoClip = RankVideoClips[0]; } else { RankVideoClip = RankVideoClips[1]; }
            RankingVidPlayer.clip = RankVideoClip; ended = true;
            int ending = Random.Range(0, Endings.Length);
            string EndingUnlockedText = ""; string NoDamAdv = ""; string DifficultAdv = "";
            bool EndingIsUnlocked = false; bool NoDamageAdvUnlocked = false; bool DifficultAdvUnlocked = false;
            string SeasonAdv = ""; bool SeasonAdvUnlocked = false;
            switch(ending) {
                case 0 : if (PlayerPrefs.GetInt("NastySnakes") == 0 || !PlayerPrefs.HasKey("NastySnakes")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("NastySnakes", 1); break;
                case 1 : if (PlayerPrefs.GetInt("FuckUrSnakes") == 0 || !PlayerPrefs.HasKey("FuckUrSnakes")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("FuckUrSnakes", 1); break;
                case 2 : if (PlayerPrefs.GetInt("CrustyDog") == 0 || !PlayerPrefs.HasKey("CrustyDog")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("CrustyDog", 1); break;
                case 3 : if (PlayerPrefs.GetInt("CrazySigima") == 0 || !PlayerPrefs.HasKey("CrazySigima")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("CrazySigima", 1); EndingSource.PlayOneShot(CrazySigimaS); break;
                case 4 : if (PlayerPrefs.GetInt("NewGenSnake") == 0 || !PlayerPrefs.HasKey("NewGenSnake")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("NewGenSnake", 1); break;
                case 5 : if (PlayerPrefs.GetInt("BloodySaw") == 0 || !PlayerPrefs.HasKey("BloodySaw")) 
                { EndingIsUnlocked = true; } PlayerPrefs.SetInt("BloodySaw", 1); break;
            }
            if (Difficulty == "Easy") { if (PlayerPrefs.GetInt("SnakeKiller-Newborn") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Newborn")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Newborn", 1); }
            if (Difficulty == "Norm") { if (PlayerPrefs.GetInt("SnakeKiller-Standart") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Standart")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Standart", 1); }
            if (Difficulty == "Hard") { if (PlayerPrefs.GetInt("SnakeKiller-Expert") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Expert")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Expert", 1); }
            if (IsWinter()) { if (PlayerPrefs.GetInt("ColdFights") == 0 || !PlayerPrefs.HasKey("ColdFights")) 
                { SeasonAdvUnlocked = true; } PlayerPrefs.SetInt("ColdFights", 1); }
            if (Player.GetComponent<PlayerProtection>().CurHP == 10 && Camp.GetComponent<SniffersBase>().Hurten == 0) {
                if (PlayerPrefs.GetInt("NoDamageSnake") == 0 || !PlayerPrefs.HasKey("NoDamageSnake")) 
                { NoDamageAdvUnlocked = true; } PlayerPrefs.SetInt("NoDamageSnake", 1);
            }
            this.gameObject.GetComponent<AudioSource>().Stop();
            if (EndingIsUnlocked == true) { EndingUnlockedText = "+Достижение за концовку"; }
            if (DifficultAdvUnlocked == true) { DifficultAdv = "+Достижение за потужность"; }
            if (NoDamageAdvUnlocked == true) { NoDamAdv = "+Достижение за прохождение без урона"; }
            if (SeasonAdvUnlocked == true) { SeasonAdv = "+Достижение за сезон"; }
                EndingText.text = "Концовка : \n" + Endings[ending] + "\n" + EndingUnlockedText + "\n" + DifficultAdv + "\n" + NoDamAdv + "\n" + SeasonAdv;
        }
    }
    IEnumerator HideGuide() {
        yield return new WaitForSeconds(3); startGuide.SetActive(false);
    }
    IEnumerator Ending() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("JungleFight");
    }
    public void SetDifficulty(string Dif) { Difficulty = Dif; }
}
