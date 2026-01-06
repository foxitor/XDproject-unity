using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProtection : MonoBehaviour {
    public GameObject Shield; public Transform ShieldPivot; public AudioClip[] AudioLib;
    public int MaxHP, CurHP;

    void Start() { CurHP = MaxHP; }
    void Update() { if (CurHP < 1) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); } }
    public void Damage() { CurHP--; this.gameObject.GetComponent<AudioSource>().PlayOneShot(AudioLib[0]); }
}