using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoricalSeasonManage : MonoBehaviour {
    public Seasons Season;
    [Space]
    public GameObject WinterFeatures;
    public GameObject[] WinterMassive;
    public GameObject Grass;
    public Color[] GrassColors;

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
                foreach (GameObject Feature in WinterMassive) {
                    Feature.SetActive(false);
                }
                Grass.GetComponent<SpriteRenderer>().color = GrassColors[0];
                WinterFeatures.SetActive(false);
            break;
            case Seasons.Winter : 
                foreach (GameObject Feature in WinterMassive) {
                    Feature.SetActive(true);
                }
                Grass.GetComponent<SpriteRenderer>().color = GrassColors[1];
                WinterFeatures.SetActive(true);
            break;
            case Seasons.NullSeasonExpection : break;
        }
    }
}
