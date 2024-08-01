using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    public static CompanionManager Instance;

    [SerializeField] public List<Companion> Companions;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        UpdateCompanionStatsAll();
    }

    public void UpdateLevelAll() {
        if(Companions.Count > 0) {
            for(int i = 0; i < Companions.Count; i++) {
                UpdateLevel(i);
            }
        }
    }

    public void UpdateLevel(int index) {
        if(Companions[index].Level >= 20) {
            Companions[index].Level = 20;
            return;
        }
        else {
            Companions[index].Level += 1;
            UpdateCompanionStats(index);
        }
    }

    void UpdateCompanionStatsAll() {
        if(Companions.Count > 0) {
            for(int i = 0; i < Companions.Count; i++ ) {
                UpdateCompanionStats(i);
            }
        } 
    }

    void UpdateCompanionStats(int index) {
        Companions[index].Health = 0;
        Companions[index].Mana = 0;

        for(int i = 0; i < Companions[index].Level; i++) {
            Companions[index].Health += UtilityMisc.CalculateHealth(Companions[index].Class, i+1, Companions[index].Constitution);
            Companions[index].Mana += UtilityMisc.CalculateMana(Companions[index].Class, i+1, Companions[index].Intelligence);
        }

        Companions[index].CurrentHealth = Companions[index].Health;
        Companions[index].CurrentMana = Companions[index].Mana;

        //Stats
        Companions[index].Constitution = UtilityMisc.Constitution(Companions[index].Class);
        Companions[index].Intelligence = UtilityMisc.Intelligence(Companions[index].Class);
    }

    public int GetCount() {
        return Companions.Count;
    }
}
