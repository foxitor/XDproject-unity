using UnityEngine; using System.Collections; using UnityEngine.UI; using UnityEngine.SceneManagement; using UnityEngine.Video;
using System.Collections.Generic;

public class TulevoGame : MonoBehaviour {
    //#Difficulty
    public float DifficultyPowerMultiplier;
    string Tajkost;
    public int CurRevolverAmmo, RevolverAmmoGoal;
    //#Season
    [Space]
    public TulevoSeasonalFeatures SeasonManage;
    //#Game
    [Space]
    public ShigimaJumpalka Player; public bool PlayerDied;
    public AudioSource Music;
    public AudioClip[] RandomMusics;
    public Text StatText;
    public GameObject startGuide, ReadyToLeave, DeadMessange;
    float CurTime, PopupTextAlpha;
    public TulevoGameAttributes Attributaje;
    public int layersSearched = 0;
    bool ended = false, downgradesComplited = false;
    //#Loots
    [Space]
    public GameObject Bullet;
    public Transform BulletContainer;
    public Transform[] BulletSpawns; 
    public TulevoObjectModifier[] Boxes;
    public GameObject[] BoxLoot;

    void Start() {
        startGuide.SetActive(true);
        StartCoroutine(HideGuide());
        SeasonManage.DefineSeason();
        Music.Stop(); Music.clip = RandomMusics[Random.Range(0, RandomMusics.Length)]; Music.Play();
    }

    public void SetupConfiguredDifficulty(float Power, int BulletQuota, string DisplayText) {
        DifficultyPowerMultiplier = Power; Tajkost = DisplayText; RevolverAmmoGoal = BulletQuota;
        Debug.Log(Tajkost);
    }
    public int ShareSearchedLayer() {
        return layersSearched;
    } public void AddLayer() {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in BulletContainer) { children.Add(child.gameObject); }
        foreach (GameObject child in children) { Destroy(child); }
        layersSearched++;
        Player.cheerUp();
        foreach (TulevoObjectModifier Box in Boxes) {
            Box.RestoreBox();
        }
        GameObject SpawnedBullet = Instantiate(Bullet, BulletSpawns[Random.Range(0, BulletSpawns.Length)].position, Quaternion.identity, BulletContainer);
    }
    public void SpawnBoxLoot(Vector3 SpawnPosition, Transform boxTransform) {
        if (Random.Range(0,2) == 1) {
            Instantiate(BoxLoot[Random.Range(0, BoxLoot.Length)], SpawnPosition, Quaternion.identity, boxTransform);
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("NewGeometryTulevo");
        }
        StatText.text = (" Патронов найдено : " + CurRevolverAmmo + "/" + RevolverAmmoGoal + "\n Слоёв обыскано : " + layersSearched); 

        if (CurRevolverAmmo >= RevolverAmmoGoal && !PlayerDied) { ReadyToLeave.SetActive(true); } else { ReadyToLeave.SetActive(false); }
        if (Input.GetKeyDown(KeyCode.Return) && CurRevolverAmmo >= RevolverAmmoGoal && !ended && !PlayerDied) {
            Attributaje.EndSession(); ended = true;
            Player.DoMove = false;
        } else if (Input.GetKeyDown(KeyCode.Return) && CurRevolverAmmo >= RevolverAmmoGoal && ended) {
            SceneManager.LoadScene("NewGeometryTulevo");
        } 
        
        if (Input.GetKeyDown(KeyCode.F)) {
            Music.Stop();
            Music.clip = RandomMusics[Random.Range(0, RandomMusics.Length)];
            Music.Play();
        }
    }
    IEnumerator HideGuide() {
        yield return new WaitForSeconds(3); startGuide.SetActive(false);
    } public void RestartScene() {
        if (!ended) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
