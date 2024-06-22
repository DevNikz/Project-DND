using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    //Profile
    [Header("Profile")]
    [SerializeField] private string Name;
    [SerializeField][Range(0,10)] private int Level;
    [SerializeField] private EntityClass Class;
    [SerializeField] private EntityRace Race;

    [Header("Stats")]
    [SerializeField][Range(0,20)] private int Strength;
    [SerializeField][Range(0,20)] private int Dexterity;
    [SerializeField][Range(0,20)] private int Constitution;
    [SerializeField][Range(0,20)] private int Intelligence;
    [SerializeField][Range(0,20)] private int Wisdom;
    [SerializeField][Range(0,20)] private int Charisma;

    [Header("DevMenu")]
    [SerializeField] public bool EnableMenu;
    [SerializeField] public bool alwaysSucceed;
    [SerializeField] public bool alwaysFail;
    [SerializeField][Range(0,3)] private int StoryProgression;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    
    void Update() {
        UpdateProfile();
        UpdateStats();
        UpdateDebugMenu();
    }

    void UpdateProfile() {
        PlayerData.Name = Name;
        PlayerData.Level = Level;
        PlayerData.Class = Class;
        PlayerData.Race = Race;
    }

    void UpdateStats(){
        PlayerData.Strength = Strength;
        PlayerData.Dexterity = Dexterity;
        PlayerData.Constitution = Constitution;
        PlayerData.Intelligence = Intelligence;
        PlayerData.Wisdom = Wisdom;
        PlayerData.Charisma = Charisma;
    }

    void UpdateDebugMenu() {
        if(EnableMenu) {
            this.transform.Find("Debug").gameObject.SetActive(true);
        }
        else {
            this.transform.Find("Debug").gameObject.SetActive(false);

            //Disable Hacks
            alwaysSucceed = false;
            alwaysFail = false;
        }
    }

    public void AlwaysSucceed() {
        alwaysSucceed = true;
        alwaysFail = false;
    }

    public void AlwaysFail() {
        alwaysFail = true;
        alwaysSucceed = false;
    }
}
