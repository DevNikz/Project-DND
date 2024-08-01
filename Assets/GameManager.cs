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
        //if(CompanionManager.Instance.Companions.Count > 0) UpdateCompanionUI(CompanionManager.Instance.Companions.Count);
    }

    public void UpdatePlayerUI() {
        //Update Player Text
        PlayerText.text = $"{PlayerManager.Instance.Name} | Level {PlayerManager.Instance.Level}";

        //Update Player Icon
        PlayerIcon.sprite = Resources.Load<Sprite>("Characters/Player");

        //Update Stats
        PlayerHP.value = UtilityMisc.ConvertSliderVal(PlayerManager.Instance.CurrentHealth, PlayerManager.Instance.Health);
        PlayerMP.value = UtilityMisc.ConvertSliderVal(PlayerManager.Instance.CurrentMana, PlayerManager.Instance.Mana);
    }

    public void UpdateCompanionUI(int count) {
        for(int i = 0; i < count; i++) {
            switch(i) {
                case 0:
                    Companion1.SetActive(true);
                    CompanionText1.text = $"{CompanionManager.Instance.Companions[i].Name} | Level {CompanionManager.Instance.Companions[i].Level}";
                    CompanionIcon1.sprite = Resources.Load<Sprite>($"Characters/{CompanionManager.Instance.Companions[i].Name}");
                    CompanionHP1.value = UtilityMisc.ConvertSliderVal(CompanionManager.Instance.Companions[i].CurrentHealth, CompanionManager.Instance.Companions[i].Health);
                    CompanionMP1.value = UtilityMisc.ConvertSliderVal(CompanionManager.Instance.Companions[i].CurrentMana, CompanionManager.Instance.Companions[i].Mana);
                    break;
                case 1:
                    Companion2.SetActive(true);
                    CompanionText2.text = $"{CompanionManager.Instance.Companions[i].Name} | Level {CompanionManager.Instance.Companions[i].Level}";
                    CompanionIcon2.sprite = Resources.Load<Sprite>($"Characters/{CompanionManager.Instance.Companions[i].Name}");
                    CompanionHP2.value = UtilityMisc.ConvertSliderVal(CompanionManager.Instance.Companions[i].CurrentHealth, CompanionManager.Instance.Companions[i].Health);
                    CompanionMP2.value = UtilityMisc.ConvertSliderVal(CompanionManager.Instance.Companions[i].CurrentMana, CompanionManager.Instance.Companions[i].Mana);
                    break;
            }
        }
    }
}
