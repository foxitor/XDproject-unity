using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum TickTypes {
    volume_every, // every tick
    volume_common, // every 4th
    volume_uncomon, // every 8th
    volume_rare, // every 10th
    volume_second // every 20th
}

public class RRTicking : MonoBehaviour {
    public int CurrentTick;
    float TickTime;
    void Update() {
        CurrentTick++;
    }
}
