using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum Blocks {
    gray_rock,
    green_rock,
    black_rock
}

public class RRBlockLibrary : MonoBehaviour {
    public Mesh[] BlockMeshes;
    public Material[] BlockMaterials;
    public GameObject BlockBreakObj, BreakSoundObj;
    public Material[] BlockDamageScales;

    public void TickNotify(TickTypes tick) {
        foreach (Transform StoredChunk in transform) {
            foreach (Transform StoredBlock in StoredChunk) {
                RRBlock BlockFunctionality = StoredBlock.GetComponent<RRBlock>();
                BlockFunctionality.onTick(tick);
            }
        }
    }
    public AudioClip GetSoundBlockTypeSound(Blocks blockType, string blockSoundType) {
        AudioClip toReturn = null;
        toReturn = GameObject.Find("SoundLib").GetComponent<RRSoundLib>().LoadBlockTypeSound(blockType, blockSoundType);
        return toReturn;
    }
}