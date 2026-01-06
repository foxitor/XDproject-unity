using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotBrain : MonoBehaviour {
    public float LeftEnergy, MaxEnergy; public float MovementPower = 1f;
    GameObject StandingOn;

    void Start() {
        MaxEnergy = LeftEnergy;
    }
    public void Move(string Direction) {
        Vector3 Axis = Vector3.zero;
        switch (Direction) {
            case "up": Axis = new Vector3(0, 1, 0); break;
            case "down": Axis = new Vector3(0, -1, 0); break;
            case "right": Axis = new Vector3(1, 0, 0); break;
            case "left": Axis = new Vector3(-1, 0, 0); break;
            default: return;
        }

        Vector3 direction = new Vector3(Axis.x, Axis.y, 0).normalized;
        Vector3 MultipliedAxis = new Vector3(Axis.x * MovementPower, Axis.y * MovementPower, 0); 
        Vector3 MovedPosition = new Vector3(transform.position.x + MultipliedAxis.x, transform.position.y + MultipliedAxis.y, 0);
        transform.position = MovedPosition;
        LeftEnergy -= 0.5f;
    }
    public void OnTriggerEnter2D(Collider2D Collider) {
        StandingOn = Collider.gameObject;
    } public void OnTriggerExit2D(Collider2D Collider) {
        StandingOn = null;
    }
    public void Charge() {
        if (DefineSurfaceID() == "Charger") {
            LeftEnergy = MaxEnergy;
        }
    }
    string DefineSurfaceID() {
        string returnText = "";
        if (StandingOn != null) {
            returnText = StandingOn.name;
        }
        return returnText;
    }
}
