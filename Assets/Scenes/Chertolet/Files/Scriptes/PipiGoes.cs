using UnityEngine;

public class PipiGoes : MonoBehaviour {
    public float speed = 2f, speedResult; public bool speen;
    
    void Update() {
        speedResult = speed * GameObject.Find("GameController").GetComponent<GamManag>().DifficultyPowerMultiplier;
        transform.position += Vector3.left * speedResult * Time.deltaTime;
        if (transform.position.x < -20f) { Destroy(gameObject); }
        if (speen == true) { transform.Rotate(0, 0, speedResult * 10 * Time.deltaTime); }
    }
}
