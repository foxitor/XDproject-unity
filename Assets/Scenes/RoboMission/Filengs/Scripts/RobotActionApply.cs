using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RobotActionApply : MonoBehaviour {
    public RobotBrain Brain;
    public float CommandComplitionDelay;
    bool isComplitingCommand;

    public void ApplyAction(RobotActions Action, string BonusInfo) {
        StartCoroutine(SendReqest(Action, BonusInfo));
    }
    IEnumerator SendReqest(RobotActions Command, string Supplier) {
        isComplitingCommand = true;
        yield return new WaitForSeconds(CommandComplitionDelay);
        isComplitingCommand = false;
        switch (Command) {
            case RobotActions.Move :  Brain.Move(Supplier); break;
            case RobotActions.Charge : Brain.Charge(); break;
            default: Debug.Log("'FalseCall' to the action script."); break;
        }
    }
}
