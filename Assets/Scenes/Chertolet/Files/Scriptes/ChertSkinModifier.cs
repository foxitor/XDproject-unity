using UnityEngine; using UnityEngine.InputSystem;

public class ChertSkinModifier : MonoBehaviour {
    public bool LastSkin, Achived, InUse; private GameObject Select;
    public string PPrefName; public Sprite Skin;
    ChertoletControls Controls;

    //GamePad
    void Awake() { 
        Controls = new ChertoletControls();
        Controls.Wearables.Wearable1.performed += Ctx => SelectThis(1);
        Controls.Wearables.Wearable2.performed += Ctx => SelectThis(2);
        Controls.Wearables.Wearable3.performed += Ctx => SelectThis(3);
        Controls.Wearables.Wearable4.performed += Ctx => SelectThis(4);
    }
    void OnEnable() { Controls.Wearables.Enable();} 
    void OnDisable() { Controls.Wearables.Disable(); }
    //-
    void SelectThis(int Child) {
        Transform parentTransform = transform.parent;

        if (parentTransform != null) {
            int index = Child - 1;
            if (index >= 0 && index < parentTransform.childCount) {
                Transform childTransform = parentTransform.GetChild(index);
                if (childTransform != null) {
                    var modifier = childTransform.GetComponent<ChertSkinModifier>();
                    if (modifier == this) { Boop(); }
                }
            }
        }
    }

    void Start() {
        Select = transform.GetChild(0).gameObject; InUse = false;
        if (PPrefName != "ChertDef") { if (PlayerPrefs.GetInt(PPrefName) == 1) { Achived = true; } } else { Achived = true; }
        if (PlayerPrefs.GetInt(PPrefName + "LastUsed") == 1) { LastSkin = true; }
        if (LastSkin) { InUse = true; }
        transform.GetChild(1).gameObject.SetActive(!Achived);
    }
    public void Boop() {
        if (Achived) {
            transform.parent.GetChild(0).GetComponent<ChertSkinModifier>().InUse = false;
            transform.parent.GetChild(1).GetComponent<ChertSkinModifier>().InUse = false;
            transform.parent.GetChild(2).GetComponent<ChertSkinModifier>().InUse = false;
            InUse = !InUse;
        }
    }
    public void StartsGame() {
        if (InUse) { PlayerPrefs.SetInt(PPrefName + "LastUsed", 1); 
        GameObject plr = GameObject.Find("GameController/Chertolet").gameObject; 
        plr.GetComponent<SpriteRenderer>().sprite = Skin;
        } else { PlayerPrefs.SetInt(PPrefName + "LastUsed", 0); }
    }
    void Update() {
        Select.SetActive(InUse);
    }
}
