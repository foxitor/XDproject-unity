using UnityEngine; using UnityEngine.UI; using System.Collections;

public class GDGameManager : MonoBehaviour {
    public Text ScoreText;
    public int LayersCount; public GameObject[] Boxes;
    public BulletSpawn[] BulletsSummoners; public GameObject[] SnowParticles;
    void Start() {
        foreach(GameObject snowie in SnowParticles) {
            snowie.SetActive(IsWinter());
        } foreach (GameObject Box in Boxes) {
            Box.GetComponent<Box>().ForceWinter();
        }
    }
    public void PlusLayer() {
        LayersCount++; foreach(GameObject Box in Boxes) { Box.GetComponent<Box>().Restore(); }
        foreach (BulletSpawn bulletSummon in BulletsSummoners) { bulletSummon.SummonBullets(); }
    }
    void Update() {
        ScoreText.text = "Слоёв Обысканно : " + LayersCount + "\nПатроны : " + 
        GameObject.Find("Section01/Player").GetComponent<Player>().MagzineBullets + "/7";
    }
    bool IsWinter() {
        if (PlayerPrefs.HasKey("LastSeason")) {
            int CurSeason = PlayerPrefs.GetInt("LastSeason");bool winterBool = false;
            if (CurSeason == 1) { winterBool = true; } return winterBool;
        } else { return false; }
    }
}
