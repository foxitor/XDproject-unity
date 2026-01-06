using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using UnityEngine.Video;
using UnityEngine.InputSystem; using UnityEngine.SceneManagement;

public class ChertGameAttributes : MonoBehaviour {
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

    //#Ending
    public Text EndingText; 
    int PossibleEndings = 6;

    //#Controlls
    ChertoletControls Controls;
    [Space]
    //#Other References
    public GamManag Game;
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
            //var wearable = WearableStorage.GetChild(i).GetComponent<ChertSkinModifier>();
            //if (wearable.InUse) {
            //    usedWearableIndex = i;
            //    break;
            //}
            WearableStorage.GetChild(i).GetComponent<ChertSkinModifier>().StartsGame();
        } 
    }   

    public void IsertDifficulty(int DifficultyIndex) {
        switch (DifficultyIndex) {
            case 0 : Difficulty = Difficulties.Easy; DifficultyDisplay = "Сонный кирпич~"; break;
            case 1 : Difficulty = Difficulties.Normal; DifficultyDisplay = "Канон!"; break;
            case 2 : Difficulty = Difficulties.Hard; DifficultyDisplay = "Тяжко."; break;
            default : Difficulty = Difficulties.Unassigned; DifficultyDisplay = "Unassigned Difficulty Error..."; break;
        }
        ConfigureDifficulty();
    }
    void ConfigureDifficulty() {
        float ConfiguredPowerMultiplier = 0.5f; int ConfiguredLapQuota = 1; float ConfiguredLapLength = 3f;
        switch (Difficulty) {
            case Difficulties.Easy : ConfiguredPowerMultiplier = 1f; ConfiguredLapQuota = 4; ConfiguredLapLength = 10f; break;
            case Difficulties.Normal : ConfiguredPowerMultiplier = 1.3f; ConfiguredLapQuota = 7; ConfiguredLapLength = 20f; break;
            case Difficulties.Hard : ConfiguredPowerMultiplier = 1.5f; ConfiguredLapQuota = 7; ConfiguredLapLength = 13f; DifficultyFeature = "Storm"; break;
            case Difficulties.Unassigned : ConfiguredPowerMultiplier = 0.5f; ConfiguredLapQuota = 1; ConfiguredLapLength = 3f; break;
        }
        Game.SetupConfiguredDifficulty(ConfiguredPowerMultiplier, ConfiguredLapQuota, ConfiguredLapLength, DifficultyDisplay, DifficultyFeature);
        inGame = true;
    }

    public void EndSession() {
        //#Setup
        BlackScreen.SetActive(true);
        RawVideoPlayer.SetActive(false);
        RankingVidPlayer.gameObject.SetActive(false);
        RankScreen.SetActive(true); 
        Game.Music.SetActive(false);
        VideoClip RankVideoClip;

        switch (Game.Chertolet.gameObject.GetComponent<SpriteRenderer>().sprite.name) {
            case "Shit_0" : RankVideoClip = RankVideoClips[0]; break;
            case "Shit_5" : RankVideoClip = RankVideoClips[1]; break;
            case "Shit_7" : RankVideoClip = RankVideoClips[2]; break;
            default: RankVideoClip = RankVideoClips[0]; break;
        }
        //#Past method :

        //if (Chertolet.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Shit_0") { RankVideoClip = RankVideoClips[0]; } 
        //else if (Chertolet.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Shit_5") { RankVideoClip = RankVideoClips[1]; } 
        //else { RankVideoClip = RankVideoClips[2]; }
        
        //#Appling
        RankingVidPlayer.clip = RankVideoClip;

        //#EndingScreenVaruables :
        string EndingAchivementUnlockedText = "∘", DifficultAdv = "∘", SeasonAdv = "∘", ConditionAdv = "∘ (не существует)";
        bool EndingIsUnlocked = false, DifficultAdvUnlocked = false, SeasonAdvUnlocked = false;
        int EndVariant = Random.Range(0, PossibleEndings);

        EndingAchivementUnlockedText = AnalizeEndingAchivement(EndVariant);
        DifficultAdv = AnalizeDifficultyAchivement();
        SeasonAdv = AnalizeSeasonAchivement();

        //#RunLables
        StartCoroutine(EndingResultText(EndVariant, EndingAchivementUnlockedText, DifficultAdv, SeasonAdv, ConditionAdv));

        //#•∘ lit & unlit dot symbols.
    }
    #region Analizing
    //#Analizing
    string DefineEndVariant(int EndingIndex) {
        string ReturnResult = "Unable to Translate Index.";
        switch (EndingIndex) {
            case 0 : ReturnResult = "Я запиздил сигиму"; break;
            case 1 : ReturnResult = "Он взрорвал мой чертолёт к хуям"; break;
            case 2 : ReturnResult = "Он pахуярил меня последним патроном"; break;
            case 3 : ReturnResult = "Я Разэбался.."; break;
            case 4 : ReturnResult = "Я задавил сигиму нахой!"; break;
            case 5 : ReturnResult = "Пчела застилила килл..."; break;
        }
        return ReturnResult;
    }
    string AnalizeEndingAchivement(int EndingIndex) {
        string NewEndingReturn = "∘"; bool EndingIsUnlocked = false;
        switch (EndingIndex) {
            case 0 : if (PlayerPrefs.GetInt("TrueEnding") == 0 || !PlayerPrefs.HasKey("TrueEnding")) { EndingIsUnlocked = true; }
             PlayerPrefs.SetInt("TrueEnding", 1); break;
            case 1 : if (PlayerPrefs.GetInt("BigBoom") == 0 || !PlayerPrefs.HasKey("BigBoom")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("BigBoom", 1); break;
            case 2 : if (PlayerPrefs.GetInt("Lucky") == 0 || !PlayerPrefs.HasKey("Lucky")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("Lucky", 1); break;
            case 3 : if (PlayerPrefs.GetInt("Crash") == 0 || !PlayerPrefs.HasKey("Crash")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("Crash", 1); break;
            case 4 : if (PlayerPrefs.GetInt("TheIncedent") == 0 || !PlayerPrefs.HasKey("TheIncedent")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("TheIncedent", 1); break;
            case 5 : if (PlayerPrefs.GetInt("Bee") == 0 || !PlayerPrefs.HasKey("Bee")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("Bee", 1); break;
        }
        if (EndingIsUnlocked) { NewEndingReturn = "• Достижение за концовку!"; }
        return NewEndingReturn;
    }
    string AnalizeDifficultyAchivement() {
        string newDifficultyReturn = "∘"; bool DifficultAdvUnlocked = false;
        if (Difficulty == Difficulties.Easy) { 
            if (PlayerPrefs.GetInt("EasyPeasy") == 0 || !PlayerPrefs.HasKey("EasyPeasy")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("EasyPeasy", 1); 
        }
        if (Difficulty == Difficulties.Normal) { 
            if (PlayerPrefs.GetInt("GettingHarder") == 0 || !PlayerPrefs.HasKey("GettingHarder")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("GettingHarder", 1); 
        }
        if (Difficulty == Difficulties.Hard) { 
            if (PlayerPrefs.GetInt("StormyNight") == 0 || !PlayerPrefs.HasKey("StormyNight")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("StormyNight", 1); 
        }
        if (DifficultAdvUnlocked) { newDifficultyReturn = "• Достижение за сложность!"; }
        return newDifficultyReturn;
    }
    string AnalizeSeasonAchivement() {
        bool SeasonAdvUnlocked = false; string newSeasonReturn = "∘";
        if (Game.SeasonManage.Season == Seasons.Winter) { 
            if (PlayerPrefs.GetInt("SnowcarpedChertolet") == 0 || !PlayerPrefs.HasKey("SnowcarpedChertolet")) { SeasonAdvUnlocked = true; }
            PlayerPrefs.SetInt("SnowcarpedChertolet", 1); 
        }
        if (SeasonAdvUnlocked) { newSeasonReturn = "• Достижение за сезон!"; }
        return newSeasonReturn;
    }
    #endregion

    //#Appling
    IEnumerator EndingResultText(int EndingIndex, string newEndingAchivement,
            string newDifficultyAchivement, string newSeasonAchivement, string specialConditionAdvancment) {
        AudioSource RankSounds = RankScreen.AddComponent<AudioSource>();
        RankSounds.PlayOneShot(RankLabelSounds[0]);
        yield return new WaitForSeconds(1.5f);
        BlackScreen.SetActive(false);
        RankSounds.PlayOneShot(RankLabelSounds[1]);
        for (int i = 0; i <= 7; i++) {
            RankSounds.pitch = RankSounds.pitch + 0.025f;
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
