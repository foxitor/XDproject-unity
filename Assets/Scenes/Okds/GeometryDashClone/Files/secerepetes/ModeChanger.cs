using UnityEngine;

public class ModeChanger : MonoBehaviour {
    public GameObject Section1, Section2;
    public void Change() { Section1.SetActive(false); Section2.SetActive(true); }
}
