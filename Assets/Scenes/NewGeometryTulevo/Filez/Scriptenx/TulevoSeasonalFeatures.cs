using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TulevoSeasonalFeatures : MonoBehaviour {
    public Seasons Season;
    [Space]
    public GameObject WinterFeatures;
    public GameObject PlayerSnow;

    public void DefineSeason() {
        if (PlayerPrefs.HasKey("LastSeason")) {
            int seasonData = PlayerPrefs.GetInt("LastSeason");
            switch (seasonData) {
                case 0 : Season = Seasons.Default; break;
                case 1 : Season = Seasons.Winter; break;
                default: Season = Seasons.NullSeasonExpection; break;
            }
        } else { Season = Seasons.Default; }
        ApplySeasonalChanges();
    }
    void ApplySeasonalChanges() {
        switch (Season) {
            case Seasons.Default : 
                WinterFeatures.SetActive(false);
                PlayerSnow.SetActive(false);
            break;
            case Seasons.Winter : 
                WinterFeatures.SetActive(true);
                PlayerSnow.SetActive(true);
            break;
            case Seasons.NullSeasonExpection : break;
        }
    }
}
