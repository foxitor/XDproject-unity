using UnityEngine; using UnityEngine.Tilemaps; using DG.Tweening;

public class TerrainGeneration : MonoBehaviour {
    public Tilemap tilemap; public TileBase[] TileLib;

    public int mapWidth = 50, mapHeight = 50;
    public float noiseScale = 0.1f, waterLevel = 0.4f;

    void Start() {
        DOTween.To(() => waterLevel, x => waterLevel = x, 0.403f, 2f)
            .SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
    }

    void Update() { 
        GenerateTiles(); 
    }

    void GenerateTiles() {
        tilemap.ClearAllTiles(); 
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                float perlinValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                TileBase tileToPlace; 
                if (perlinValue < waterLevel) { tileToPlace = TileLib[13]; } else { tileToPlace = TileLib[0]; }
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if(tileToPlace != null) { tilemap.SetTile(tilePosition, tileToPlace); }
            }
        }
    }
}