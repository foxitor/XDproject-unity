using System.Collections; using System.Collections.Generic; using UnityEngine; using System;

[Serializable]
public class BlockSoundTypes {
    public Blocks block;
    public AudioClip[] Impacts;
    public AudioClip[] Hits;
    public AudioClip[] Specials;
}

public class RRSoundLib : MonoBehaviour {
    public BlockSoundTypes[] Sounds;

    public AudioClip LoadBlockTypeSound(Blocks blockType, string blockSoundType) {
        foreach (BlockSoundTypes bst in Sounds) {
            if (bst.block == blockType) {
                AudioClip[] clips = null;

                switch (blockSoundType.ToLower()) {
                    case "impact":
                    case "impacts":
                        clips = bst.Impacts;
                        break;
                    case "hit":
                    case "hits":
                            clips = bst.Hits;
                        break;

                    case "special":
                    case "specials":
                            clips = bst.Specials;
                        break;

                    default:
                        Debug.LogWarning("Unknown blockSoundType: " + blockSoundType);
                        return null;
                }

                if (clips != null && clips.Length > 0) {
                    int randomIndex = UnityEngine.Random.Range(0, clips.Length);
                    return clips[randomIndex];
                }
                else {
                    Debug.LogWarning($"No audio clips found for {blockSoundType} of block {blockType}");
                    return null;
                }
            }
        }

        Debug.LogWarning("Block type not found in sound library: " + blockType);
        return null;
    }
}
