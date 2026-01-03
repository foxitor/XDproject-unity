using System.Collections; using System.Collections.Generic; using UnityEngine;

public class Box : MonoBehaviour {
    public GameObject[] Items; public GameObject Cap;
    public Transform DropPosition; public bool Opened;
    public void Open() {
        if (!Opened) {
            int DoesDrop = Random.Range(0, 2); Cap.SetActive(false);
            Opened = true;
            if (DoesDrop == 1) {
                GameObject DropedItem = Instantiate(Items[0], DropPosition.position, Quaternion.identity, this.transform);
            }
        }
    } public void Restore() {
        Cap.SetActive(true);
        Opened = false;
    } public void ForceWinter() {
        Color WinterCap = new Color(0.25f,0.5f,0.25f,1f); Color WinterBase = new Color(0.5f,0.25f,0.25f,1f);
        Cap.GetComponent<SpriteRenderer>().color = WinterCap; this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = WinterBase;
    }
}
