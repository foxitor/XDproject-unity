using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;

public class RobotBrain : MonoBehaviour {
    #region movement
    public float LeftEnergy, MaxEnergy; public float MovementPower = 1f;
    public GameObject MyCamera; bool isMoving = false;
    #endregion
    [Space]
    #region area
    public LayerMask Obsticales; GameObject StandingOn;
    #endregion 
    [Space]
    #region Abilities
    [SerializeField]
    private GameObject ShockWavePrefab;
    #endregion
    [Space]
    #region UI
    public Text PowerDisplay;
    #endregion

    void Start() {
        MaxEnergy = LeftEnergy;
    }
    void Update() {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(LeftEnergy/MaxEnergy,LeftEnergy/MaxEnergy,LeftEnergy/MaxEnergy,1f);
        MyCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        PowerDisplay.text = LeftEnergy * 2 + "W Left";
    }
    public void Move(string Direction) {
        Vector3 Axis = Vector3.zero;
        switch (Direction) {
            case "up": Axis = new Vector3(0, 1, 0); break;
            case "down": Axis = new Vector3(0, -1, 0); break;
            case "right": Axis = new Vector3(1, 0, 0); break;
            case "left": Axis = new Vector3(-1, 0, 0); break;
            default: return;
        }
        Debug.DrawRay(transform.position, Axis, Color.green); 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Axis, 1f, Obsticales);
        if (hit.collider == null) {
            Vector3 direction = new Vector3(Axis.x, Axis.y, 0).normalized;
            Vector3 MultipliedAxis = new Vector3(Axis.x * MovementPower, Axis.y * MovementPower, 0); 
            Vector3 MovedPosition = new Vector3(transform.position.x + MultipliedAxis.x, transform.position.y + MultipliedAxis.y, 0);
            StartCoroutine(MoveTo(MovedPosition));
            LeftEnergy -= 0.5f;
        }
    }
    public void Look(string Direction) {
        transform.rotation = new Quaternion(0,0,0,0);
        switch (Direction) {
            case "up": transform.Rotate(0,0,135); break;
            case "down": transform.Rotate(0,0,315); break;
            case "right": transform.Rotate(0,0,45); break;
            case "left": transform.Rotate(0,0,225); break;
            default: return;
        }
    }
    public void OnTriggerEnter2D(Collider2D Collider) {
        StandingOn = Collider.gameObject;
    } public void OnTriggerExit2D(Collider2D Collider) {
        StandingOn = null;
    }
    public void Charge() {
        if (DefineSurfaceID() == "Charger") {
            LeftEnergy = MaxEnergy;
        }
    }
    public void ShockWave(int Power) {
        GameObject Boom = Instantiate(ShockWavePrefab, transform.position, transform.rotation, transform.parent);
        Boom.GetComponent<RobotAbilityBehaviors>().Power = (float)Power;
        LeftEnergy -= (float)Power * 0.5f;
    }
    string DefineSurfaceID() {
        string returnText = "";
        if (StandingOn != null) {
            returnText = StandingOn.name;
        }
        return returnText;
    }
    IEnumerator MoveTo(Vector3 movePos) {
        isMoving = true;
        while (Vector3.Distance(transform.position, movePos) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, movePos, ((LeftEnergy*10)/MaxEnergy * Time.deltaTime));
            yield return null;
        }
        transform.position = movePos;
        isMoving = false;
    }
}
