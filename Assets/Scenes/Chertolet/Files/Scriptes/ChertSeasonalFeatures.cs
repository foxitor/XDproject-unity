using UnityEngine;

public class ChertSeasonalFeatures : MonoBehaviour {
    public Seasons Season;
    public Color[] RockColorStorage;
    public Color[] StormColorStorage;
    [Space]
    public GameObject WinterFeatures;

    [Space]
    public ParticleSystem[] StormParticles;
    public RockSpawn[] RockSpawners;

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
                foreach (RockSpawn Spawner in RockSpawners) {
                    Spawner.SetColor(RockColorStorage[0]);
                } foreach (ParticleSystem StormParticle in StormParticles) {
                    StormParticle.startColor = StormColorStorage[0];                    
                }
                WinterFeatures.SetActive(false);
            break;
            case Seasons.Winter : 
                foreach (RockSpawn Spawner in RockSpawners) {
                    Spawner.SetColor(RockColorStorage[1]);
                } foreach (ParticleSystem StormParticle in StormParticles) {
                    StormParticle.startColor = StormColorStorage[1];                    
                }
                WinterFeatures.SetActive(true);
            break;
            case Seasons.NullSeasonExpection : break;
        }
    }
}
