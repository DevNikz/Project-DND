using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    public static CombatManager Instance;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartCombat(GameObject trigger, List<Character> characters, List<Enemy> enemies) {
        Debug.Log("Combat Triggered!");
    }
}