using UnityEngine; using UnityEngine.UI; using System.Collections; using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameTool : MonoBehaviour {
    public int Speed = 1; public Button SpeedButton; public Text SpeedText;
    public GameObject GameObjects, Nuke, ExplosionFlash, BeepFlash, Statistics;
    public AudioSource AudioOrigin; public AudioClip ExplosionClip, ExplosionScream, NukeFallSound;
    public float shakeDuration = 1f; public float shakeMagnitude = 0.1f; private Vector3 originalPosition;
    public int Goobles; public GameObject CookiesShelf, NutShelf;
    public bool DialogueActive;
    public Text ScoreTxt; public PostProcessProfile Realistic, Default;
    public bool curVision, StatisticsShowing;
    [Header("Rain & Fog")]
    public GameObject Rain; public GameObject Fog; public bool Rainy;
    public BoxCollider2D GameCollider; public GameObject RainPuddle, SporeSeed;
    private int CookieScore = 0; private int recordScore = 0;
    bool BeepActivated;

    void Start() {
        Speed = 1; SpeedText.text = Speed + "X"; Time.timeScale = Speed;
        originalPosition = transform.localPosition;
        recordScore = PlayerPrefs.GetInt("CookiesRecord", 0);
    } void Update() {
        //if(GameObject.Find("Creatures/Nuts").transform.childCount == 0) {
        //    Instantiate(SporeSeed, new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Creatures/Nuts").transform);
        //}
        CookieScore = CookiesShelf.transform.childCount;
        ScoreTxt.text = "Cookies : " + CookieScore + "\nRecord : " + recordScore;
    } public void OnSpeedButton() {
        Speed++; if (Speed > 5) { Speed = 0; }
        SpeedText.text = Speed + "X";
        Time.timeScale = Speed;
    } public void Clear() {
        if (GameObjects == null) return; var target = GameObjects.transform;
        var children = new Transform[target.childCount];
        for (int i = 0; i < target.childCount; i++) children[i] = target.GetChild(i);
        foreach (var child in children) Destroy(child.gameObject);
    } public void SendNuke() {
        StartCoroutine(Nukefall()); StartCoroutine(Shake()); StartCoroutine(FlashingRed());
    } public IEnumerator Nukefall() { 
        yield return new WaitForSeconds(0.05f);

        var Ctarget = CookiesShelf.transform;
        var Cchildren = new Transform[Ctarget.childCount];
        for (int i = 0; i < Ctarget.childCount; i++) Cchildren[i] = Ctarget.GetChild(i);
        foreach (var child in Cchildren) { child.GetComponent<CookieBrain>().Brain = false; 
        child.GetComponent<SpriteRenderer>().sprite = child.GetComponent<CookieBrain>().SpriteLib[child.GetComponent<CookieBrain>().ParentGen + 2]; }

        var Ntarget = NutShelf.transform;
        var Nchildren = new Transform[Ntarget.childCount];
        for (int i = 0; i < Ntarget.childCount; i++) Nchildren[i] = Ntarget.GetChild(i);
        foreach (var child in Nchildren) { child.GetComponent<Nut>().AI = false; 
        child.GetComponent<SpriteRenderer>().sprite = child.GetComponent<Nut>().SpriteLib[3]; }

        Instantiate(Nuke, transform.position, Quaternion.identity);
        AudioOrigin.pitch = Speed;
        AudioOrigin.PlayOneShot(NukeFallSound);
        yield return new WaitForSeconds(1.2f);
        AudioOrigin.PlayOneShot(ExplosionClip);
        ExplosionFlash.SetActive(true);
        if (CookiesShelf != null) {
            foreach (Transform child in CookiesShelf.transform) {
                var cookieroAI = child.GetComponent<CookieroAI>();
                if (cookieroAI != null) {
                    var audioSource = cookieroAI.GetComponent<AudioSource>();
                    if (audioSource != null && ExplosionScream != null) {
                        audioSource.PlayOneShot(ExplosionScream);
                    }
                }
                var CookieBrain = child.GetComponent<CookieBrain>();
                if (CookieBrain != null) {
                    var audioSource = CookieBrain.GetComponent<AudioSource>();
                    if (audioSource != null && ExplosionScream != null) {
                        audioSource.PlayOneShot(ExplosionScream);
                    }
                }
            }
        }

        yield return new WaitForSeconds(5f); 
        SaveRecord();
        SceneManager.LoadScene("CreatureSimulator");
    } private IEnumerator FlashingRed() {
        yield return new WaitForSeconds(0.1f); BeepActivated = !BeepActivated; BeepFlash.SetActive(BeepActivated);
        StartCoroutine(FlashingRed());
    }
    private IEnumerator Shake() {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration) {
            float x = Random.Range(-1f, 1f) * shakeMagnitude; float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.localPosition = originalPosition + new Vector3(x, y, 0); elapsed += Time.deltaTime; 
            yield return null;
        } 
        transform.localPosition = originalPosition;
    }
    public void ToggleRainSpawn() { if (!Rainy) { StartCoroutine(StartRain()); } else {
        StartCoroutine(EndRain());
    } }
    public IEnumerator StartRain() {
        Rainy = true; Rain.SetActive(Rainy);
        yield return new WaitForSeconds(Random.Range(1, 5));
        Fog.SetActive(Rainy);
        StartCoroutine(DropPuddle());
        if (CookiesShelf != null) { foreach (Transform child in CookiesShelf.transform) { 
            var cookieroAI = child.GetComponent<CookieroAI>();
                if (cookieroAI != null) { cookieroAI.RainyBehavior(true); }
            }
        }
    } public IEnumerator DropPuddle() {
        yield return new WaitForSeconds(Random.Range(5, 10));
        Vector3 randomPoint = GetRandomPointOnCollider(GameCollider);
        Instantiate(RainPuddle, randomPoint, Quaternion.identity, GameObject.Find("GameObjects").GetComponent<Transform>());
        if (Rainy) { StartCoroutine(DropPuddle()); }
    }
    public IEnumerator EndRain() {
        Rainy = false; Rain.SetActive(Rainy);
        yield return new WaitForSeconds(Random.Range(1, 7));
        Fog.SetActive(Rainy);
        if (CookiesShelf != null) { foreach (Transform child in CookiesShelf.transform) { 
            var cookieroAI = child.GetComponent<CookieroAI>();
                if (cookieroAI != null) { cookieroAI.RainyBehavior(false); }
            }
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
    void SaveRecord() {
        if (CookieScore > recordScore) {
            recordScore = CookieScore;
            PlayerPrefs.SetInt("CookiesRecord", recordScore);
            PlayerPrefs.Save();
        }
    } void OnDisable() { SaveRecord(); }
    public void VisionChange() {
        curVision = !curVision;
        if (curVision) {
            this.gameObject.GetComponent<PostProcessVolume>().profile = Realistic;
        } else {
            this.gameObject.GetComponent<PostProcessVolume>().profile = Default;
        }
    }
    public void SwitchStatShowing() {
        StatisticsShowing = !StatisticsShowing; Statistics.SetActive(StatisticsShowing);
    }
}