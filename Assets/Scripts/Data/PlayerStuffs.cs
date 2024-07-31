using UnityEngine;

[System.Serializable]
public class PlayerStuffs {
    public int Level;
    public string Name;
    public string Class;
    public string Race;
    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;
    public float[] position;

    public PlayerStuffs(PlayerManager player) {
        Level = player.Level;
        Name = player.Name;
        Class = player.Class.ToString();
        Race = player.Race.ToString();
        Strength = player.Strength;
        Dexterity = player.Dexterity;
        Constitution = player.Constitution;
        Intelligence = player.Intelligence;
        Wisdom = player.Wisdom;
        Charisma = player.Charisma;

        position = new float[3];
        position[0] = player.CurrentPosition.x;
        position[1] = player.CurrentPosition.y;
        position[2] = player.CurrentPosition.z;

    }
}