using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotActionApply : MonoBehaviour {
    public void ApplyAction(string Action) {
        switch (Action) {
            case "Move" : break;
            case "Charge" : break;
            default: Debug.Log("FalseCall to the action script."); break;
        }
    }
}
