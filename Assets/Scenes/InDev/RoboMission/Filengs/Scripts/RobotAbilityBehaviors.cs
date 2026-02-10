using System.Collections; using System.Collections.Generic; using UnityEngine;
public enum RMabilities {
    ShockBoom
}

public class RobotAbilityBehaviors : MonoBehaviour {
    public RMabilities AbilityInstance;
    public float Power;
    float liveTime;

    void Update() {
        if (Power > 0) {
            liveTime += Power*2 * Time.deltaTime;
            if (AbilityInstance == RMabilities.ShockBoom){
                transform.localScale = new Vector3(liveTime, liveTime, 1);
                if (liveTime >= Power*2) { Destroy(this.gameObject); }
                Color MyColor = this.GetComponent<SpriteRenderer>().color;
                this.GetComponent<SpriteRenderer>().color = new Color(MyColor.r, MyColor.g, MyColor.b, Mathf.Lerp(1f, 0f, (liveTime/2)/Power));
            }
        }
    }
}
