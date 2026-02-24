using System.Collections; using System.Collections.Generic; using UnityEngine; using System;

public enum TickTypes {
    volume_every, // every tick
    volume_common, // every 4th
    volume_uncomon, // every 8th
    volume_rare, // every 10th
    volume_second, // every 20th
    volume_ten_second
}
[Serializable]
public class TickSubscriber {
    public GameObject LibraryLocation;
    public string ScriptSubType;
}
public class RRTicking : MonoBehaviour {
    public int CurrentTick;

    [Space]
    public TickSubscriber[] Subscribed;

    float TickTime; 
    const float TickingInterval = 0.05f;//1/20; //0.05s

    int FrameCount = 0;
    float FpsTimer = 0f, currentFps = 0f;

    void Update() {
        TickTime += 1 * Time.deltaTime;
        
        if (TickTime >= TickingInterval) {
            TickTime = 0f;
            CurrentTick++;
            ManageTickFunc();
        }
        //FPS
        FrameCount++;
        FpsTimer += Time.deltaTime;
        if (FpsTimer > 1) {
            currentFps = FrameCount / FpsTimer;
            FpsTimer = 0; FrameCount = 0;
        }
    }
    void ManageTickFunc() {
        SendToSubs(TickTypes.volume_every);
        if (CurrentTick % 4 == 0) { SendToSubs(TickTypes.volume_common); }
        if (CurrentTick % 8 == 0) { SendToSubs(TickTypes.volume_uncomon); }
        if (CurrentTick % 10 == 0) { SendToSubs(TickTypes.volume_rare); }
        if (CurrentTick % 20 == 0) { SendToSubs(TickTypes.volume_second); }
        if (CurrentTick % 200 == 0) { SendToSubs(TickTypes.volume_ten_second); }
    }
    void SendToSubs(TickTypes tickTpye) {
        foreach (TickSubscriber sub in Subscribed) {
            switch (sub.ScriptSubType) {
                case "Block": sub.LibraryLocation.GetComponent<RRBlockLibrary>().TickNotify(tickTpye); break;
            }
        }
    }
    public int GetFps() { return Mathf.RoundToInt(currentFps); }
}
