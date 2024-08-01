using System;
using System.Collections.Generic;

[Serializable]
public class Character {
    public string Name;
    public int Level;
    public int Health;
    public int Mana;
    public List<Skill> Skills;
}