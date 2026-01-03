using System.Collections.Generic; using UnityEngine; using System.Collections;

public class RobotActionApply : MonoBehaviour {
    public CommandInput Console;
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
        Console.gameObject.SetActive(!isComplitingCommand);
    }

    IEnumerator ProcessNextCommand() {
        while (commandQueue.Count > 0) {
            var (action, info) = commandQueue.Dequeue();
            yield return StartCoroutine(SendRequest(action, info));
        }
    }

    IEnumerator SendRequest(RobotActions Command, string Supplier) {
        isComplitingCommand = true;
        yield return new WaitForSeconds(CommandComplitionDelay);
        switch (Command) {
            case RobotActions.Move:
                Brain.Move(Supplier);
                break;
            case RobotActions.Charge:
                Brain.Charge();
                break;
            default:
                Debug.Log("'FalseCall' to the action script.");
                break;
        }
        isComplitingCommand = false; if (commandQueue.Count > 0) { yield return StartCoroutine(ProcessNextCommand()); }
    }
}