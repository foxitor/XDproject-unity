using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotActionApply : MonoBehaviour {
    public RobotBrain Brain;
    public void ApplyAction(RobotActions Action, string BonusInfo) {
        switch (Action) {
            case RobotActions.Move : Brain.Move(BonusInfo); break;
            case RobotActions.Charge : Brain.Charge(); break;
            default: Debug.Log("FalseCall to the action script."); break;
        }
    }
}
