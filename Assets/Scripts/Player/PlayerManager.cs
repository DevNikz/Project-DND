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
    [SerializeField][Range(0,10)] public int Level;
    [SerializeField] public EntityClass Class;
    [SerializeField] public EntityRace Race;

    [Header("Stats")]
    [SerializeField][Range(0,20)] public int Strength;
    [SerializeField][Range(0,20)] public int Dexterity;
    [SerializeField][Range(0,20)] public int Constitution;
    [SerializeField][Range(0,20)] public int Intelligence;
    [SerializeField][Range(0,20)] public int Wisdom;
    [SerializeField][Range(0,20)] public int Charisma;

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

        player = GameObject.Find("/PlayerContainer");

        string path = Application.persistentDataPath + "/player.kek";
        if(File.Exists(path)) {
            LoadPlayer();
        }
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

        CurrentPosition = player.transform.position;
        PlayerData.CurrentPosition = CurrentPosition;
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

    [ContextMenu("Save")]
    public void SavePlayer() {
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
}
