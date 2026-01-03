using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentalData : MonoBehaviour {
    public AudioClip[] Instruments;
    public string Type;
    AudioSource Speaker;

    void Start() {
        Speaker = this.gameObject.GetComponent<AudioSource>();
    }

    public void Play(int instrumentIndex) {
        Speaker.PlayOneShot(Instruments[instrumentIndex]);
    }
}
