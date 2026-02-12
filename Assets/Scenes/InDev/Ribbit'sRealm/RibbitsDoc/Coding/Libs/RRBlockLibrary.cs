using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum Blocks {
    gray_rock,
    green_rock
}

public class RRBlockLibrary : MonoBehaviour {
    public Mesh[] BlockMeshes;
    public Material[] BlockMaterials;
    public void TickNotify(TickTypes tick) {
        foreach (Transform StoredBlock in transform) {
            RRBlock BlockFunctionality = StoredBlock.GetComponent<RRBlock>();
            BlockFunctionality.onTick(tick);
        }
    }
}
