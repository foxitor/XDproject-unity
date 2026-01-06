using UnityEngine;

public class Music : MonoBehaviour {
    public AudioClip[] Clips;
    private AudioSource audioSource;

    void Awake() { audioSource = GetComponent<AudioSource>(); }
    void Start() { PlayRandomClip(); }
    private void PlayRandomClip() {
        int randomIndex = Random.Range(0, Clips.Length);
        audioSource.clip = Clips[randomIndex];
        audioSource.Play();
    }
}