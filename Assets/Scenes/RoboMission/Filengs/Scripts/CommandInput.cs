using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class CommandInput : MonoBehaviour {
    public RobotActionApply RobotActionMaking;
    public float CommandComplitionDelay;
    public void readNsend() {
        InputField MyField = this.gameObject.GetComponent<InputField>();
        //MyField.text = "Test";
        string[] commands = MyField.text.Split(new char[] { ',', ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string command in commands) {
            string trimmedCommand = command.Trim().ToLower();
            switch (trimmedCommand) {
                case "move(left)":
                    RobotActionMaking.ApplyAction(RobotActions.Move, "left");
                    break;
                case "move(up)":
                    RobotActionMaking.ApplyAction(RobotActions.Move, "up");
                    break;
                case "move(right)":
                    RobotActionMaking.ApplyAction(RobotActions.Move, "right");
                    break;
                case "move(down)":
                    RobotActionMaking.ApplyAction(RobotActions.Move, "down");
                    break;
                case "charge()":
                    RobotActionMaking.ApplyAction(RobotActions.Charge, "");
                    break;
                default:
                    Debug.Log("Err01(UnknownCommand) : " + command);
                    break;
            }
        }
    }
}
