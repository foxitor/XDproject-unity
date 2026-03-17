using UnityEngine; using UnityEngine.SceneManagement; using System.Collections; using UnityEngine.UI; using UnityEngine.Video;

public class Player : MonoBehaviour {
    public float Speed = 5f; public float JumpHeight = 9f; private bool isGrounded;
    private Rigidbody2D rb; public bool SuperPowers; public bool OnPad; public float rotationSpeed = -200f;
    public Transform VizualPlayerTransform; public GameObject VizualPlayer; public GameObject ReadyToFightImg;
    public GameObject WinMessenge, DeadMessange, RankScreen; public AudioClip Dead; public AudioSource AudioOrigin;
    public bool CanMove = true, ended; public AudioSource Music; public bool ReadyToFight; public int MagzineBullets;
    public GameObject GuideImage; public Text EndingText; public string[] Endings; public bool CanOpenBox;
    public GameObject TriggeredBox; public VideoClip[] RankVideoClips; public VideoPlayer RankingVidPlayer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(HideGuide());
    }
    bool IsWinter() {
        if (PlayerPrefs.HasKey("LastSeason")) {
            int CurSeason = PlayerPrefs.GetInt("LastSeason");bool winterBool = false;
            if (CurSeason == 1) { winterBool = true; } return winterBool;
        } else { return false; }
    }
    void Update() {
        MoveForward(); Jump(); RotateObject();
        GDGameManager GameManager = GameObject.Find("Section01/GameManager").GetComponent<GDGameManager>();
        if (GameManager.LayersCount == 5 && SuperPowers == false) {
            ApplySuperPowers();
        } if (MagzineBullets >= 7) { ReadyToFight = true; } if (Input.GetKeyDown(KeyCode.F) && ReadyToFight && !ended) {
            StartCoroutine(EndingGenerator()); ReadyToFight = false;
            ended = true;
        } ReadyToFightImg.SetActive(ReadyToFight);
        if (ended && Input.GetKey(KeyCode.Return)) { SceneManager.LoadScene("GeometryDashClone"); }
        if (Input.GetKey("escape")) { SceneManager.LoadScene("GeometryDashClone"); }
    }
    public void ApplySuperPowers() {
        SuperPowers = true;
        Speed = 7f;
        VizualPlayer.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.3f, 0.3f, 1f);
    }

    void MoveForward() {
        if (CanMove) { transform.Translate(transform.right * Speed * Time.deltaTime); }
    }

    void Jump() {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && isGrounded && CanMove && !CanOpenBox) {
            rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
            isGrounded = false;
        } if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && !isGrounded && OnPad && CanMove) {
            rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
        } if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && CanOpenBox) { 
            TriggeredBox.GetComponent<Box>().Open();
        }
    }

    void RotateObject() {
        if (!isGrounded && VizualPlayerTransform != null) {
            VizualPlayerTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            SnapToNearestAngle();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Pad")) {
            OnPad = true;
        } if (collision.gameObject.CompareTag("Kill")) {
            AudioOrigin.PlayOneShot(Dead); StartCoroutine(Dying());
        } if (collision.gameObject.CompareTag("Portal")) {
            collision.gameObject.GetComponent<Portal>().Teleport();
            WinMessenge.SetActive(true);
            StartCoroutine(Wait());
        } if (collision.gameObject.CompareTag("Item")) {
            collision.GetComponent<Item>().PickUp();
        } if (collision.gameObject.CompareTag("Box")) { 
            CanOpenBox = true; TriggeredBox = collision.gameObject;
        }
    }
    IEnumerator EndingGenerator() {
        RankScreen.SetActive(true);
        VideoClip RankVideoClip;
        if (VizualPlayer.GetComponent<SpriteRenderer>().sprite.name == "PlayerDefaultSkin") { RankVideoClip = RankVideoClips[0]; } else { RankVideoClip = RankVideoClips[1]; }
        RankingVidPlayer.clip = RankVideoClip;
        int ending = Random.Range(0, Endings.Length);
        string EndingUnlockedText = ""; string NoOneCameAdv = "";
        bool EndingIsUnlocked = false; bool NoOneCameAdvUnlocked = false;
        string SeasonAdv = ""; bool SeasonAdvUnlocked = false;
        if (MagzineBullets >= 25) { if (PlayerPrefs.GetInt("NoOne") == 0 || !PlayerPrefs.HasKey("NoOne")) 
            { NoOneCameAdvUnlocked = true; } PlayerPrefs.SetInt("NoOne", 1); }
        switch(ending) {
            case 0 : if (PlayerPrefs.GetInt("Dantess") == 0 || !PlayerPrefs.HasKey("Dantess")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("Dantess", 1); break;
            case 1 : if (PlayerPrefs.GetInt("HappyEnd") == 0 || !PlayerPrefs.HasKey("HappyEnd")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("HappyEnd", 1); break;
            case 2 : if (PlayerPrefs.GetInt("NoAmmo") == 0 || !PlayerPrefs.HasKey("NoAmmo")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("NoAmmo", 1); break;
            case 3 : if (PlayerPrefs.GetInt("Palcovka") == 0 || !PlayerPrefs.HasKey("Palcovka")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("Palcovka", 1); break;
            case 4 : if (PlayerPrefs.GetInt("MyBoy") == 0 || !PlayerPrefs.HasKey("MyBoy")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("MyBoy", 1); break;
            case 5 : if (PlayerPrefs.GetInt("Weak") == 0 || !PlayerPrefs.HasKey("Weak")) 
            { EndingIsUnlocked = true; } PlayerPrefs.SetInt("Weak", 1); break;
        }
        if (IsWinter()) { if (PlayerPrefs.GetInt("IceGunFight") == 0 || !PlayerPrefs.HasKey("IceGunFight")) 
            { SeasonAdvUnlocked = true; } PlayerPrefs.SetInt("IceGunFight", 1); }
        if (EndingIsUnlocked == true) { EndingUnlockedText = "+Достижение за концовку"; }
        if (NoOneCameAdvUnlocked == true) { NoOneCameAdv = "+Достижение за патроны"; }
        if (SeasonAdvUnlocked == true) { SeasonAdv = "+Достижение за сезон"; }
        EndingText.text = "Концовка : \n" + Endings[ending] + "\n"+ EndingUnlockedText +"\n" + NoOneCameAdv + "\n" + SeasonAdv;
        CanMove = false; rb.bodyType = RigidbodyType2D.Static; Music.Stop();
        yield return new WaitForSeconds(3);
    }
    IEnumerator HideGuide() {
        yield return new WaitForSeconds(3);
        GuideImage.SetActive(false);
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(1);
        WinMessenge.SetActive(false);
    } IEnumerator Dying() {
        rb.bodyType = RigidbodyType2D.Static; Music.Stop();
        CanMove = false; DeadMessange.SetActive(true);
        yield return new WaitForSeconds(4);
        RestartScene();
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Pad")) { OnPad = false; } 
        if (collision.gameObject.CompareTag("Box")) { CanOpenBox = false; TriggeredBox = null; }
    }

    private void RestartScene() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void SnapToNearestAngle() {
        if (VizualPlayerTransform != null) {
            float currentAngle = VizualPlayerTransform.eulerAngles.z;
            float snappedAngle = Mathf.Round(currentAngle / 90f) * 90f;
            VizualPlayerTransform.rotation = Quaternion.Euler(0, 0, snappedAngle);
        }
    }
}