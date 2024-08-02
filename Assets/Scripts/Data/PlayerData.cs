using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    //Profile
    public static int Level { get; set; }
    public static string Name { get; set; }
    public static EntityClass Class { get; set; }
    public static EntityRace Race { get; set; }

    //Basic Stats
    public static int MaxHealth { get; set; }
    public static int CurrentHealth { get; set; }
    public static int MaxMana { get; set; }
    public static int CurrentMana { get; set; }

    //Stats
    public static int Strength { get; set; }

    public static int Dexterity { get; set; }

    public static int Constitution { get; set; }

    public static int Intelligence { get; set; }

    public static int Wisdom { get; set; }

    public static int Charisma { get; set; }

    //Location
    public static Vector3 CurrentPosition { get; set; }

    //Skills
    public static List<Skill> Skills { get; set; }

    //Player Movement
    public static DirectionH directionH { get; set; }
    public static DirectionV directionV { get; set; }
    public static EntityState entityState { get; set; }
}
