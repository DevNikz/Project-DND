using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public List<Character> Characters;
    public List<Enemy> Enemies;

    void Start() {
        InitEnemies();
    }

    void InitEnemies() {
        if(Enemies.Count > 0 ) {
            for(int i = 0; i < Enemies.Count; i++) {
                for(int j = 0; j < Enemies[i].Level; j++) {
                    Enemies[i].Health += UtilityMisc.CalculateHealth(Enemies[i].Class, j+1, Enemies[i].Constitution);
                    Enemies[i].Mana += UtilityMisc.CalculateMana(Enemies[i].Class, j+1, Enemies[i].Intelligence);
                }
                Enemies[i].CurrentHealth = Enemies[i].Health;
                Enemies[i].CurrentMana = Enemies[i].Mana;

                Enemies[i].Constitution = UtilityMisc.Constitution(Enemies[i].Class);
                Enemies[i].Intelligence = UtilityMisc.Intelligence(Enemies[i].Class);
            }
        }
        else Debug.Log("Enemies Not Integrated?!");
    }

    public void TriggerCombat() {
        CheckCount();
        CombatManager.Instance.StartCombat(this.gameObject, Characters, Enemies);
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
            Name = PlayerManager.Instance.Name,
            Level = PlayerManager.Instance.Level,
            Health = PlayerManager.Instance.Health,
            Mana = PlayerManager.Instance.Mana,
            Skills = PlayerManager.Instance.Skills
        };

        return charTemp;
    }

    Character GetCompanion(int index) {
        Character charTemp = new Character
        {
            Name = CompanionManager.Instance.Companions[index].Name.ToString(),
            Level = CompanionManager.Instance.Companions[index].Level,
            Health = CompanionManager.Instance.Companions[index].Health,
            Mana = CompanionManager.Instance.Companions[index].Mana,
            Skills = CompanionManager.Instance.Companions[index].Skills
        };

        return charTemp;
    }


    // void GetPlayerStats() {
    //     Characters[0].Name = PlayerManager.Instance.Name;
    //     Characters[0].Level = PlayerManager.Instance.Level;
    //     Characters[0].Health = PlayerManager.Instance.Health;
    //     Characters[0].Mana = PlayerManager.Instance.Mana;
    //     Characters[0].Skills = PlayerManager.Instance.Skills;
    // }

    // void GetCompanionStats() {
    //     if(CompanionManager.Instance.Companions.Count > 0) {
    //         for(int i = 0; i < CompanionManager.Instance.Companions.Count; i++) {
    //             Characters[i+1].Name = CompanionManager.Instance.Companions[i].Name.ToString();
    //             Characters[i+1].Level = CompanionManager.Instance.Companions[i].Level;
    //             Characters[i+1].Health = CompanionManager.Instance.Companions[i].Health;
    //             Characters[i+1].Mana = CompanionManager.Instance.Companions[i].Mana;
    //             Characters[i+1].Skills = CompanionManager.Instance.Companions[i].Skills;
    //         }
    //     } 
    //     else Debug.Log("No Companions Detected");
    // }

}
