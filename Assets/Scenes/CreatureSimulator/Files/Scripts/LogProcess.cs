using UnityEngine; using UnityEngine.UI; using System.Collections.Generic; using System;

public class LiveGraph : MonoBehaviour {    
    public RawImage displayImage; private Texture2D texture; public GameTool Game;
    public int textureWidth = 800; public int textureHeight = 400;

    private List<float> timeData = new List<float>(); private List<int> cookieData = new List<int>();
    private List<int> nutData = new List<int>();

    private float timer = 0f; public float recordInterval = 1f;

    void Start() {
        texture = new Texture2D(textureWidth, textureHeight); displayImage.texture = texture; ClearTexture();
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= recordInterval) {
            timer = 0f; float currentTime = Time.time;
            int totalCookies = GetTotalCookies(); int totalNuts = GetTotalNuts();
            timeData.Add(currentTime); cookieData.Add(totalCookies); nutData.Add(totalNuts); DrawGraph();
        }
    }
    int GetTotalCookies() { return Game.CookiesShelf.transform.childCount; }
    int GetTotalNuts() {
        int count = 0;
        GameObject Nuts = GameObject.Find("Creatures/Nuts");
        foreach (Transform creature in Nuts.transform) {
            Nut NutComponent = creature.GetComponent<Nut>();
            if (NutComponent != null) { count++; }
        } return count;
    }

    void ClearTexture() {
        Color[] fillColor = new Color[textureWidth * textureHeight];
        for (int i = 0; i < fillColor.Length; i++) fillColor[i] = Color.white;
        texture.SetPixels(fillColor); texture.Apply();
    }

    void DrawGraph() {
        ClearTexture();
        if (timeData.Count < 2) return; float minTime = timeData[0]; float maxTime = timeData[timeData.Count - 1];
        int minCookies = int.MaxValue; int maxCookies = int.MinValue; 
        int minNuts = int.MaxValue; int maxNuts = int.MinValue;
        for (int i = 0; i < cookieData.Count; i++) {
            int c = cookieData[i]; if (c < minCookies) minCookies = c; if (c > maxCookies) maxCookies = c;
            int n = nutData[i]; if (n < minNuts) minNuts = n; if (n > maxNuts) maxNuts = n;
        }
        if (maxCookies == minCookies) maxCookies++; if (maxNuts == minNuts) maxNuts++;
        DrawLine(50, 10, 50, textureHeight - 50, Color.black);
        DrawLine(50, textureHeight - 50, textureWidth - 10, textureHeight - 50, Color.black);
        int overallMax = Math.Max(maxCookies, maxNuts);
        for (int i = 1; i < timeData.Count; i++) {
            Vector2 p1 = MapToTextureCoords(timeData[i - 1], cookieData[i - 1], minTime, maxTime, 0, overallMax);
            Vector2 p2 = MapToTextureCoords(timeData[i], cookieData[i], minTime, maxTime, 0, overallMax);
            DrawLine((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, Color.blue);
        }
        for (int i = 1; i < timeData.Count; i++) {
            Vector2 p1 = MapToTextureCoords(timeData[i - 1], nutData[i - 1], minTime, maxTime, 0, overallMax);
            Vector2 p2 = MapToTextureCoords(timeData[i], nutData[i], minTime, maxTime, 0, overallMax);
            DrawLine((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, Color.red);
        } texture.Apply();
    }

    Vector2 MapToTextureCoords(float time, int value, float minTime, float maxTime, float minVal, float maxVal) {
        int marginLeft = 50; int marginBottom = 50; float xNormalized = (time - minTime) / (maxTime - minTime);
        float yNormalized = (value - minVal) / (maxVal - minVal); 
        float x = marginLeft + xNormalized * (textureWidth - marginLeft - 10); 
        float y = (textureHeight - marginBottom) - yNormalized * (textureHeight - marginBottom - 10); 
        return new Vector2(x, y);
    }

    void DrawLine(int x0, int y0, int x1, int y1, Color col) {
        int dx = Mathf.Abs(x1 - x0); int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1; int sy = (y0 < y1) ? 1 : -1; int err = dx - dy;
        while (true) {
            if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height) texture.SetPixel(x0, y0, col);
            if (x0 == x1 && y0 == y1) break; int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; } if (e2 < dx) { err += dx; y0 += sy; }
        }
    }
}