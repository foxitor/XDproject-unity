using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RRParticularLib : MonoBehaviour {
    public GameObject[] Store;

    public GameObject ShareParticle(string Name) {
        int storedIndex = DecodeParticle(Name);
        return Store[storedIndex];
    }
    int DecodeParticle(string Input) {
        int result = 0;
        switch (Input) {
            case "green_rock_spark" :
                result = 1;
            break;
        }
        return result;
    }
}
