using UnityEngine;

public class Portal : MonoBehaviour {
    public Transform TeleportTransform;
    public bool AddScore = true;

    public void Teleport() {
        GameObject Player = GameObject.Find("Section01/Player");
        Player.GetComponent<Transform>().position = TeleportTransform.position;
        GameObject GameManager = GameObject.Find("Section01/GameManager");
        if (AddScore) { GameManager.GetComponent<GDGameManager>().PlusLayer(); }
    }
}
