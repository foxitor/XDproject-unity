using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.Rendering;

public class RRBlock : MonoBehaviour {
    public Blocks BlockType;
    Blocks PreviousBlockType;
    int damageState = 0, PreviousDamage = 0, ticksSinceLastHit; GameObject myBlockBreak;
    float MaxDamage = 1f;

    Renderer blockRender;

    RRParticularLib Particles;
    RRBlockLibrary BlockLib;

    void Start() {
        Particles = GameObject.Find("ParticularLib").GetComponent<RRParticularLib>();
        BlockLib = GameObject.Find("BlockLib").GetComponent<RRBlockLibrary>();
        
        blockRender = this.gameObject.GetComponent<Renderer>();

        PreviousBlockType = BlockType;
        RewriteBlock(BlockType);
    }

    public void onTick(TickTypes tickTpye) {
        switch (BlockType) {
            case Blocks.green_rock :
                if (blockRender.isVisible) {
                    if (tickTpye == TickTypes.volume_rare) {
                        SpawnParticle("green_rock_spark", "ambience", true, true, 1);
                    }
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

        ticksSinceLastHit++;

        if (PreviousDamage != damageState) {
            float compiledDamage = (float)damageState / MaxDamage;
            int damageLevel = Mathf.Clamp(Mathf.FloorToInt(compiledDamage * BlockLib.BlockDamageScales.Length), 0, BlockLib.BlockDamageScales.Length);
            ticksSinceLastHit = 0;

            if (damageState >= MaxDamage) {
                GameObject Leftover = 
                Instantiate(BlockLib.BreakSoundObj, transform.position, transform.rotation, GameObject.Find("SoundLib").transform);

                Leftover.GetComponent<AudioSource>().clip = BlockLib.GetSoundBlockTypeSound(BlockType, "Impact");
                Leftover.GetComponent<AudioSource>().Play();

                Destroy(Leftover, 1f);
                Destroy(this.gameObject);
                return;
            } else {
                if (myBlockBreak == null && damageState != 0) {
                    myBlockBreak = Instantiate(BlockLib.BlockBreakObj, transform.position, transform.rotation, transform);
                }
                if (myBlockBreak != null && damageLevel < BlockLib.BlockDamageScales.Length) {
                    myBlockBreak.GetComponent<Renderer>().material = BlockLib.BlockDamageScales[damageLevel];
                }
                if (PreviousDamage < damageState) gameObject.GetComponent<AudioSource>().PlayOneShot(BlockLib.GetSoundBlockTypeSound(BlockType, "Hit"));
            }
            PreviousDamage = damageState;
        }
        if (ticksSinceLastHit > 4) {
            damageState = 0; Destroy(myBlockBreak); myBlockBreak = null;
        }
        OptimizeBlock();
    }
    void RewriteBlock(Blocks newBlockType) {
        switch (newBlockType) {
            case Blocks.gray_rock : 
                ShapeObject("Cuboid");
                TextureBlock(0);
                MaxDamage = 4.5f;
            break;
            case Blocks.green_rock : 
                ShapeObject("Cuboid");
                TextureBlock(1);
                MaxDamage = 10;
            break;
            case Blocks.black_rock : 
                ShapeObject("Cuboid");
                TextureBlock(2);
                MaxDamage = 7;
            break;
        }
    }
    void ShapeObject(string Type) {
        if (Type == "Cuboid") {
            this.gameObject.GetComponent<MeshFilter>().mesh = BlockLib.BlockMeshes[0];
        }
    }
    void TextureBlock(int TexturalIndex) { blockRender.material = BlockLib.BlockMaterials[TexturalIndex]; }

    public void Punch(int Damage) {
        damageState += Damage;
    }
    void OptimizeBlock() {
        if (blockRender != null) {
            if (blockRender.isVisible) {
                blockRender.shadowCastingMode = ShadowCastingMode.On; blockRender.receiveShadows = true;
            } else {
                blockRender.shadowCastingMode = ShadowCastingMode.Off; blockRender.receiveShadows = false;
            }
        }
    }
    void SpawnParticle(string particleName, string spawnReason, bool DoOffset, bool DoFullOffset, int count) {
        Vector3 RandomizedPosition = new Vector3(
            DoFullOffset ? transform.position.x + Random.Range(-0.5f, 0.5f) : transform.position.x + 0,
            DoOffset ? transform.position.y + Random.Range(-0.5f, 0.5f) : transform.position.y + 0,
            DoFullOffset ? transform.position.z + Random.Range(-0.5f, 0.5f) : transform.position.z + 0
        );
        Instantiate(Particles.ShareParticle(particleName), RandomizedPosition, transform.rotation, transform);
    }
}