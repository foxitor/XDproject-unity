using UnityEngine;

public class TullWearableModifier : MonoBehaviour {
    public bool LastSkin, Achived, InUse; private GameObject Select;
    public string PPrefName; public Sprite Skin;
    void Start() {
        Select = transform.GetChild(0).gameObject; InUse = false;
        if (PPrefName != "TullDef") { if (PlayerPrefs.GetInt(PPrefName) == 1) { Achived = true; } } else { Achived = true; }
        if (PlayerPrefs.GetInt(PPrefName + "LastUsed") == 1) { LastSkin = true; }
        if (LastSkin) { InUse = true; }
        transform.GetChild(1).gameObject.SetActive(!Achived);
    }
    public void Boop() {
        if (Achived) {
            transform.parent.GetChild(0).GetComponent<TullWearableModifier>().InUse = false;
            transform.parent.GetChild(1).GetComponent<TullWearableModifier>().InUse = false;
            InUse = !InUse;
        }
    }
    public void StartsGame() {
        if (InUse) { PlayerPrefs.SetInt(PPrefName + "LastUsed", 1); 
        GameObject plr = GameObject.Find("Game/Shigima/VisualPlayer").gameObject; 
        plr.GetComponent<SpriteRenderer>().sprite = Skin;
        } else { PlayerPrefs.SetInt(PPrefName + "LastUsed", 0); }
    }
    void Update() {
        Select.SetActive(InUse);
    }
}
