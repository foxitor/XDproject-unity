using System.Collections; using System.Collections.Generic; using UnityEngine;

public enum Blocks {
    gray_rock,
    green_rock
}

public class RRBlock : MonoBehaviour {
    public Blocks BlockType;
    RRParticularLib Particles;

    void Start() {
        Particles = GameObject.Find("ParticularLib").GetComponent<RRParticularLib>();
    }

    void onTick(TickTypes tickTpye) {
        switch (BlockType) {
            case Blocks.gray_rock :
                if(tickTpye == TickTypes.volume_common) {
                    Instantiate(Particles.ShareParticle("green_rock_spark"), transform.position, transform.rotation, transform);
                }
                break;
        }
    }
}
