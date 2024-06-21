using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Profile
    [Header("Profile")]
    [SerializeField] private string Name;
    [SerializeField][Range(0,10)] private int Level;
    [SerializeField] private EntityClass Class;
    [SerializeField] private EntityRace Race;

    [Header("Stats")]
    [SerializeField][Range(0,20)] private int Strength;
    [SerializeField][Range(0,20)] private int Dexterity;
    [SerializeField][Range(0,20)] private int Constitution;
    [SerializeField][Range(0,20)] private int Intelligence;
    [SerializeField][Range(0,20)] private int Wisdom;
    [SerializeField][Range(0,20)] private int Charisma;
    
    void Update() {
        UpdateProfile();
        UpdateStats();
    }

    void UpdateProfile() {
        PlayerData.Name = Name;
        PlayerData.Level = Level;
        PlayerData.Class = Class;
        PlayerData.Race = Race;
    }

    void UpdateStats(){
        PlayerData.Strength = Strength;
        PlayerData.Dexterity = Dexterity;
        PlayerData.Constitution = Constitution;
        PlayerData.Intelligence = Intelligence;
        PlayerData.Wisdom = Wisdom;
        PlayerData.Charisma = Charisma;
    }
}
