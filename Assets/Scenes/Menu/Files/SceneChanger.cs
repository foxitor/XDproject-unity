using UnityEngine; using UnityEngine.SceneManagement;

public class AutoSceneLoad : MonoBehaviour {
    public string sceneName;
    public void Push() {
        SceneManager.LoadScene(sceneName);
    } public void Quit() {
        Application.Quit();
    }
}
