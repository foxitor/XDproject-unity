using UnityEngine; using UnityEngine.UI; using UnityEngine.EventSystems; using System.Collections.Generic;

public class GuidingInstructor : MonoBehaviour {
    public GameObject Environment; public Sprite[] Portraits;
    private Text displayText, actionText; private Image displayImage;
    private int currentPage; private string pageDomen;
    private GameTool GameSetting; private int ReturnGameSpeed;

    void Start() {
        displayText = Environment.GetComponent<Transform>().GetChild(1).GetChild(0).GetComponent<Text>();
        actionText = Environment.GetComponent<Transform>().GetChild(1).GetChild(1).GetComponent<Text>();
        displayImage = Environment.GetComponent<Transform>().GetChild(0).GetComponent<Image>();
        GameSetting = GameObject.Find("Main Camera").GetComponent<GameTool>();
    }
    void Update() { 
        LoadPageLogic(pageDomen, currentPage); CheckPortrait();
        displayText.text = PageDisplay(pageDomen, currentPage); actionText.text = ActionDisplay(pageDomen, currentPage);
    }
    public void EnableInstructor() {
        Environment.SetActive(true);
        pageDomen = "Starter"; currentPage = 0;
        EventSystem.current.SetSelectedGameObject(null);
        ReturnGameSpeed = GameSetting.Speed; Time.timeScale = 0;
    }
    string PageDisplay(string Domen, int Page) {
        if (string.IsNullOrEmpty(Domen))
            return "Error: Domen is null or empty!";
        if (pages.ContainsKey(Domen) && pages[Domen].ContainsKey(Page)) {
            return pages[Domen][Page];
        }
        return "Error01 No Content Found!";
    }
    string ActionDisplay(string Domen, int Page) {
        string ResultText = "Error01";
        if (Domen == "Starter") {
            ResultText = "Actions\nEnter - Exit, C - Learn About Cookies, T - Learn About Tools, N - Learn About Nuts.";
        } if (Domen == "Cookie") {
            ResultText = "Actions\nEnter - Continue.";
        } if (Domen == "Nut") {
            ResultText = "Actions\nEnter - Continue.";
        } if (Domen == "Tools") {
            ResultText = "Actions\nEnter - Continue.";
        } if (Domen == "Main") {
            ResultText = "Actions\nEnter - Exit, C - Learn About Cookies, T - Learn About Tools, N - Learn About Nuts.";
        }
        return ResultText;
    }
    void LoadPageLogic(string Domen, int Page) {
        if (Domen == "Starter" || Domen == "Main") {
            if (Input.GetKeyDown(KeyCode.Return)) { Deactivate(); }
            if (Input.GetKeyDown(KeyCode.C)) { 
                pageDomen = "Cookie"; currentPage = 0;
            } if (Input.GetKeyDown(KeyCode.N)) { 
                pageDomen = "Nut"; currentPage = 0;
            } if (Input.GetKeyDown(KeyCode.T)) { 
                pageDomen = "Tools"; currentPage = 0;
            }
        } 
        if (Domen == "Cookie" && currentPage != 8) {
            if (Input.GetKeyDown(KeyCode.Return)) { currentPage++; }
        } else if (Domen == "Cookie" && currentPage == 8) {
            if (Input.GetKeyDown(KeyCode.Return)) { 
                pageDomen = "Main"; currentPage = 0; 
            }
        } if (Domen == "Nut" && currentPage != 11) {
            if (Input.GetKeyDown(KeyCode.Return)) { currentPage++; }
        } else if (Domen == "Nut" && currentPage == 11) {
            if (Input.GetKeyDown(KeyCode.Return)) { 
                pageDomen = "Main"; currentPage = 0; 
            }
        } if (Domen == "Tools" && currentPage != 9) {
            if (Input.GetKeyDown(KeyCode.Return)) { currentPage++; }
        } else if (Domen == "Tools" && currentPage == 9) {
            if (Input.GetKeyDown(KeyCode.Return)) { 
                pageDomen = "Main"; currentPage = 0; 
            }
        }
    }
    private void CheckPortrait() {
        displayImage.sprite = Portraits[0];
        if (pageDomen == "Main" || pageDomen == "Starter") {
            displayImage.sprite = Portraits[2];
        } if (pageDomen == "Cookie" && currentPage == 7) {
            displayImage.sprite = Portraits[1];
        } if (pageDomen == "Nut" && currentPage == 4) {
            displayImage.sprite = Portraits[1];
        } if (pageDomen == "Nut" && currentPage == 6) {
            displayImage.sprite = Portraits[1];
        } if (pageDomen == "Nut" && currentPage == 8) {
            displayImage.sprite = Portraits[2];
        } if (pageDomen == "Tools" && currentPage == 6) {
            displayImage.sprite = Portraits[2];
        } if (pageDomen == "Tools" && currentPage == 5) {
            displayImage.sprite = Portraits[1];
        } if (pageDomen == "Tools" && currentPage == 9) {
            displayImage.sprite = Portraits[1];
        }
    }
    private void Deactivate() { 
        Environment.SetActive(false); 
        Time.timeScale = ReturnGameSpeed;
    }

    Dictionary<string, Dictionary<int, string>> pages = new Dictionary<string, Dictionary<int, string>>() {
        { "Starter", new Dictionary<int, string> {
            { 0, "Hey Pal!\nAnything need to know?" }
        }},
        { "Cookie", new Dictionary<int, string> {
            { 0, "Cookies are creatures we study in our factory." },
            { 1, "They eat berries and drink water puddles." },
            { 2, "Counting chips on their forehead reveals their 'parenting gen'." },
            { 3, "Chip count indicates their gen strength." },
            { 4, "Gen=1: can't have children; Gen=2: can reproduce.\nGen=3: higher chances of reproduction. Gen >3 is impossible." },
            { 5, "Cookies produce children by eating berries." },
            { 6, "'P-gen' mutates randomly when a kid is born." },
            { 7, "I'm not one of them. Nobody in the factory is." },
            { 8, "Almost forgot you can select this fellas and change his P-gen by pressing 'P'.\nDEVELOPER_NOTE: now you can't bro."}
        }},
        { "Nut", new Dictionary<int, string> {
            { 0, "Nut - creature of wild.." },
            { 1, "Those preditors eat Cookies. And drink the same water that Cookies drink." },
            { 2, "They can make kids only if they eat 1 Cookie." },
            { 3, "For breeding they need to find partner." },
            { 4, "Or instead of finding parner they can randomly spray spores." },
            { 5, "Those 'Nut Spores' - work like eggs, that spawn new nut after 20 sec." },
            { 6, "And those fellas kill by jumping on their prey. >:o" },
            { 7, "Nuts can move ONLY by jumping across territory" },
            { 8, "When that said, on start of the simulation we have 2 ways" },
            { 9, "Or Cookies all die cuz of the preditor. Or Cookies WIN the preditor!" },
            { 10, "Yes! male Cookies arent usless. they can try attack preditor while females runing."},
            { 11, "And those fights often end... Not so positve for cookies :("}
        }},
        { "Tools", new Dictionary<int, string> {
            { 0, "In this factory, we use tools to make simulation easier." },
            { 1, "Tools are under these 3 bright buttons." },
            { 2, "1st: 'Simulation Speed' — tap to change speed.\nMax is 5; after that, it loops back to 1." },
            { 3, "2nd: 'Clear' — clears all objects on the ground.\nCreatures stay safe." },
            { 4, "3rd: 'Rain' — simulates rain to see how creatures adapt.\nDrops create water puddles for drinking." },
            { 5, "4th: 'Nuke' — restarts the simulation. Use carefully!" },
            { 6, "After these tools, there are objects!\nLet me tell you about them >:3" },
            { 7, "Item 'Bush' — place it for creatures to eat.\nIt regrows after being eaten." },
            { 8, "Item 'Puddle' — for drinking.\nBut it doesn't regrow after use." },
            { 9, "Btw I dont Understand what this eye feature does, something happens with colors..?" }
        }},
        { "Main", new Dictionary<int, string> {
            {0, "Wanna hear more, partner?" }
        }}
    };
}
