using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.SceneManagement;

public class Zmeestradales : MonoBehaviour {
    //#Protection
    int MaxHealth = 3, CurHealth;
    public Transform ShieldPivot, Shield;

    public AudioClip[] AudioLib;

    //#Movement
    public float offset, zDistance = 10f;
    public LayerMask obstacleLayer;
    public bool Move = true; public Camera Cam;
    bool OneshotDamaged = false;

    void Start() {
        CurHealth = MaxHealth;
        this.gameObject.AddComponent<AudioSource>();
    }
    void Update() { 
        if (CurHealth < 1) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); } 
        if (Move) {
            Vector3 thispos = transform.position; Vector3 directionToGoal = Shield.position - thispos;
            RaycastHit2D hit = Physics2D.Raycast(thispos, directionToGoal.normalized, directionToGoal.magnitude, obstacleLayer);
            if (hit.collider != null) {
                if (!OneshotDamaged) {
                    StartCoroutine(CooldownedDamage());
                    OneshotDamaged = true;
                }
            } else {
                Vector3 newPos = Vector3.MoveTowards(thispos, Shield.position, 4f * Time.deltaTime);
                transform.position = newPos;
            }
            Vector3 mouseScreenPos = Input.mousePosition; mouseScreenPos.z = zDistance;
            Vector3 worldPos = Cam.ScreenToWorldPoint(mouseScreenPos);
            Vector3 difference = worldPos - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
        }
        Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
    }
    public void Damage() { CurHealth--; this.gameObject.GetComponent<AudioSource>().PlayOneShot(AudioLib[0]); }
    public int ShareHealthValue() { return CurHealth; }
    IEnumerator CooldownedDamage() {
        Damage();
        yield return new WaitForSeconds(1);
        OneshotDamaged = false;
    }
}
