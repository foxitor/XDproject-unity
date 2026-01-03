using UnityEngine;

public class FollowMouse : MonoBehaviour {
    public float offset, zDistance = 10f;
    public Transform Goal; public LayerMask obstacleLayer;
    public bool Move = true; public Camera Cam;

    void Update() {
        if (Move) {
            Vector3 thispos = transform.position; Vector3 directionToGoal = Goal.position - thispos;
            RaycastHit2D hit = Physics2D.Raycast(thispos, directionToGoal.normalized, directionToGoal.magnitude, obstacleLayer);
            if (hit.collider != null) {
            } else {
                Vector3 newPos = Vector3.MoveTowards(thispos, Goal.position, 4f * Time.deltaTime);
                transform.position = newPos;
            }
            Vector3 mouseScreenPos = Input.mousePosition; mouseScreenPos.z = zDistance;
            Vector3 worldPos = Cam.ScreenToWorldPoint(mouseScreenPos);
            Vector3 difference = worldPos - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
        }
    }
}