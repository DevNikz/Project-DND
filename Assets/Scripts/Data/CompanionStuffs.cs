using UnityEngine;

[System.Serializable]
public class CompanionStuffs {

    //Profile
    public int Level;
    public string Name;
    public string Class;

    //Basic Stats
    public int Health;
    public int CurrentHealth;
    public int Mana;
    public int CurrentMana;

    //DNDStats
    public int Constitution;
    public int Intelligence;

    public CompanionStuffs(Companion companion) {
        //Profile
        Level = companion.Level;
        Name = companion.Name.ToString();
        Class = companion.Class.ToString();

        //Stats
        Health = companion.Health;
        CurrentHealth = companion.CurrentHealth;
        Mana = companion.Mana;
        CurrentMana = companion.CurrentMana;

        //DNDStats
        Constitution = companion.Constitution;
        Intelligence = companion.Intelligence;
    }
}