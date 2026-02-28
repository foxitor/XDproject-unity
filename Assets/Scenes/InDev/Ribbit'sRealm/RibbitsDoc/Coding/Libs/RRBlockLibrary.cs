using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum Blocks {
    gray_rock,
    green_rock,
    black_rock,
    cobblestone
}
public enum RRBlockSubTypes {
    Cube,
    Stair,
    Slab
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
    //DURABILITY
    float GetBlockDurabily(Blocks blockType) {
        float toReturn = 0;
        switch (blockType) {
            case Blocks.gray_rock : toReturn =  4.5f; break;
            case Blocks.black_rock : toReturn =  7f; break;
            case Blocks.green_rock : toReturn = 10f; break;
            //
            case Blocks.cobblestone : toReturn =  5.5f; break;
        } return toReturn;
    }
    //TEXTURE
    int GetBlockTexture(Blocks blockType) {
        int toReturn = 0;
        switch (blockType) {
            case Blocks.gray_rock : toReturn = 0; break;
            case Blocks.black_rock : toReturn = 2; break;
            case Blocks.green_rock : toReturn = 1; break;
            //
            case Blocks.cobblestone : toReturn = 3; break;
        } return toReturn;
    }
    //SUB-SHAPE
    RRBlockSubTypes GetBlockSubType(Blocks block) {
        RRBlockSubTypes toReturn = RRBlockSubTypes.Cube;
        switch (block) {
            case Blocks.gray_rock: toReturn = RRBlockSubTypes.Cube; break;
            case Blocks.black_rock: toReturn = RRBlockSubTypes.Cube; break;
            case Blocks.green_rock: toReturn = RRBlockSubTypes.Cube; break;
            //
            case Blocks.cobblestone: toReturn = RRBlockSubTypes.Cube; break;
        }
        return toReturn;
    }
    //MODEL
    int GetBlockModel(Blocks blockType) {
        int toReturn = 0;
        if (GetBlockSubType(blockType) == RRBlockSubTypes.Slab) {
            toReturn = 1;
        }
        return toReturn;
    }


    public float GetBlockFloatData(Blocks block, string Path) {
        float DataPackage = -1;
        switch (Path) {
            case "Durability" : DataPackage = GetBlockDurabily(block); break;
        }

        if (DataPackage == -1) Debug.Log("DataPackage Error \n at Path: " + Path);
        return DataPackage;
    }
    public int GetBlockIntData(Blocks block, string Path) {
        int DataPackage = -1;
        switch (Path) {
            case "Texture" : DataPackage = GetBlockTexture(block); break;
            case "Model" :  DataPackage=  GetBlockModel(block); break;
        }

        if (DataPackage == -1) Debug.Log("DataPackage Error \n at Path: " + Path);
        return DataPackage;
    }
}