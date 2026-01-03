using System.Collections; using System.Collections.Generic; using UnityEngine;
public enum TulevoObjectTypes {
    InnerPortal, OuterPortal,
    Saw, Spike,
    Interactive,
    Orb,
    ReversePortal,
    GravityOrb, GravityPortal,
    Item
}
public class TulevoObjectModifier : MonoBehaviour {
    public TulevoObjectTypes ObjectType;
    public Transform PortalLink;
    public bool AddsLayer;
    public string CustomAlghoritm;
    TulevoGame Game;
    void Start() {
        Game = GameObject.Find("Game").GetComponent<TulevoGame>();
    }
    void Update() {
        if (ObjectType == TulevoObjectTypes.Saw) {
            transform.Rotate(0, 0, -180 * Time.deltaTime);
        }
    } 
    
    public void OpenBox() {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        Game.SpawnBoxLoot(transform.position, transform);
    } public void RestoreBox() {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        if (transform.childCount > 1) {
            Destroy(transform.GetChild(1).gameObject);
        }
    }
}
