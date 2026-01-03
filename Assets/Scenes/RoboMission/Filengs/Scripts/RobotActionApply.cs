using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotActionApply : MonoBehaviour {
    public RobotBrain Brain;
    public void ApplyAction(string Action, string BonusInfo) {
        switch (Action) {
            case "Move" : Brain.Move(BonusInfo); break;
            case "Charge" : Brain.Charge(); break;
            default: Debug.Log("FalseCall to the action script."); break;
        }
    }
}
