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
            LoadGame();
        }

        else {
            InitBasicStats();
            InitProfile();
            InitStats();
            InitSkills();
        }

    } 

    
    void Update() {
        UpdateDebugMenu();
    }

    void InitProfile() {
        PlayerData.Name = Name;
        PlayerData.Level = Level;
        PlayerData.Class = Class;
        PlayerData.Race = Race;
    }

    void InitStats(){
        PlayerData.MaxHealth = Health;
        PlayerData.CurrentHealth = CurrentHealth;
        PlayerData.MaxMana = Mana;
        PlayerData.CurrentMana = CurrentMana;

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

    void InitBasicStats() {
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

    void InitSkills() {
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

    public void UpdateBasicStats() {
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

    public void UpdateBasicStats(int currentHP, int currentMP) {
        CurrentHealth = currentHP;
        CurrentMana = currentMP;
    }

    public void UpdateSkills() {
        if(Skills.Count > 0) {
            for(int i = 0; i < Skills.Count; i++) {
                Skills[i].ActualModifier = UtilityMisc.CalculateModifier(Level, Skills[i].Modifier);
            }
        }
    }

    //SaveManagement
    [ContextMenu("Save")]
    public void SavePlayer() {
        //Player
        InitProfile();
        InitStats();
        SaveSystem.SavePlayer(this);

        //PlayerSkills
        for(int i = 0; i < Skills.Count; i++) {
            Skill skill = new Skill {
                Category = Skills[i].Category,
                Type = Skills[i].Type,
                Modifier = Skills[i].Modifier,
                ActualModifier = Skills[i].ActualModifier,
                Requirement = Skills[i].Requirement,
                Value = Skills[i].Value
            };
            SaveSystem.SavePlayerSkills(skill, i);
        }

        //Companion
        Companion companion;
        for(int i = 0; i < CompanionManager.Instance.GetCount(); i++) {
            companion = new Companion {
                Level = CompanionManager.Instance.Companions[i].Level,
                Name = CompanionManager.Instance.Companions[i].Name,
                Class = CompanionManager.Instance.Companions[i].Class,
                Health = CompanionManager.Instance.Companions[i].Health,
                CurrentHealth = CompanionManager.Instance.Companions[i].CurrentHealth,
                Mana = CompanionManager.Instance.Companions[i].Mana,
                CurrentMana = CompanionManager.Instance.Companions[i].CurrentMana,
                Constitution = CompanionManager.Instance.Companions[i].Constitution,
                Intelligence = CompanionManager.Instance.Companions[i].Intelligence
            };
            SaveSystem.SaveCompanion(companion, i);

            //Skills
            for(int j = 0; j < CompanionManager.Instance.Companions[i].Skills.Count; j++) {
                Skill skill = new Skill {
                    Category = CompanionManager.Instance.Companions[i].Skills[j].Category,
                    Type = CompanionManager.Instance.Companions[i].Skills[j].Type,
                    Modifier = CompanionManager.Instance.Companions[i].Skills[j].Modifier,
                    ActualModifier = CompanionManager.Instance.Companions[i].Skills[j].ActualModifier,
                    Requirement = CompanionManager.Instance.Companions[i].Skills[j].Requirement,
                    Value = CompanionManager.Instance.Companions[i].Skills[j].Value
                };
                SaveSystem.SaveCompanionSkills(skill, i, j);
            }
        }
    }

    [ContextMenu("Load")]
    public void LoadGame() {
        LoadPlayer();
        LoadCompanions();
    }

    void LoadPlayer() {
        //Player
        PlayerStuffs data = SaveSystem.LoadPlayer();

        //Profile
        Level = data.Level;
        Name = data.Name;
        Class = (EntityClass) System.Enum.Parse(typeof(EntityClass), data.Class);
        Race = (EntityRace) System.Enum.Parse(typeof(EntityRace), data.Race);

        //Stats
        Health = data.Health;
        CurrentHealth = data.CurrentHealth;
        Mana = data.Mana;
        CurrentMana = data.CurrentMana;

        //DND
        Strength = data.Strength;
        Dexterity = data.Dexterity;
        Constitution = data.Constitution;
        Intelligence = data.Intelligence;
        Wisdom = data.Wisdom;
        Charisma = data.Charisma;

        //Reference
        player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        CurrentPosition = player.transform.position;

        //Skills
        SkillStuffs data2;
        for(int i = 0; i < Skills.Count; i++) {
            data2 = SaveSystem.LoadPlayerSkills(i);
            Skills[i].Category = (SkillCategory) System.Enum.Parse(typeof(SkillCategory), data2.Category);
            Skills[i].Type = (SkillType) System.Enum.Parse(typeof(SkillType), data2.Type);
            Skills[i].Modifier = data2.Modifier;
            Skills[i].ActualModifier = data2.ActualModifier;
            Skills[i].Requirement = (SkillReqType) System.Enum.Parse(typeof(SkillReqType), data2.Requirement);
            Skills[i].Value = data2.Value;
        }
    }

    public void LoadCompanions() {
        CompanionStuffs data1;
        SkillStuffs data2;
        for(int i = 0; i < CompanionManager.Instance.GetCount(); i++) {
            data1 = SaveSystem.LoadCompanions(i);
            CompanionManager.Instance.Companions[i].Level = data1.Level;
            CompanionManager.Instance.Companions[i].Name = (CompanionType) System.Enum.Parse(typeof(CompanionType), data1.Name);
            CompanionManager.Instance.Companions[i].Class = (EntityClass) System.Enum.Parse(typeof(EntityClass), data1.Class);
            CompanionManager.Instance.Companions[i].Health = data1.Health;
            CompanionManager.Instance.Companions[i].CurrentHealth = data1.CurrentHealth;
            CompanionManager.Instance.Companions[i].Mana = data1.Mana;
            CompanionManager.Instance.Companions[i].CurrentMana = data1.CurrentMana;
            CompanionManager.Instance.Companions[i].Constitution = data1.Constitution;
            CompanionManager.Instance.Companions[i].Intelligence = data1.Intelligence;

            for(int j = 0; j < CompanionManager.Instance.Companions[i].Skills.Count; j++) {
                data2 = SaveSystem.LoadCompanionSkills(i, j);
                CompanionManager.Instance.Companions[i].Skills[j].Category = (SkillCategory) System.Enum.Parse(typeof(SkillCategory), data2.Category);
                CompanionManager.Instance.Companions[i].Skills[j].Type = (SkillType) System.Enum.Parse(typeof(SkillType), data2.Type);
                CompanionManager.Instance.Companions[i].Skills[j].Modifier = data2.Modifier;
                CompanionManager.Instance.Companions[i].Skills[j].ActualModifier = data2.ActualModifier;
                CompanionManager.Instance.Companions[i].Skills[j].Requirement = (SkillReqType) System.Enum.Parse(typeof(SkillReqType), data2.Requirement);
                CompanionManager.Instance.Companions[i].Skills[j].Value = data2.Value;
            }
        }
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
