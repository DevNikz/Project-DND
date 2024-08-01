using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy {
    public EnemyType Name;
    public EntityClass Class;
    [Range(1,20)] public int Level;
    public int Health;
    public int CurrentHealth;
    public int Mana;
    public int CurrentMana;
    public int Constitution;
    public int Intelligence;
    public List<Skill> Skills;
}