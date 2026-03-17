using UnityEngine;

public class Snake : MonoBehaviour {
    Transform Player, Camp, Target; EgoricalGame GameSetting; float Speed;
    public AudioClip KilledSound;
    void Start() {
        GameSetting = GameObject.Find("Fight").GetComponent<EgoricalGame>();
        Player = GameObject.Find("Fight/Zmeestradales").transform;
        Camp = GameObject.Find("Fight/Base").transform;
    } void Update() {
        if (!GameSetting.SnakesOn) { Speed = 0; }
        else { Speed = (GameSetting.MusicState + 2) * GameSetting.DifficultyPowerMultiplier; }
        UpdateTarget(); Vector3 thispos = transform.position; 
        Vector3 newPos = Vector3.MoveTowards(thispos, Target.position, Speed * Time.deltaTime);
        transform.position = newPos;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Kill")) {
            GameSetting.playSnakeSound(KilledSound);
            GameSetting.Kills++; Destroy(this.gameObject);
            GameSetting.Player.VibrateController(0.15f, 0.15f, 0.05f);
        } if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Zmeestradales>().Damage(); Destroy(this.gameObject);
        } if (collision.gameObject.CompareTag("Respawn")) {
            collision.gameObject.GetComponent<EgoricalCamp>().DamageCamp(); Destroy(this.gameObject);
        }
    }
    void UpdateTarget() {
        if (Player == null || Camp == null) return;
        float distToPlayer = Vector3.Distance(transform.position, Player.position);
        float distToCamp = Vector3.Distance(transform.position, Camp.position);
        if (distToPlayer < distToCamp) {
            Target = Player;
        } else {
            Target = Camp;
        }
    }
}
