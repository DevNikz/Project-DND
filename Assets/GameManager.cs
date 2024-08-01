using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Reference")]
    public GameObject CharacterUI;
    private GameObject Player;
    private GameObject Companion1;
    private GameObject Companion2;

    //Identity
    [Header("Text")]
    public TextMeshProUGUI PlayerText;
    public TextMeshProUGUI CompanionText1;
    public TextMeshProUGUI CompanionText2;

    //Icon
    [Header("Icon")]
    public Image PlayerIcon;
    public Image CompanionIcon1;
    public Image CompanionIcon2;

    //Health
    [Header("Health")]
    public Slider PlayerHP;
    public Slider CompanionHP1;
    public Slider CompanionHP2;

    //Mana
    [Header("Mana")]
    public Slider PlayerMP;
    public Slider CompanionMP1;
    public Slider CompanionMP2;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        CharacterUI = GameObject.Find("/MainUI/Characters");
        Player = CharacterUI.transform.Find("Player").gameObject;
        Companion1 = CharacterUI.transform.Find("Companion1").gameObject;
        Companion2 = CharacterUI.transform.Find("Companion2").gameObject;

        PlayerText = Player.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();
        CompanionText1 = Companion1.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();
        CompanionText2 = Companion2.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();

        PlayerIcon = Player.transform.Find("Icon").GetComponent<Image>();
        CompanionIcon1 = Companion1.transform.Find("Icon").GetComponent<Image>();
        CompanionIcon2 = Companion2.transform.Find("Icon").GetComponent<Image>();

        PlayerHP = Player.transform.Find("HUD/Health").GetComponent<Slider>();
        CompanionHP1 = Companion1.transform.Find("HUD/Health").GetComponent<Slider>();
        CompanionHP2 = Companion2.transform.Find("HUD/Health").GetComponent<Slider>();

        PlayerMP = Player.transform.Find("HUD/Mana").GetComponent<Slider>();
        CompanionMP1 = Companion1.transform.Find("HUD/Mana").GetComponent<Slider>();
        CompanionMP2 = Companion2.transform.Find("HUD/Mana").GetComponent<Slider>();
    }

    void Start() {
        UpdateUI();
    }

    public void UpdateUI() {
        UpdatePlayerUI();
        if(CompanionManager.Instance.GetCount() > 0) {
            Debug.Log("Has Companions");
            UpdateCompanionUI(CompanionManager.Instance.GetCount());
        }
    }

    public void UpdatePlayerUI() {
        //Update Player Text
        PlayerText.text = $"{PlayerManager.Instance.Name} | Level {PlayerManager.Instance.Level}";

        //Update Player Icon
        PlayerIcon.sprite = Resources.Load<Sprite>($"Characters/{PlayerManager.Instance.Class}Player");

        //Update Stats
        PlayerHP.wholeNumbers = true;
        PlayerHP.maxValue = PlayerManager.Instance.Health;
        PlayerHP.value = PlayerManager.Instance.CurrentHealth;

        PlayerMP.wholeNumbers = true;
        PlayerMP.maxValue = PlayerManager.Instance.Mana;
        PlayerMP.value = PlayerManager.Instance.CurrentMana;
    }

    public void UpdateCompanionUI(int count) {
        for(int i = 0; i < count; i++) {
            switch(i) {
                case 0:
                    Companion1.SetActive(true);
                    CompanionText1.text = $"{CompanionManager.Instance.Companions[i].Name} | Level {CompanionManager.Instance.Companions[i].Level}";
                    CompanionIcon1.sprite = Resources.Load<Sprite>($"Characters/{CompanionManager.Instance.Companions[i].Name}");

                    CompanionHP1.wholeNumbers = true;
                    CompanionHP1.maxValue = CompanionManager.Instance.Companions[i].Health;
                    CompanionHP1.value = CompanionManager.Instance.Companions[i].CurrentHealth;

                    CompanionMP1.wholeNumbers = true;
                    CompanionMP1.maxValue = CompanionManager.Instance.Companions[i].Mana;
                    CompanionMP1.value = CompanionManager.Instance.Companions[i].CurrentMana;
                    break;
                case 1:
                    Companion2.SetActive(true);
                    CompanionText2.text = $"{CompanionManager.Instance.Companions[i].Name} | Level {CompanionManager.Instance.Companions[i].Level}";
                    CompanionIcon2.sprite = Resources.Load<Sprite>($"Characters/{CompanionManager.Instance.Companions[i].Name}");

                    CompanionHP2.wholeNumbers = true;
                    CompanionHP2.maxValue = CompanionManager.Instance.Companions[i].Health;
                    CompanionHP2.value = CompanionManager.Instance.Companions[i].CurrentHealth;

                    CompanionMP2.wholeNumbers = true;
                    CompanionMP2.maxValue = CompanionManager.Instance.Companions[i].Mana;
                    CompanionMP2.value = CompanionManager.Instance.Companions[i].CurrentMana;
                    break;
            }
        }
    }
}
