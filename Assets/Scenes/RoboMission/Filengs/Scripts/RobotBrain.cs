using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotBrain : MonoBehaviour {
    public float LeftEnergy; public float MovementPower = 1f;
    public void Move(string Direction) {
        Vector3 Axis = Vector3.zero;
        switch (Direction) {
            case "up" : Axis = new Vector3(0, 1, 0); break;
            case "down" : Axis = new Vector3(0, -1, 0); break;
            case "right" : Axis = new Vector3(1, 0, 0); break;
            case "left" : Axis = new Vector3(-1, 0, 0); break;
            default: break;
        }
        Vector3 MultipliedAxis = new Vector3(Axis.x * MovementPower, Axis.y * MovementPower, 0); 
        Vector3 MovedPosition = new Vector3(transform.position.x + MultipliedAxis.x, transform.position.y + MultipliedAxis.y,0);
        transform.position = MovedPosition;
    }
    public void Charge() {
        Debug.Log("Charging" + isOnCharger());
    }
    bool isOnCharger() {
        return true;
    }
}
