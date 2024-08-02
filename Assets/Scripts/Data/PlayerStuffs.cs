using UnityEngine;

[System.Serializable]
public class PlayerStuffs {

    //Profile
    public int Level;
    public string Name;
    public string Class;
    public string Race;

    //Basic Stats
    public int Health;
    public int CurrentHealth;
    public int Mana;
    public int CurrentMana;

    //DNDStats
    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;

    //References
    public float[] position;

    public PlayerStuffs(PlayerManager player) {
        //Profile
        Level = player.Level;
        Name = player.Name;
        Class = player.Class.ToString();
        Race = player.Race.ToString();

        //Stats
        Health = player.Health;
        CurrentHealth = player.CurrentHealth;
        Mana = player.Mana;
        CurrentMana = player.CurrentMana;

        //DNDStats
        Strength = player.Strength;
        Dexterity = player.Dexterity;
        Constitution = player.Constitution;
        Intelligence = player.Intelligence;
        Wisdom = player.Wisdom;
        Charisma = player.Charisma;

        //References
        position = new float[3];
        position[0] = player.CurrentPosition.x;
        position[1] = player.CurrentPosition.y;
        position[2] = player.CurrentPosition.z;

    }
}