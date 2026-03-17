using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum Voices {
    Sabre,
    Backshot
}

public class VoiceContainerData : MonoBehaviour {
    public Voices Voice;
    AudioSource CharacterSpeaker;
    public AudioClip[] VoiceSounds, MissSounds;

    void Start() {
        CharacterSpeaker = this.gameObject.GetComponent<AudioSource>();
    }
    public void SingNote(int Note) {
        //if (Random.Range(0, 2) == 0) {
            //Note = Note + (Note > 3 ? -2 : 2);
        //}
        Note = Note + (Note > 3 ? -4 : 0);
        CharacterSpeaker.Stop();
        CharacterSpeaker.PlayOneShot(VoiceSounds[Note]);
    }
    public void PlayMiss() {
        CharacterSpeaker.Stop();
        CharacterSpeaker.PlayOneShot(MissSounds[Random.Range(0, MissSounds.Length)]);
    }
}
