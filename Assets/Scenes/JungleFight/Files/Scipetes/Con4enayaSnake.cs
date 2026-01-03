using UnityEngine;

public class Con4enayaSnake : MonoBehaviour {
    Transform Player, Camp, Target; EgorGameManage GameSetting; float Speed;
    void Start() {
        GameSetting = GameObject.Find("Fight").GetComponent<EgorGameManage>();
        Player = GameObject.Find("Fight/Player").transform;
        Camp = GameObject.Find("Fight/Camp").transform;
        Speed = (GameSetting.MusicState + 1) * GameSetting.DifPower;
    } void Update() {
        if (!GameSetting.SnakesOn) { Speed = 0; }
        UpdateTarget(); Vector3 thispos = transform.position; 
        Vector3 newPos = Vector3.MoveTowards(thispos, Target.position, Speed * Time.deltaTime);
        transform.position = newPos;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Kill")) {
            GameSetting.Kills++; Destroy(this.gameObject);
        } if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerProtection>().Damage(); Destroy(this.gameObject);
        } if (collision.gameObject.CompareTag("Respawn")) {
            collision.gameObject.GetComponent<SniffersBase>().DamageCamp(); Destroy(this.gameObject);
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
