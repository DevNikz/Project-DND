using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int Strength { get; set; }

    public static int Dexterity { get; set; }

    public static int Constitution { get; set; }

    public static int Intelligence { get; set; }

    public static int Wisdom { get; set; }

    public static int Charisma { get; set; }

    public static DirectionH directionH { get; set; }
    public static DirectionV directionV { get; set; }
    public static EntityState entityState { get; set; }
}
