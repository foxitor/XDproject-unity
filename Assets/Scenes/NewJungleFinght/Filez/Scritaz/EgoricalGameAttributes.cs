using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using UnityEngine.Video;

public class EgoricalGameAttributes : MonoBehaviour {
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
    [Space]
    //#Other References
    public EgoricalGame Game;
    [Space]
    //#SpecialFeatures
    public AudioClip CrazySigimaS;
    public AudioSource EndGameFeatureSource;

    void Start() {
        //
    }

    public void InsertWearable() {
        int totalWearables = WearableStorage.childCount; int usedWearableIndex = -1;
        for (int i = 0; i < totalWearables; i++) {
            WearableStorage.GetChild(i).GetComponent<EgoricalWearableModifier>().StartsGame();
        } 
    }   

    public void IsertDifficulty(int DifficultyIndex) {
        switch (DifficultyIndex) {
            case 0 : Difficulty = Difficulties.Easy; DifficultyDisplay = "Размокщая печенькаи"; break;
            case 1 : Difficulty = Difficulties.Normal; DifficultyDisplay = "Канон!"; break;
            case 2 : Difficulty = Difficulties.Hard; DifficultyDisplay = "Змиздец."; break;
            default : Difficulty = Difficulties.Unassigned; DifficultyDisplay = "Unassigned Difficulty Error..."; break;
        }
        ConfigureDifficulty();
    }
    void ConfigureDifficulty() {
        float ConfiguredPowerMultiplier = 0.5f; int ConfiguredLapQuota = 1, ConfiguredKillQuota = 0; float ConfiguredLapLength = 3f; float ConfiguredSnakeFreq = 5f;
        switch (Difficulty) {
            case Difficulties.Easy : ConfiguredPowerMultiplier = 1f; ConfiguredLapQuota = 4; ConfiguredKillQuota = 12; ConfiguredLapLength = 12.5f; ConfiguredSnakeFreq = 5f; break;
            case Difficulties.Normal : ConfiguredPowerMultiplier = 1.3f; ConfiguredLapQuota = 6; ConfiguredKillQuota = 50; ConfiguredLapLength = 17f; ConfiguredSnakeFreq = 2.5f; break;
            case Difficulties.Hard : ConfiguredPowerMultiplier = 1.5f; ConfiguredLapQuota = 7; ConfiguredKillQuota = 120; ConfiguredLapLength = 23f; ConfiguredSnakeFreq = 1.5f; break;
            case Difficulties.Unassigned : ConfiguredPowerMultiplier = 0.5f; ConfiguredLapQuota = 1; ConfiguredLapLength = 3f; ConfiguredSnakeFreq = 3f; break;
        }
        Game.SetupConfiguredDifficulty(ConfiguredPowerMultiplier, ConfiguredLapQuota, ConfiguredLapLength, ConfiguredKillQuota, DifficultyDisplay, ConfiguredSnakeFreq, DifficultyFeature);
    }

    public void EndSession() {
        //#Setup
        BlackScreen.SetActive(true);
        RawVideoPlayer.SetActive(false);
        RankingVidPlayer.gameObject.SetActive(false);
        RankScreen.SetActive(true); 
        Game.Music.gameObject.SetActive(false);
        VideoClip RankVideoClip;

        switch (Game.Player.gameObject.GetComponent<SpriteRenderer>().sprite.name) {
            case "SnakeKiller" : RankVideoClip = RankVideoClips[0]; break;
            case "SnakeKillerCreepy" : RankVideoClip = RankVideoClips[1]; break;
            default: RankVideoClip = RankVideoClips[0]; break;
        }
        //#Past method :
        
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
            case 0 : ReturnResult = "Они меня трахнули уже знаешь сколько раз?"; break;
            case 1 : ReturnResult = "ДА ИДИ ТЫ НАХУЙ ЗМЕЯ КОНЧЕНАЯ"; break;
            case 2 : ReturnResult = "Они сожгли собаку..."; break;
            case 3 : ReturnResult = "Шигима сошёл с ума"; break;
            case 4 : ReturnResult = "Баебрих офигевал в сторонке."; break;
            case 5 : ReturnResult = "А потом я достаю свою бензопилу..."; break;
        }
        return ReturnResult;
    }
    string AnalizeEndingAchivement(int EndingIndex) {
        string NewEndingReturn = "∘"; bool EndingIsUnlocked = false;
        switch (EndingIndex) {
            case 0 : if (PlayerPrefs.GetInt("NastySnakes") == 0 || !PlayerPrefs.HasKey("NastySnakes")) { EndingIsUnlocked = true; }
            PlayerPrefs.SetInt("NastySnakes", 1); break;
            case 1 : if (PlayerPrefs.GetInt("FuckUrSnakes") == 0 || !PlayerPrefs.HasKey("FuckUrSnakes")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("FuckUrSnakes", 1); break;
            case 2 : if (PlayerPrefs.GetInt("CrustyDog") == 0 || !PlayerPrefs.HasKey("CrustyDog")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("CrustyDog", 1); break;
            case 3 : if (PlayerPrefs.GetInt("CrazySigima") == 0 || !PlayerPrefs.HasKey("CrazySigima")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("CrazySigima", 1); break;
            case 4 : if (PlayerPrefs.GetInt("NewGenSnake") == 0 || !PlayerPrefs.HasKey("NewGenSnake")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("NewGenSnake", 1); break;
            case 5 : if (PlayerPrefs.GetInt("BloodySaw") == 0 || !PlayerPrefs.HasKey("BloodySaw")) { EndingIsUnlocked = true; } 
            PlayerPrefs.SetInt("BloodySaw", 1); break;
        }
        if (EndingIsUnlocked) { NewEndingReturn = "• Достижение за концовку!"; }
        return NewEndingReturn;
    }
    string AnalizeDifficultyAchivement() {
        string newDifficultyReturn = "∘"; bool DifficultAdvUnlocked = false;
        if (Difficulty == Difficulties.Easy) { 
            if (PlayerPrefs.GetInt("SnakeKiller-Newborn") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Newborn")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Newborn", 1); 
        }
        if (Difficulty == Difficulties.Normal) { 
            if (PlayerPrefs.GetInt("SnakeKiller-Standart") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Standart")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Standart", 1); 
        }
        if (Difficulty == Difficulties.Hard) { 
            if (PlayerPrefs.GetInt("SnakeKiller-Expert") == 0 || !PlayerPrefs.HasKey("SnakeKiller-Expert")) 
                { DifficultAdvUnlocked = true; } PlayerPrefs.SetInt("SnakeKiller-Expert", 1); 
        }
        if (DifficultAdvUnlocked) { newDifficultyReturn = "• Достижение за сложность!"; }
        return newDifficultyReturn;
    }
    string AnalizeSeasonAchivement() {
        bool SeasonAdvUnlocked = false; string newSeasonReturn = "∘";
        if (Game.SeasonManage.Season == Seasons.Winter) { 
            if (PlayerPrefs.GetInt("ColdFights") == 0 || !PlayerPrefs.HasKey("ColdFights")) { SeasonAdvUnlocked = true; }
            PlayerPrefs.SetInt("ColdFights", 1); 
        }
        if (SeasonAdvUnlocked) { newSeasonReturn = "• Достижение за сезон!"; }
        return newSeasonReturn;
    }
    string AnalizeConditionAchivement() {
        bool ConditionUnlocked = false; string newConditionReturn = "∘";
        int ConditionsCount = 0;
        if (Game.Kills >= 250) {
            if (PlayerPrefs.GetInt("Snake-o-Madness") == 0 || !PlayerPrefs.HasKey("Snake-o-Madness")) { ConditionUnlocked = true; }
            PlayerPrefs.SetInt("Snake-o-Madness", 1); ConditionsCount++;
            //Debug.Log("Snake-o-Madness");
        } 
        if (Game.Player.ShareHealthValue() == 3 && Game.Camp.CurBaseDurability == 3) { 
            if (PlayerPrefs.GetInt("NoDamageSnake") == 0 || !PlayerPrefs.HasKey("NoDamageSnake")) { ConditionUnlocked = true; }
            PlayerPrefs.SetInt("NoDamageSnake", 1); ConditionsCount++; 
            //Debug.Log("NoDamageSnake");
        }
        //Debug.Log("ConditionsCount is " + ConditionsCount);
        if (ConditionUnlocked) { newConditionReturn = (ConditionsCount <= 1 ? "• Достижение за условие!" : "• Достижение за условия!! (" + ConditionsCount + ")"); }
        return newConditionReturn;
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
                case 2 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex); if (EndingIndex == 3) { EndGameFeatureSource.PlayOneShot(CrazySigimaS); } break;
                case 3 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement; break;
                case 4 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement; break;
                case 5 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement; break; 
                case 6 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement + "\n" + specialConditionAdvancment; 
                RawVideoPlayer.SetActive(true);
                RankingVidPlayer.gameObject.SetActive(true); break;
                case 7 : EndingText.text = "Результаты Забега :\nКонцовка : " + DefineEndVariant(EndingIndex) + "\n\n" + newEndingAchivement + "\n" + newDifficultyAchivement + "\n" + newSeasonAchivement + "\n" + specialConditionAdvancment + "\n\nEnter что бы выйти"; break;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
} 
