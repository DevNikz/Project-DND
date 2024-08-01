using System;
using UnityEngine;

[Serializable]
public class Skill {
    [Header("Properties")]
    public SkillCategory Category;
    public SkillType Type;
    [Range(1,1000)] public int Modifier;
    public int ActualModifier;

    [Header("Requirement")]
    public SkillReqType Requirement;
    [Range(0,100)] public int Value;
}