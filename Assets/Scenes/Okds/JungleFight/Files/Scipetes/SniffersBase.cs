using UnityEngine; using UnityEngine.SceneManagement;

public class SniffersBase : MonoBehaviour {
    public int Hurten;
    public void DamageCamp() {
        Hurten++;
        if (Hurten >= 15) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
