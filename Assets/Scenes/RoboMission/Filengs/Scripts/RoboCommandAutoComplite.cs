using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class RoboCommandAutoComplite : MonoBehaviour {
    public InputField inputField;
    public GameObject suggestionsContainer, suggestionPrefab;
    private List<string> allCommands = new List<string>() { "move(left)", "move(right)", "move(up)", "move(down)", "charge()" };
    private List<GameObject> currentSuggestions = new List<GameObject>();

    void Start() {
        inputField.onValueChanged.AddListener(OnInputChanged);
        suggestionsContainer.SetActive(false);
    }

    void OnInputChanged(string input) {
        foreach (var suggestion in currentSuggestions) {
            Destroy(suggestion);
        }
        currentSuggestions.Clear();

        if (string.IsNullOrEmpty(input)) { suggestionsContainer.SetActive(false); return; }

        var filteredCommands = allCommands.FindAll(cmd => cmd.StartsWith(input.ToLower()));

        if (filteredCommands.Count > 0) {
            suggestionsContainer.SetActive(true);

            Vector3 startPos = Vector3.zero;
            RectTransform containerRect = suggestionsContainer.GetComponent<RectTransform>();
            startPos = containerRect.localPosition;

            for (int i = 0; i < filteredCommands.Count; i++) {
                string cmd = filteredCommands[i];
                GameObject suggestionObj = Instantiate(suggestionPrefab, suggestionsContainer.transform);
                suggestionObj.GetComponentInChildren<Text>().text = cmd;
                RectTransform rt = suggestionObj.GetComponent<RectTransform>();
                Vector3 newPos = startPos + new Vector3(0, -30f * i+1, 0);
                rt.localPosition = newPos;

                suggestionObj.GetComponent<Button>().onClick.AddListener(() => {
                    inputField.text = cmd;
                    suggestionsContainer.SetActive(false);
                });
                currentSuggestions.Add(suggestionObj);
            }
        } else { suggestionsContainer.SetActive(false); }
    }
}