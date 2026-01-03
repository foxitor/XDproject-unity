using UnityEngine;

public class MouseHoverDetector : MonoBehaviour {
    private SpriteRenderer Vizual; public bool Editing, Sight;
    public string Method;
    void Start() { Vizual = GetComponent<SpriteRenderer>(); }

    private void OnMouseEnter() {
        Vizual.color = new Color(0.8f, 0.8f, 0.8f, 1); Sight = true;
    }

    private void OnMouseExit() {
        if (!Editing) { Vizual.color = new Color(1, 1, 1, 1); } Sight = false;
    }
    void Update() {
        Transform Geo = GetComponent<Transform>();
        if (Sight) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    Editing = !Editing;
                }
        } if (Editing) {
            GameTool GameToolSetting = GameObject.Find("Main Camera").GetComponent<GameTool>();
            Vizual.color = new Color(0.5f, 0.5f, 0.5f, 1);
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                Geo.position = new Vector3(Geo.position.x, Geo.position.y+0.5f, Geo.position.z);
            } if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                Geo.position = new Vector3(Geo.position.x, Geo.position.y-0.5f, Geo.position.z);
            } if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                Geo.position = new Vector3(Geo.position.x-0.5f, Geo.position.y, Geo.position.z);
            } if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                Geo.position = new Vector3(Geo.position.x+0.5f, Geo.position.y, Geo.position.z);
            } if (Input.GetKeyDown(KeyCode.Mouse1)) {
                Destroy(this.gameObject);
            }
            if (!GameToolSetting.DialogueActive) {
                if (Input.GetKeyDown(KeyCode.P)) {
                    this.gameObject.GetComponent<CookieroAI>().GenderInteract();
                    GameToolSetting.DialogueActive = true;
                }
            }
        } else {
            if (Sight) { Vizual.color = new Color(0.8f, 0.8f, 0.8f, 1); } 
            else { Vizual.color = new Color(1, 1, 1, 1); }
        }
    } void OnTriggerEnter2D(Collider2D Trigger) {
        if(Trigger.CompareTag("Kill")) {
            Destroy(this.gameObject);
        }
    }
}