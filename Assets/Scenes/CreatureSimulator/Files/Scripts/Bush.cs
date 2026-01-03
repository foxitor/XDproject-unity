using UnityEngine; using System.Collections;

public class Bush : MonoBehaviour {
    public Sprite YoungSprite; public Sprite GrowingSprite; public Sprite GrownSprite;
    private SpriteRenderer spriteRenderer; public int Stage;

    public float growthTime = 5f;

    void Start() { spriteRenderer = GetComponent<SpriteRenderer>(); Stage = 1; StartCoroutine(Grow()); }
    public void Eat() { Stage = 1; StartCoroutine(Grow()); }

    public IEnumerator Grow() { yield return new WaitForSeconds(growthTime); 
        switch (Stage) { 
            case 1: Stage++; StartCoroutine(Grow()); break;
            case 2: Stage++; break;
        }  
    }
    void Update() { 
        switch (Stage) { 
            case 1: spriteRenderer.sprite = YoungSprite; break;
            case 2: spriteRenderer.sprite = GrowingSprite; break;
            case 3: spriteRenderer.sprite = GrownSprite; break;
        }
    }
}