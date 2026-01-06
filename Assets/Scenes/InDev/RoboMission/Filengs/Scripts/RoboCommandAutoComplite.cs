using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        foreach (var suggestion in currentSuggestions) { Destroy(suggestion); }
        currentSuggestions.Clear();

        string[] commaParts = input.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> allParts = new List<string>();
        foreach (var part in commaParts) {
            allParts.AddRange(part.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
        }

        string lastPart = "";
        if (allParts.Count > 0) {
            lastPart = allParts[allParts.Count - 1].Trim().ToLower();
        }

        if (string.IsNullOrEmpty(lastPart)) { suggestionsContainer.SetActive(false); return; }

        var filteredCommands = allCommands.FindAll(cmd => cmd.StartsWith(lastPart));
        if (filteredCommands.Count > 0) {
            suggestionsContainer.SetActive(true);
            Vector3 startPos = Vector3.zero;
            RectTransform containerRect = suggestionsContainer.GetComponent<RectTransform>();
            startPos = new Vector3(0, 0, containerRect.localPosition.z);

            for (int i = 0; i < filteredCommands.Count; i++) {
                string cmd = filteredCommands[i];
                GameObject suggestionObj = Instantiate(suggestionPrefab, suggestionsContainer.transform);
                suggestionObj.GetComponentInChildren<Text>().text = cmd;
                RectTransform rt = suggestionObj.GetComponent<RectTransform>();
                Vector3 newPos = startPos + new Vector3(0, -30f * i, 0);
                rt.localPosition = newPos;
                suggestionObj.GetComponent<Button>().onClick.AddListener(() => {
                    allParts[allParts.Count - 1] = cmd;
                    string newInput = "";
                    for (int j = 0; j < allParts.Count; j++) {
                        if (j > 0) newInput += ", ";
                        newInput += allParts[j];
                    }
                    inputField.text = newInput; suggestionsContainer.SetActive(false);
                });
                currentSuggestions.Add(suggestionObj);
            }
        } else {
            suggestionsContainer.SetActive(false);
        }
    }
}