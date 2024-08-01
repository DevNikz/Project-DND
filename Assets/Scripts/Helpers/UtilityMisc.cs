using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UtilityMisc : MonoBehaviour{
    public static int CalculateHealth(EntityClass entityClass, int Level, int Constitution) {
        int Health = 0;
        switch(Level) {
            case 1:
                switch(entityClass) {
                    case EntityClass.Wizard:
                        Health = 6 + Constitution;
                        break;
                    case EntityClass.Fighter:
                        Health = 10 + Constitution;
                        break;
                    case EntityClass.Ranger:
                        Health = 10 + Constitution;
                        break;
                }
                break;
            default:
                switch(entityClass) {
                    case EntityClass.Wizard:
                        Health = 4 + Constitution;
                        break;
                    case EntityClass.Fighter:
                        Health = 6 + Constitution;
                        break;
                    case EntityClass.Ranger:
                        Health = 6 + Constitution;
                        break;
                }
                break;
        }
        

        return Health;
    }

    

    public static int CalculateMana(EntityClass entityClass, int Level, int Intelligence) {
        int Mana = 0;

        switch(Level) {
            case 1:
                switch(entityClass) {
                    case EntityClass.Wizard:
                        Mana = 10 + Intelligence;
                        break;
                    case EntityClass.Fighter:
                        Mana = 6 + Intelligence;
                        break;
                    case EntityClass.Ranger:
                        Mana = 6 + Intelligence;
                        break;
                }
                break;
            default:
                switch(entityClass) {
                    case EntityClass.Wizard:
                        Mana = 6 + Intelligence;
                        break;
                    case EntityClass.Fighter:
                        Mana = 4 + Intelligence;
                        break;
                    case EntityClass.Ranger:
                        Mana = 4 + Intelligence;
                        break;
                }
                break;
        }

        return Mana;
    }

    public static int Strength(EntityClass entityClass) {
        int Strength = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Strength = 15;
                break;
            case EntityClass.Ranger:
                Strength = 9;
                break;
            case EntityClass.Wizard:
                Strength = 8;
                break;
        }
        return Strength;
    }

    public static int Dexterity(EntityClass entityClass) {
        int Dexterity = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Dexterity = 10;
                break;
            case EntityClass.Ranger:
                Dexterity = 15;
                break;
            case EntityClass.Wizard:
                Dexterity = 14;
                break;
        }
        return Dexterity;
    }

    public static int Constitution(EntityClass entityClass) {
        int Constitution = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Constitution = 15;
                break;
            case EntityClass.Ranger:
                Constitution = 13;
                break;
            case EntityClass.Wizard:
                Constitution = 14;
                break;
        }
        return Constitution;
    }

    public static int Intelligence(EntityClass entityClass) {
        int Intelligence = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Intelligence = 10;
                break;
            case EntityClass.Ranger:
                Intelligence = 11;
                break;
            case EntityClass.Wizard:
                Intelligence = 15;
                break;
        }
        return Intelligence;
    }

    public static int Wisdom(EntityClass entityClass) {
        int Wisdom = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Wisdom = 12;
                break;
            case EntityClass.Ranger:
                Wisdom = 15;
                break;
            case EntityClass.Wizard:
                Wisdom = 12;
                break;
        }
        return Wisdom;
    }

    public static int Charisma(EntityClass entityClass) {
        int Charisma = 0;
        switch(entityClass) {
            case EntityClass.Fighter:
                Charisma = 9;
                break;
            case EntityClass.Ranger:
                Charisma = 8;
                break;
            case EntityClass.Wizard:
                Charisma = 8;
                break;
        }
        return Charisma;
    }

    public static int CalculateModifier(int Level, int value) {
        int mod = 0;
        for(int i = 0; i < Level; i++) {
            mod = value + (i * 2);
        }

        return mod;
    }

    public static float ConvertSliderVal(int current, int max) {
        return current / max;
    }

    // public static void AnimateText(TextMeshProUGUI txtMesh, string sentence) {
    //     txtMesh.text = "";
    //     foreach(char letter in sentence.ToCharArray()) {
    //         txtMesh.text += letter;
    //     }
    // }
}