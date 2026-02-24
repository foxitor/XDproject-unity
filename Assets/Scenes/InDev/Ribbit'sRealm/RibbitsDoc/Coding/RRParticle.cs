using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RRParticle : MonoBehaviour {
    Camera TargetCamera;

    [Header("=Setting=")]
    public float LiveTime = 0.6f;
    public float Speed = 1f;
    public Vector2 MoveVectors;
    public int ParticleImportance = 3;

    //
    GameObject Visual;
    Vector3 CompiledVector;
    RRTicking TickLib;

    void Start() {
        Visual = transform.GetChild(0).gameObject;
        TickLib = GameObject.Find("Tick").GetComponent<RRTicking>();
        TargetCamera = Camera.main;
        Destroy(this.gameObject, LiveTime);

        if (ParticleImportance > 3) { return; }
        else if (ParticleImportance == 3) { int newRand = Random.Range(0, 2); if (newRand != 1) { Destroy(this.gameObject); } }
        else if ( ParticleImportance < 3) { if (TickLib.GetFps() < 40) { Destroy(this.gameObject); } }

        int DoMove1 = Random.Range(0,2); int DoMove2 = Random.Range(0,2);
        CompiledVector = new Vector3(DoMove1 == 1 ? MoveVectors.x * Random.Range(-1,1) : 0f, MoveVectors.y, DoMove2 == 1 ? MoveVectors.x * Random.Range(-1,1) : 0f);
    }
    void Update() {
        Vector3 CameraPos = TargetCamera.transform.position;
        Vector3 MyPos = transform.position;
        Vector3 Direction = CameraPos - MyPos; Direction.y = 0;

        if (Direction.sqrMagnitude < 0.001) return;

        Quaternion rot = Quaternion.LookRotation(Direction);
        transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        
        transform.position += CompiledVector * Speed * Time.deltaTime;

        if (Visual.GetComponent<Renderer>().isVisible) {
            Visual.SetActive(true);
        } else {
            Visual.SetActive(false);
        }
    }
}
