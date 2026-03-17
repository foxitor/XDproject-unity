using UnityEngine;

public class LinkManager : MonoBehaviour {
    public string link;
    public void OpenLink() {
        Application.OpenURL(link);
    }
}
