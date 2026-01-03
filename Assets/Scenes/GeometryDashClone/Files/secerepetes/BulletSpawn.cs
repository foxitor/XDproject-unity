using UnityEngine;

public class BulletSpawn : MonoBehaviour {
    public GameObject BulletObject;
    public Transform[] Positions;
    private GameObject BulletSpawned;

    public void SummonBullets() {
        if (BulletSpawned != null) { 
            Destroy(BulletSpawned); 
        }
        Transform randomPosition = Positions[Random.Range(0, Positions.Length)];
        BulletSpawned = Instantiate(BulletObject, randomPosition.position, Quaternion.identity, this.transform);
    }
}