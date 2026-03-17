using UnityEngine;

public class Item : MonoBehaviour {
    public int ItemID;
    public void PickUp() {
        UseItemID(); Destroy(this.gameObject);
    } private void UseItemID() {
        switch(ItemID) {
            case 0: if (GameObject.Find("Section01/Player").GetComponent<Player>().MagzineBullets <= 25) 
            { GameObject.Find("Section01/Player").GetComponent<Player>().MagzineBullets++; } break;
        }
    }
}
