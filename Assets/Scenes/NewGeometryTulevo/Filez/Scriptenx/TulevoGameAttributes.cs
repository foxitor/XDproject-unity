using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using UnityEngine.Video; using UnityEngine.SceneManagement;

public class TulevoGameAttributes : MonoBehaviour {
    //#Rank
    public VideoClip[] RankVideoClips; 
    public VideoPlayer RankingVidPlayer;
    public GameObject RankScreen, RawVideoPlayer, BlackScreen;
    public AudioClip[] RankLabelSounds;
    [Space]

    //#WearableSetting
    public Transform WearableStorage;
    [Space]

    //#DifficultySetting
    string DifficultyDisplay = "", DifficultyFeature = "";
    Difficulties Difficulty;
    [Space]

    //#Controlls
    ChertoletControls Controls;

    //#Ending
    public Text EndingText; 
    int PossibleEndings = 6;
    [Space]
    //#Other References
    public TulevoGame Game;
    bool inGame;

    //GamePad
    void Awake() { 
        Controls = new ChertoletControls();
        Controls.GamePlay.Exit.performed += Ctx => Menu();
    }
    void OnEnable() { Controls.GamePlay.Enable();} 
    void OnDisable() { Controls.GamePlay.Disable(); }
    //-
    void Menu() {
        if (!inGame) {
            SceneManager.LoadScene("Menu");
        } else { SceneManager.LoadScene("Chertolet"); }
    }

    public void InsertWearable() {
        int totalWearables = WearableStorage.childCount; int usedWearableIndex = -1;
        for (int i = 0; i < totalWearables; i++) {
            WearableStorage.GetChild(i).GetComponent<TullWearableModifier>().StartsGame();
        } 
    }   

    public void IsertDifficulty(int DifficultyIndex) {
        switch (DifficultyIndex) {
            case 0 : Difficulty = Difficulties.Easy; DifficultyDisplay = "Плачущая тапка"; break;
            case 1 : Difficulty = Difficulties.Normal; DifficultyDisplay = "Адекват"; break;
            case 2 : Difficulty = Difficulties.Hard; DifficultyDisplay = "Проклятье альфредо."; break;
            default : Difficulty = Difficulties.Unassigned; DifficultyDisplay = "Unassigned Difficulty Error..."; break;
        }
        ConfigureDifficulty();
    }
    void ConfigureDifficulty() {
        float ConfiguredPowerMultiplier = 0.5f; int ConfiguredAmmoQuota = 0;
        switch (Difficulty) {
            case Difficulties.Easy : ConfiguredPowerMultiplier = 1f; ConfiguredAmmoQuota = 4; break;
            case Difficulties.Normal : ConfiguredPowerMultiplier = 1.3f; ConfiguredAmmoQuota = 7; break;
            case Difficulties.Hard : ConfiguredPowerMultiplier = 1.5f; ConfiguredAmmoQuota = 14; break;
            case Difficulties.Unassigned : ConfiguredPowerMultiplier = 0.5f; ConfiguredAmmoQuota = 1; break;
        }
        Game.SetupConfiguredDifficulty(ConfiguredPowerMultiplier, ConfiguredAmmoQuota, DifficultyDisplay);
        inGame = true;
    }

    public void EndSession() {
        //#Setup
        BlackScreen.SetActive(true);
        RawVideoPlayer.SetActive(false);
        RankingVidPlayer.gameObject.SetActive(false);
        RankScreen.SetActive(true); 
        Game.Music.gameObject.SetActive(false);
        VideoClip RankVideoClip;

        switch (Game.Player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite.name) {
            case "SnakeKiller" : RankVideoClip = RankVideoClips[0]; break;
            case "SnakeKillerCreepy" : RankVideoClip = RankVideoClips[1]; break;
            default: RankVideoClip = RankVideoClips[0]; break;
        }
        
        //#Appling
        RankingVidPlayer.clip = RankVideoClip;

        //#EndingScreenVaruables :
        string EndingAchivementUnlockedText = "∘", DifficultAdv = "∘", SeasonAdv = "∘", ConditionAdv = "∘";
        bool EndingIsUnlocked = false, DifficultAdvUnlocked = false, SeasonAdvUnlocked = false;
        int EndVariant = Random.Range(0, PossibleEndings);

        EndingAchivementUnlockedText = AnalizeEndingAchivement(EndVariant);
        DifficultAdv = AnalizeDifficultyAchivement();
        SeasonAdv = AnalizeSeasonAchivement();
        ConditionAdv = AnalizeConditionAchivement();

        //#RunLables
        StartCoroutine(EndingResultText(EndVariant, EndingAchivementUnlockedText, DifficultAdv, SeasonAdv, ConditionAdv));

        //#•∘ lit & unlit dot symbols.
    }
    #region Analizing
    //#Analizing
    string DefineEndVariant(int EndingIndex) {
        string ReturnResult = "Unable to Translate Index.";
        switch (EndingIndex) {
            case 0 : ReturnResult = "Алё пушкин, я дантес"; break;
            case 1 : ReturnResult = "Я затулил Скулика."; break;
            case 2 : ReturnResult = "У меня патроны закончились..."; break;
            case 3 : ReturnResult = "Кинул распальцовку"; break;
            case 4 : ReturnResult = "ЭУ МА БОЙ!!!!!!!"; break;
            case 5 : ReturnResult = "Скулик сдался!"; break;
        }
        return ReturnResult;
    }
    string AnalizeEndingAchivement(int EndingIndex) {
        string NewEndingReturn = "∘"; bool EndingIsUnlocked = false;
        switch (EndingIndex) {
            case 0 : if (PlayerPrefs.GetInt("Dantess") == 0 || !PlayerPrefs.HasKey("Dantess")) { EndingIsUnlocked = true; }
            PlayerPrefs.SetInt("Dantess", 1); break;
            case 1 : if (PlayerPrefs.GetInt("HappyEnd") == 0 || !PlayerPrefs.HasKey("HappyEnd")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("HappyEnd", 1); break;
            case 2 : if (PlayerPrefs.GetInt("NoAmmo") == 0 || !PlayerPrefs.HasKey("NoAmmo")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("NoAmmo", 1); break;
            case 3 : if (PlayerPrefs.GetInt("Palcovka") == 0 || !PlayerPrefs.HasKey("Palcovka")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("Palcovka", 1); break;
            case 4 : if (PlayerPrefs.GetInt("MyBoy") == 0 || !PlayerPrefs.HasKey("MyBoy")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("MyBoy", 1); break;
            case 5 : if (PlayerPrefs.GetInt("Weak") == 0 || !PlayerPrefs.HasKey("Weak")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("Weak", 1); break;
        }
        if (EndingIsUnlocked) { NewEndingReturn = "• Достижение за концовку!"; }
        return NewEndingReturn;
    }
    string AnalizeDifficultyAchivement() {
        string newDifficultyReturn = "∘"; bool DifficultAdvUnlocked = false;
        if (Difficulty == Difficulties.Easy) { 
            if (PlayerPrefs.GetInt("Gangster-Newborn") == 0 || !PlayerPrefs.HasKey("Gangster-Newborn")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("Gangster-Newborn", 1); 
        }
        if (Difficulty == Difficulties.Normal) { 
            if (PlayerPrefs.GetInt("Gangster-Standart") == 0 || !PlayerPrefs.HasKey("Gangster-Standart")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("Gangster-Standart", 1); 
        }
        if (Difficulty == Difficulties.Hard) { 
            if (PlayerPrefs.GetInt("Gangster-Expert") == 0 || !PlayerPrefs.HasKey("Gangster-Expert")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("Gangster-Expert", 1); 
        }
        if (DifficultAdvUnlocked) { newDifficultyReturn = "• Достижение за сложность!"; }
        return newDifficultyReturn;
    }
    string AnalizeSeasonAchivement() {
        bool SeasonAdvUnlocked = false; string newSeasonReturn = "∘";
        if (Game.SeasonManage.Season == Seasons.Winter) { 
            if (PlayerPrefs.GetInt("IceGunFight") == 0 || !PlayerPrefs.HasKey("IceGunFight")) { SeasonAdvUnlocked = true; }
            PlayerPrefs.SetInt("IceGunFight", 1); 
        }
        if (SeasonAdvUnlocked) { newSeasonReturn = "• Достижение за сезон!"; }
        return newSeasonReturn;
    }
    string AnalizeConditionAchivement() {
        bool ConditionUnlocked = false; string newConditionReturn = "∘";
        int ConditionsCount = 0;
        if (Game.CurRevolverAmmo >= 25) {
            if (PlayerPrefs.GetInt("NoOne") == 0 || !PlayerPrefs.HasKey("NoOne")) { ConditionUnlocked = true; }
            PlayerPrefs.SetInt("NoOne", 1); ConditionsCount++;
        } 
        Debug.Log("ConditionsCount is " + ConditionsCount);
        if (ConditionUnlocked) { newConditionReturn = (ConditionsCount <= 1 ? "• Достижение за условие!" : "• Достижение за условия!! (" + ConditionsCount + ")"); }
        return newConditionReturn;
    }
    #endregion

    //#Appling
    IEnumerator EndingResultText(int EndingIndex, string newEndingAchivement,
            string newDifficultyAchivement, string newSeasonAchivement, string specialConditionAdvancment) {
        AudioSource RankSounds = RankScreen.AddComponent<AudioSource>();
        RankSounds.PlayOneShot(RankLabelSounds[0]);
        Game.Player.VibrateController(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(1.5f);
        BlackScreen.SetActive(false);
        RankSounds.PlayOneShot(RankLabelSounds[1]);
        for (int i = 0; i <= 7; i++) {
            RankSounds.pitch = RankSounds.pitch + 0.025f;
            Game.Player.VibrateController(0.05f, 0.05f, 0.1f);
            RankSounds.PlayOneShot(RankLabelSounds[i + 2]);
            switch (i) {
                case 0 : EndingText.text = ""; break;
                case 1 : EndingText.text = "Результаты Забега!\n"; break;
                case 2 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex); break;
                case 3 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement; break;
                case 4 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement; break;
                case 5 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement; break; 
                case 6 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement + "\n" + specialConditionAdvancment; 
                RawVideoPlayer.SetActive(true);
                RankingVidPlayer.gameObject.SetActive(true); break;
                case 7 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement + "\n" + specialConditionAdvancment + "\n\nEnter/A что бы выйти"; break;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
} 
