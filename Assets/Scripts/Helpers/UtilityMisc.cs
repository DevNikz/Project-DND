using Unity.VisualScripting;
using UnityEngine;

public class UtilityMisc : MonoBehaviour{
    public static int CalculateHealth(EntityClass entityClass, int Level, int Constitution) {
        int Health = 0;
        switch(Level) {
            case 1:
                switch(entityClass) {
                    case EntityClass.WIZARD:
                        Health = 6 + Constitution;
                        break;
                    case EntityClass.FIGHTER:
                        Health = 10 + Constitution;
                        break;
                    case EntityClass.RANGER:
                        Health = 10 + Constitution;
                        break;
                }
                break;
            default:
                switch(entityClass) {
                    case EntityClass.WIZARD:
                        Health = 4 + Constitution;
                        break;
                    case EntityClass.FIGHTER:
                        Health = 6 + Constitution;
                        break;
                    case EntityClass.RANGER:
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
                    case EntityClass.WIZARD:
                        Mana = 10 + Intelligence;
                        break;
                    case EntityClass.FIGHTER:
                        Mana = 6 + Intelligence;
                        break;
                    case EntityClass.RANGER:
                        Mana = 6 + Intelligence;
                        break;
                }
                break;
            default:
                switch(entityClass) {
                    case EntityClass.WIZARD:
                        Mana = 6 + Intelligence;
                        break;
                    case EntityClass.FIGHTER:
                        Mana = 4 + Intelligence;
                        break;
                    case EntityClass.RANGER:
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
            case EntityClass.FIGHTER:
                Strength = 15;
                break;
            case EntityClass.RANGER:
                Strength = 9;
                break;
            case EntityClass.WIZARD:
                Strength = 8;
                break;
        }
        return Strength;
    }

    public static int Dexterity(EntityClass entityClass) {
        int Dexterity = 0;
        switch(entityClass) {
            case EntityClass.FIGHTER:
                Dexterity = 10;
                break;
            case EntityClass.RANGER:
                Dexterity = 15;
                break;
            case EntityClass.WIZARD:
                Dexterity = 14;
                break;
        }
        return Dexterity;
    }

    public static int Constitution(EntityClass entityClass) {
        int Constitution = 0;
        switch(entityClass) {
            case EntityClass.FIGHTER:
                Constitution = 15;
                break;
            case EntityClass.RANGER:
                Constitution = 13;
                break;
            case EntityClass.WIZARD:
                Constitution = 14;
                break;
        }
        return Constitution;
    }

    public static int Intelligence(EntityClass entityClass) {
        int Intelligence = 0;
        switch(entityClass) {
            case EntityClass.FIGHTER:
                Intelligence = 10;
                break;
            case EntityClass.RANGER:
                Intelligence = 11;
                break;
            case EntityClass.WIZARD:
                Intelligence = 15;
                break;
        }
        return Intelligence;
    }

    public static int Wisdom(EntityClass entityClass) {
        int Wisdom = 0;
        switch(entityClass) {
            case EntityClass.FIGHTER:
                Wisdom = 12;
                break;
            case EntityClass.RANGER:
                Wisdom = 15;
                break;
            case EntityClass.WIZARD:
                Wisdom = 12;
                break;
        }
        return Wisdom;
    }

    public static int Charisma(EntityClass entityClass) {
        int Charisma = 0;
        switch(entityClass) {
            case EntityClass.FIGHTER:
                Charisma = 9;
                break;
            case EntityClass.RANGER:
                Charisma = 8;
                break;
            case EntityClass.WIZARD:
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
}