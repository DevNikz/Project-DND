using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public GameObject ParentArea;
    public List<Character> Characters;
    public List<Enemy> Enemies;

    void Start() {
        ParentArea = this.transform.parent.transform.parent.gameObject;
        InitEnemies();
    }

    void InitEnemies() {
        if(Enemies.Count > 0 ) {
            for(int i = 0; i < Enemies.Count; i++) {
                Enemies[i].currentState = CharacterState.Alive;
                for(int j = 0; j < Enemies[i].Level; j++) {
                    Enemies[i].Health += UtilityMisc.CalculateHealth(Enemies[i].Class, j+1, Enemies[i].Constitution);
                    Enemies[i].Mana += UtilityMisc.CalculateMana(Enemies[i].Class, j+1, Enemies[i].Intelligence);
                }
                Enemies[i].CurrentHealth = Enemies[i].Health;
                Enemies[i].CurrentMana = Enemies[i].Mana;

                Enemies[i].Constitution = UtilityMisc.Constitution(Enemies[i].Class);
                Enemies[i].Intelligence = UtilityMisc.Intelligence(Enemies[i].Class);
                
                //Skills
                if(Enemies[i].Skills.Count > 0) {
                    for(int j = 0; j < Enemies[i].Skills.Count; j++) {
                        Enemies[i].Skills[j].ActualModifier = UtilityMisc.CalculateModifier(Enemies[i].Level, Enemies[i].Skills[j].Modifier);
                    }
                }
            }
        }
        else Debug.Log("Enemies Not Integrated?!");
    }

    [ContextMenu("TriggerCombat")]
    public void TriggerCombat() {
        CheckCount();
        CombatManager.Instance.StartCombat(ParentArea, Characters, Enemies);
    }

    void CheckCount() {
        switch(CompanionManager.Instance.GetCount()) {
            case 1:
                Characters = new List<Character>(2)
                {
                    GetPlayer(),
                    GetCompanion(0)
                }; 
                break;
            case 2:
                Characters = new List<Character>(3)
                {
                    GetPlayer(),
                    GetCompanion(0),
                    GetCompanion(1)
                }; 
                break;
            default:
                Characters = new List<Character>(1) 
                {
                    GetPlayer()
                };
                break;
        }
    }

    Character GetPlayer() {
        Character charTemp = new Character
        {
            currentState = CharacterState.Alive,
            Name = PlayerManager.Instance.Name,
            Class = PlayerManager.Instance.Class,
            Level = PlayerManager.Instance.Level,
            Health = PlayerManager.Instance.Health,
            CurrentHealth = PlayerManager.Instance.Health,
            Mana = PlayerManager.Instance.Mana,
            CurrentMana = PlayerManager.Instance.Mana,
            Skills = PlayerManager.Instance.Skills
        };

        return charTemp;
    }

    Character GetCompanion(int index) {
        Character charTemp = new Character
        {
            currentState = CharacterState.Alive,
            Name = CompanionManager.Instance.Companions[index].Name.ToString(),
            Class = CompanionManager.Instance.Companions[index].Class,
            Level = CompanionManager.Instance.Companions[index].Level,
            Health = CompanionManager.Instance.Companions[index].Health,
            CurrentHealth = CompanionManager.Instance.Companions[index].CurrentHealth,
            Mana = CompanionManager.Instance.Companions[index].Mana,
            CurrentMana = CompanionManager.Instance.Companions[index].Mana,
            Skills = CompanionManager.Instance.Companions[index].Skills
        };

        return charTemp;
    }
}
