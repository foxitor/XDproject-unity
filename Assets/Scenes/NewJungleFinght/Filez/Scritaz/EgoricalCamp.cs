using UnityEngine; using UnityEngine.SceneManagement;

public class EgoricalCamp : MonoBehaviour {
    public int CurBaseDurability = 3;
    public EgoricalGame Game;
    public AudioClip[] HurtAudios;
    AudioSource Source;

    void Start() {
        Source = gameObject.AddComponent<AudioSource>();
    }
    public void DamageCamp() {
        CurBaseDurability--;
        if (CurBaseDurability < 1) {
            Game.ReloadScene();
        } else {
            Source.PlayOneShot(HurtAudios[Random.Range(0, HurtAudios.Length)]);
            Game.Player.VibrateController(0.35f, 0.35f, 0.25f);
        }
    } 
}
