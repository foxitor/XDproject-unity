using System.Collections; using System.Collections.Generic; using UnityEngine;

public class RadarIcon : MonoBehaviour {
    SpriteRenderer IconSprite; public float FadeOutSpeed = 1;
    Color DefaultColor; float CurAlpha = 0f;

    void Start() {
        IconSprite = this.gameObject.GetComponent<SpriteRenderer>();
        DefaultColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }
    void Update() {
        IconSprite.color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, CurAlpha);
        if (CurAlpha > 0) {
            CurAlpha = CurAlpha - FadeOutSpeed * Time.deltaTime;
        }
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("RadarArrow")) {
            CurAlpha = 1;
        }
    }
}
