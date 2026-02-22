using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RRPlayerModel : MonoBehaviour {
    public GameObject[] BodyParts;
    public float LookBoarder = 30;
    RRPlayer Player;

    void Start() {
        Player = transform.parent.GetComponent<RRPlayer>();
        
    }
    void Update() {
        float xRotation = Player.Cam.transform.localEulerAngles.x;

            if (xRotation > 180f) {
                xRotation -= 360f;
            }
    
        float clampedX = Mathf.Clamp(xRotation, -LookBoarder, LookBoarder);
        BodyParts[0].transform.localEulerAngles = new Vector3(-clampedX, 0, 0);
    }
}
