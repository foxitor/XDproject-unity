using System.Collections.Generic; using UnityEngine; using System.Collections;

public class RobotActionApply : MonoBehaviour {
    public GameObject ConsoleGroup;
    public RobotBrain Brain; 
    public float CommandComplitionDelay;
    //#comandHelding
    bool isComplitingCommand; 
    Queue<(RobotActions, string)> commandQueue = new Queue<(RobotActions, string)>();

    public void ApplyAction(RobotActions Action, string BonusInfo) {
        commandQueue.Enqueue((Action, BonusInfo));
        if (!isComplitingCommand) {
            StartCoroutine(ProcessNextCommand());
        }
    }
    void Update() {
        ConsoleGroup.SetActive(!isComplitingCommand);
    }

    IEnumerator ProcessNextCommand() {
        while (commandQueue.Count > 0) {
            var (action, info) = commandQueue.Dequeue();
            yield return StartCoroutine(SendRequest(action, info));
        }
    }

    IEnumerator SendRequest(RobotActions Command, string Supplier) {
        float energyRatio = Brain.LeftEnergy / Brain.MaxEnergy;
        float speedFactor = Mathf.Lerp(5f, 0.05f, energyRatio);
        isComplitingCommand = true;
        yield return new WaitForSeconds(CommandComplitionDelay * speedFactor + 0.5f);
        switch (Command) {
            case RobotActions.Move:
                Brain.Move(Supplier);
                break;
            case RobotActions.Charge:
                Brain.Charge();
                break;
            case RobotActions.Look:
                Brain.Look(Supplier);
                break;
            case RobotActions.ShockWave:
                Brain.ShockWave(int.Parse(Supplier));
                break;
            default:
                Debug.Log("'FalseCall' to the action script.");
                break;
        }
        isComplitingCommand = false; if (commandQueue.Count > 0) { yield return StartCoroutine(ProcessNextCommand()); }
    }
}