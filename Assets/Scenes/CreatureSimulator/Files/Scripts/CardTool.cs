using UnityEngine;

public class CardTool : MonoBehaviour {
    public GameObject SpawnObject; public int Price;
    public BoxCollider2D targetCollider;

    public void Click() {
        GameTool GameBrain = GameObject.Find("Main Camera").GetComponent<GameTool>();
        if (GameBrain.Goobles >= Price) {
            Vector3 randomPoint = GetRandomPointOnCollider(targetCollider);
            Instantiate(SpawnObject, randomPoint, Quaternion.identity, GameObject.Find("GameObjects").GetComponent<Transform>());
            GameBrain.Goobles = GameBrain.Goobles - Price;
        }
    }

    Vector3 GetRandomPointOnCollider(BoxCollider2D col) {
        Bounds bounds = col.bounds;

        Vector3 point;
        int attempts = 0;
        const int maxAttempts = 10;

        do {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            point = new Vector3(x, y, z);

            attempts++;
        } while (!IsPointOnSurfaceOrInside(col, point) && attempts < maxAttempts);

        if (attempts >= maxAttempts) {
            Debug.LogWarning("IDK where to place this shit, dog.");
        }

        return point;
    }

    bool IsPointOnSurfaceOrInside(BoxCollider2D col, Vector3 point) {
        Vector3 closestPoint = col.ClosestPoint(point);
        float distance = Vector3.Distance(closestPoint, point);
        return distance < 0.01f;
    }
}