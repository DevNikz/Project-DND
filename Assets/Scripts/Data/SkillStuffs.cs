using UnityEngine;

[System.Serializable]
public class SkillStuffs {

    public string Category;
    public string Type;
    public int Modifier;
    public int ActualModifier;
    public string Requirement;
    public int Value;

    public SkillStuffs(Skill skill) {
        Category = skill.Category.ToString();
        Type = skill.Type.ToString();
        Modifier = skill.Modifier;
        ActualModifier = skill.ActualModifier;
        Requirement = skill.Requirement.ToString();
        Value = skill.Value;
    }
}