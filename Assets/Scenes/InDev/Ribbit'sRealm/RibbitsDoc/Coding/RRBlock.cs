using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.Rendering;

public class RRBlock : MonoBehaviour {
    public Blocks BlockType;
    Blocks PreviousBlockType;

    Renderer blockRender;

    RRParticularLib Particles;
    RRBlockLibrary BlockLib;

    void Start() {
        Particles = GameObject.Find("ParticularLib").GetComponent<RRParticularLib>();
        BlockLib = GameObject.Find("BlockLib").GetComponent<RRBlockLibrary>();

        PreviousBlockType = BlockType;
        blockRender = this.gameObject.GetComponent<Renderer>();
    }

    public void onTick(TickTypes tickTpye) {
        switch (BlockType) {
            case Blocks.green_rock :
                if (tickTpye == TickTypes.volume_second) {
                    Instantiate(Particles.ShareParticle("green_rock_spark"), transform.position, transform.rotation, transform);
                } break;
        }
        if (tickTpye == TickTypes.volume_every) {
            CheckBlockState(BlockType);
        }
    }
    void CheckBlockState(Blocks blockType) {
        if (PreviousBlockType != BlockType) {
            RewriteBlock(BlockType);
            PreviousBlockType = BlockType;
        }

        if (blockRender.isVisible) {
            blockRender.shadowCastingMode = ShadowCastingMode.On;
            blockRender.receiveShadows = true;
            //BlockType = Blocks.gray_rock;
        } else {
            blockRender.shadowCastingMode = ShadowCastingMode.Off;
            blockRender.receiveShadows = false;
            
            //BlockType = Blocks.black_rock;
        }
    }
    void RewriteBlock(Blocks newBlockType) {
        switch (newBlockType) {
            case Blocks.gray_rock : 
                ShapeObject("Cuboid");
                TextureBlock(0);
            break;
            case Blocks.green_rock : 
                ShapeObject("Cuboid");
                TextureBlock(1);
            break;
            case Blocks.black_rock : 
                ShapeObject("Cuboid");
                TextureBlock(2);
            break;
        }
    }
    void ShapeObject(string Type) {
        if (Type == "Cuboid") {
            this.gameObject.GetComponent<MeshFilter>().mesh = BlockLib.BlockMeshes[0];
        }
    }
    void TextureBlock(int TexturalIndex) { blockRender.material = BlockLib.BlockMaterials[TexturalIndex]; }
}