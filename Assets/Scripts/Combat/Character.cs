using System;
using System.Collections.Generic;

[Serializable]
public class Character {
    public string Name;
    public EntityClass Class;
    public int Level;
    public int Health;
    public int CurrentHealth;
    public int Mana;
    public int CurrentMana;
    public List<Skill> Skills;
}