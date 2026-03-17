using System.Collections; using System.Collections.Generic; using UnityEngine;

public class NutSpore : MonoBehaviour {
    public GameObject Nut; 
    void Start() {
        StartCoroutine(HatchNut());
    }
    public IEnumerator HatchNut() {
        yield return new WaitForSeconds(20);
        Instantiate(Nut, transform.position, transform.rotation, transform.parent);
        //yield return new WaitForSeconds(2);
        //Instantiate(Nut, transform.position, transform.rotation, transform.parent);
        Destroy(this.gameObject);
    }
}
