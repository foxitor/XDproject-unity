using UnityEngine;

public class TrailScale : MonoBehaviour {
    public TrailRenderer Target; public float Size;

    void Update() {
        Target.widthMultiplier = Size;
    }
}
