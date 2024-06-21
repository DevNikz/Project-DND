using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public bool endFrame;
    [SerializeField] public GameObject MainUI;
    private GameObject bg;
    private GameObject baseUI;
    private Animator bgAnimate;
    private Animator baseUIAnimate;

    //Profile References
    private GameObject profileParent;
    private TextMeshProUGUI PLevel;
    private TextMeshProUGUI PName;
    private TextMeshProUGUI PClass;
    private TextMeshProUGUI PRace;

    //Stat References
    private GameObject statVParent;
    private TextMeshProUGUI strStat;
    private TextMeshProUGUI dexStat;
    private TextMeshProUGUI conStat;
    private TextMeshProUGUI intStat;
    private TextMeshProUGUI wisStat;
    private TextMeshProUGUI chaStat;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() {
        //Animation
        bg = this.transform.Find("Inventory").transform.Find("BG").gameObject;
        baseUI = this.transform.Find("Inventory").transform.Find("Base").gameObject;

        //Profile
        profileParent = baseUI.transform.Find("Profile").gameObject;
        PLevel = profileParent.transform.Find("Level").GetComponent<TextMeshProUGUI>();
        PName = profileParent.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        PClass = profileParent.transform.Find("Class").GetComponent<TextMeshProUGUI>();
        PRace = profileParent.transform.Find("Race").GetComponent<TextMeshProUGUI>();

        //Stats
        statVParent = baseUI.transform.Find("StatValues").gameObject;
        strStat = statVParent.transform.Find("STR").GetComponent<TextMeshProUGUI>();
        dexStat = statVParent.transform.Find("DEX").GetComponent<TextMeshProUGUI>();
        conStat = statVParent.transform.Find("CON").GetComponent<TextMeshProUGUI>();
        intStat = statVParent.transform.Find("INT").GetComponent<TextMeshProUGUI>();
        wisStat = statVParent.transform.Find("WIS").GetComponent<TextMeshProUGUI>();
        chaStat = statVParent.transform.Find("CHA").GetComponent<TextMeshProUGUI>();
    }
    

    //Animation
    public void OpenInventory() {
        MainUI.SetActive(false);

        //Animate
        bgAnimate = bg.GetComponent<Animator>();
        baseUIAnimate = baseUI.GetComponent<Animator>();
        bgAnimate.Play("BGOpen");
        baseUIAnimate.SetBool("IsOpen", true);

        //Active
        bg.SetActive(true);
        baseUI.SetActive(true);
    }

    public void CloseInventory() {
        baseUIAnimate.SetBool("IsOpen",false);
    }

    void Update() {
        //Animation
        if(this.endFrame == true) {
            bg.SetActive(false);
            baseUI.SetActive(false);
            MainUI.SetActive(true);
            this.endFrame = false;
        }

        //Profile
        UpdateProfile();

        //Stat
        UpdateStats();
    }

    void UpdateProfile() {
        PLevel.text = "LEVEL " + PlayerData.Level.ToString();
        PName.text = "NAME: " + PlayerData.Name;
        PClass.text = "CLASS: " + PlayerData.Class.ToString();
        PRace.text = "RACE: " + PlayerData.Race.ToString();
    }

    void UpdateStats() {
        strStat.text = PlayerData.Strength.ToString();
        dexStat.text = PlayerData.Dexterity.ToString();
        conStat.text = PlayerData.Constitution.ToString();
        intStat.text = PlayerData.Intelligence.ToString();
        wisStat.text = PlayerData.Wisdom.ToString();
        chaStat.text = PlayerData.Charisma.ToString();
    }
}
