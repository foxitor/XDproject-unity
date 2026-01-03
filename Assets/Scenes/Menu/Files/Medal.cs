using UnityEngine; using UnityEngine.UI;

public class Medal : MonoBehaviour {
    Image Render; public string Achivment;
    void Start() {
        Render = transform.GetChild(0).GetComponent<Image>();
    } void Update() {
        if (PlayerPrefs.GetInt(Achivment) == 1) { Render.gameObject.SetActive(true); }
        if (PlayerPrefs.GetInt(Achivment) == 0) { Render.gameObject.SetActive(false); }
        if (!PlayerPrefs.HasKey(Achivment)) { Render.gameObject.SetActive(false); }
    }
}
