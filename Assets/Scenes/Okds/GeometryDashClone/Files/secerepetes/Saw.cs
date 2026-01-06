using UnityEngine;

public class Saw : MonoBehaviour {
    public GameObject[] RotationObjects;
    private float rotationSpeed = -150f;

    void Update() {
        foreach(GameObject RotationObj in RotationObjects) {
            Vector3 rotate = new Vector3(0f, 0f, rotationSpeed * Time.deltaTime);
            RotationObj.transform.Rotate(rotate);
        }
    }
}