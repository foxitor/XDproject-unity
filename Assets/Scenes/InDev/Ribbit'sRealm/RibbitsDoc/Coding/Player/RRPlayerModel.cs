using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RRPlayerModel : MonoBehaviour {
    public GameObject[] BodyParts;
    RRPlayer Player;

    void Start() {
        Player = transform.parent.GetComponent<RRPlayer>();
    }
    void Update() {
        Quaternion camRot = Player.Cam.transform.rotation;
        //BodyParts[0].transform.rotation = new Quaternion(-camRot.x,-camRot.y,-camRot.z,-camRot.w);
    }
}
