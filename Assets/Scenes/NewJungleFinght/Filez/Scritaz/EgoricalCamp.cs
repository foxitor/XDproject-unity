using UnityEngine; using UnityEngine.SceneManagement;

public class EgoricalCamp : MonoBehaviour {
    public int CurBaseDurability = 3;
    public AudioClip[] HurtAudios;
    AudioSource Source;

    void Start() {
        Source = gameObject.AddComponent<AudioSource>();
    }
    public void DamageCamp() {
        CurBaseDurability--;
        if (CurBaseDurability < 1) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        } else {
            Source.PlayOneShot(HurtAudios[Random.Range(0, HurtAudios.Length)]);
        }
    } 
}
