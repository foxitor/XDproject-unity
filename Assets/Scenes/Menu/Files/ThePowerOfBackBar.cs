using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class ThePowerOfBackBar : MonoBehaviour {
    private AudioSource SoundBrubrubru; public AudioClip[] SmexSounds;
    public GameObject Advants, Setinges, Medales; public bool AdvantsShowing, GamesShowing, SetengShowing, MedalesShowing;
    public Animator[] SmexUtils; public Dropdown Seasons;
    public GameObject[] GameButtons;
    void Start() {
        SoundBrubrubru = this.gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("LastSeason")) {
            Seasons.value = PlayerPrefs.GetInt("LastSeason");
        }
    }
    public void smexuyatina() {
        int SmexPicked = Random.Range(0,SmexSounds.Length); SoundBrubrubru.PlayOneShot(SmexSounds[SmexPicked]);
        SmexUtils[SmexPicked].Play("StartGlow");
    } public void SwitchAdvants() {
        AdvantsShowing = !AdvantsShowing; Advants.SetActive(AdvantsShowing);
    } public void SwitchGames() {
        GamesShowing = !GamesShowing; 
        foreach (GameObject Button in GameButtons) { Button.SetActive(GamesShowing); }
    } public void SwitchSetinges() {
        SetengShowing = !SetengShowing; Setinges.SetActive(SetengShowing);
    } public void SwitchMedales() {
        MedalesShowing = !MedalesShowing; Medales.SetActive(MedalesShowing);
    } public void DeleteAll() {
        PlayerPrefs.DeleteAll();
        Seasons.value = 0;
    } public void SeasonValueChanged() {
        //Debug.Log(Seasons.value.ToString());
        PlayerPrefs.SetInt("LastSeason", Seasons.value);
    }
}
