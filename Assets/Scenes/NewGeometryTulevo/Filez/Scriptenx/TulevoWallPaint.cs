using UnityEngine;
public enum TulevoPaintEvents {
    Start,
    Trigger,
    DashAbility,
    OnetimeDashAbility,
    OnetimeTrigger
}

public class TulevoWallPaint : MonoBehaviour {
    public TulevoPaintEvents SpawnEvent;
    float PaintingAlpha, FuelUpSpeed;
    bool Active = false, FadeOutActive = false, OneShotActived = false;
    TulevoGame Game;

    void Start() {
        Game = GameObject.Find("Game").GetComponent<TulevoGame>();
        if (SpawnEvent == TulevoPaintEvents.Start) {
            PaintingAlpha = 0f; FuelUpSpeed = 0.25f;
            Active = true;
        }
        if (SpawnEvent == TulevoPaintEvents.Trigger) {
            PaintingAlpha = 0f; FuelUpSpeed = 0.5f;
        }
        if (SpawnEvent == TulevoPaintEvents.DashAbility) {
            PaintingAlpha = 0f; FuelUpSpeed = 0.5f;
        } if (SpawnEvent == TulevoPaintEvents.OnetimeDashAbility) {
            PaintingAlpha = 0f; FuelUpSpeed = 0.5f;
        } if (SpawnEvent == TulevoPaintEvents.OnetimeTrigger) {
            PaintingAlpha = 0f; FuelUpSpeed = 0.75f;
        }
    }
    void Update() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            Color color = sr.color;
            color.a = PaintingAlpha;
            sr.color = color;
        }

        if (Active) {
            PaintingAlpha += FuelUpSpeed * Time.deltaTime;
            if (PaintingAlpha > 1f) PaintingAlpha = 1f;
        }

        if (FadeOutActive) {
            PaintingAlpha -= FuelUpSpeed * Time.deltaTime;
            if (PaintingAlpha < 0f) PaintingAlpha = 0f;
        }

        if (SpawnEvent == TulevoPaintEvents.DashAbility) {
            if (Game != null && Game.ShareSearchedLayer() > 4) {
                Active = true;
            }
        } 
        
        if (SpawnEvent == TulevoPaintEvents.OnetimeDashAbility && !OneShotActived) {
            if (Game != null && Game.ShareSearchedLayer() > 4 && Game.ShareSearchedLayer() < 6) {
                
                Active = true; OneShotActived = true;
            } if (Game.ShareSearchedLayer() > 5) {
                Active = false; FadeOutActive = true;
            }
        } if (SpawnEvent == TulevoPaintEvents.Start) {
            if (Game != null && Game.ShareSearchedLayer() > 0) {
                Active = false; FadeOutActive = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (SpawnEvent == TulevoPaintEvents.Trigger) {
            if (collision.gameObject.CompareTag("Player")) { 
                Active = true;
            }
        }
        if (SpawnEvent == TulevoPaintEvents.OnetimeTrigger) {
            if (collision.gameObject.CompareTag("Player") && !OneShotActived) { 
                Active = true; OneShotActived = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (SpawnEvent == TulevoPaintEvents.OnetimeTrigger) {
            if (collision.gameObject.CompareTag("Player")) { 
                Active = false; FadeOutActive = true;
            }
        }
    }
}
