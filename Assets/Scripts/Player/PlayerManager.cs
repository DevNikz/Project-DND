using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    //Player 
    [SerializeField] private GameObject player;

    //Profile
    [Header("Profile")]
    [SerializeField] public string Name;
    [SerializeField][Range(1,20)] public int Level;
    [SerializeField] public EntityClass Class;
    [SerializeField] public EntityRace Race;

    [Header("Basic Stats")] 
    [SerializeField] public int Health;
    public int CurrentHealth;
    [SerializeField] public int Mana;
    public int CurrentMana;

    [Header("Stats")]
    [SerializeField][Range(0,20)] public int Strength;
    [SerializeField][Range(0,20)] public int Dexterity;
    [SerializeField][Range(0,20)] public int Constitution;
    [SerializeField][Range(0,20)] public int Intelligence;
    [SerializeField][Range(0,20)] public int Wisdom;
    [SerializeField][Range(0,20)] public int Charisma;

    [Header("Skills")]
    [SerializeField] public List<Skill> Skills;

    [Header("Location")]
    [SerializeField] public Vector3 CurrentPosition;

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

        //GameObject
        player = GameObject.Find("/PlayerContainer");

        //Save
        string path = Application.persistentDataPath + "/player.kek";
        if(File.Exists(path)) {
            LoadPlayer();
        }

        UpdateBasicStats();
        UpdateProfile();
        UpdateStats();
        UpdateSkills();
    } 

    
    void Update() {
        UpdateDebugMenu();
    }

    void UpdateProfile() {
        PlayerData.Name = Name;
        PlayerData.Level = Level;
        PlayerData.Class = Class;
        PlayerData.Race = Race;
    }

    void UpdateStats(){
        ClassStats();

        PlayerData.Strength = Strength;
        PlayerData.Dexterity = Dexterity;
        PlayerData.Constitution = Constitution;
        PlayerData.Intelligence = Intelligence;
        PlayerData.Wisdom = Wisdom;
        PlayerData.Charisma = Charisma;

        CurrentPosition = player.transform.position;
        PlayerData.CurrentPosition = CurrentPosition;
    }

    void ClassStats() {
        Strength = UtilityMisc.Strength(Class);
        Dexterity = UtilityMisc.Dexterity(Class);
        Constitution = UtilityMisc.Constitution(Class);
        Intelligence = UtilityMisc.Intelligence(Class);
        Wisdom = UtilityMisc.Wisdom(Class);
        Charisma = UtilityMisc.Charisma(Class); 
    }

    void UpdateBasicStats() {
        Health = 0;
        Mana = 0;
        for(int i = 0; i < Level; i++) {
            Health += UtilityMisc.CalculateHealth(Class, i+1, Constitution); 
            Mana += UtilityMisc.CalculateMana(Class, i+1, Intelligence);
        }

        PlayerData.MaxHealth = Health;
        PlayerData.MaxMana = Mana;
        CurrentHealth = Health;
        CurrentMana = Mana;
    }

    void UpdateSkills() {
        if(Skills.Count > 0) {
            for(int i = 0; i < Skills.Count; i++) {
                Skills[i].ActualModifier = UtilityMisc.CalculateModifier(Level, Skills[i].Modifier);
            }
        }
    }

    [ContextMenu("Level Up")]
    public void UpdateLevel() {
        if(Level >= 20) {
            Debug.Log("Max Level Reached.");
            Level = 20;
            return;
        }
        else {
            Level += 1;
            UpdateBasicStats();
            UpdateSkills();
        }
    }

    //SaveManagement
    [ContextMenu("Save")]
    public void SavePlayer() {
        UpdateProfile();
        UpdateStats();
        SaveSystem.SavePlayer(this);
    }

    [ContextMenu("Load")]
    public void LoadPlayer() {
        PlayerStuffs data = SaveSystem.LoadPlayer();
        Level = data.Level;
        Name = data.Name;
        Class = (EntityClass) System.Enum.Parse(typeof(EntityClass), data.Class);
        Race = (EntityRace) System.Enum.Parse(typeof(EntityRace), data.Race);
        Strength = data.Strength;
        Dexterity = data.Dexterity;
        Constitution = data.Constitution;
        Intelligence = data.Intelligence;
        Wisdom = data.Wisdom;
        Charisma = data.Charisma;

        player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        CurrentPosition = player.transform.position;
    }

    //Debug (Cheats)
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
