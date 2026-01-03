using UnityEngine; using UnityEngine.UI;

public class Advancment : MonoBehaviour {
    public string PPrefName; private Toggle Togl;
    public bool AllInAchivment; public string[] Allins;
    void Start() {
        Togl = transform.GetChild(0).GetComponent<Toggle>();
    }
    void Update() {
        if (!AllInAchivment) {
            if (PlayerPrefs.GetInt(PPrefName) == 1) { Togl.isOn = true; }
            if (PlayerPrefs.GetInt(PPrefName) == 0) { Togl.isOn = false; }
        } else {
            bool allAchieved = true;
                foreach (string allin in Allins) {
                    if (PlayerPrefs.GetInt(allin) != 1 || !PlayerPrefs.HasKey(allin)) { allAchieved = false; break; }
                } 
            if (allAchieved) {
                PlayerPrefs.SetInt(PPrefName, 1);
            } else { PlayerPrefs.SetInt(PPrefName, 0); }
                Togl.isOn = allAchieved;
        }
    }
}
