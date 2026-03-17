using UnityEngine;

public class RockSpawn : MonoBehaviour {
    public GameObject pipePrefab; public GameObject Cum; public float spawnInterval = 2f;
    public float pipeMinY = -1f; public float pipeMaxY = 3f;
    public float spawnXPosition = 10f; private float timer = 0f;
    public GamManag Game;
    Color RockColor;
    void Start() {
        if (Game == null) {
            Game = this.gameObject.GetComponent<GamManag>();
        }
    }
    void Update() {
        timer += Time.deltaTime; if (timer >= spawnInterval) { SpawnPipe(); timer = 0f; }
    }
    public void SetColor(Color newColor) {
        RockColor = newColor;
    }

    void SpawnPipe() { 
        if (Game.DoPPspawn) {
            float yPosition = Random.Range(pipeMinY, pipeMaxY); 
            Vector3 spawnPosition = new Vector3(spawnXPosition, yPosition, 0);
            if (Game.CurrentEvent == "Cum") { Instantiate(Cum, spawnPosition, Quaternion.identity); }
            else {
                GameObject newRock = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
                newRock.GetComponent<SpriteRenderer>().color = RockColor;
            }
        }
    }
}
