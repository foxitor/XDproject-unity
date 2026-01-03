using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class CommandInput : MonoBehaviour {
    public RobotActionApply RobotActionMaking;
    public void WhileTyping() {
        InputField MyField = this.gameObject.GetComponent<InputField>();
        MyField.text = MyField.text.Replace(",", "\n");
    }
    public void readNsend() {
        InputField MyField = this.gameObject.GetComponent<InputField>();
        //MyField.text = "Test";
        string[] commands = MyField.text.Split(new char[] { ',', ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string cmd in commands) {
            string command = cmd.ToLower();
            if (command.StartsWith("move(") && command.EndsWith(")")) {
                int startIdx = command.IndexOf('(') + 1;
                int endIdx = command.IndexOf(')');
                string direction = command.Substring(startIdx, endIdx - startIdx);
                RobotActionMaking.ApplyAction(RobotActions.Move, direction);
            }
            else if (command == "charge()") {
                RobotActionMaking.ApplyAction(RobotActions.Charge, "");
            }
            else {
                Debug.Log("Err01(Unknown command) : " + command);
            }
        }
    }
}
