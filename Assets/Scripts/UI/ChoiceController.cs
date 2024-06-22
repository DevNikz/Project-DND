using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceController : MonoBehaviour
{
    public Choice choice;
    public Stats stat;

    public void UpdateChoice(Stats stat, int statReq, string sentences) {
        choice.choiceStat = stat;
        choice.choiceStatReq = statReq;
        choice.sentences = sentences;
    }

    public void RollDice() {
        //Save Data
        DiceData.choiceStat = choice.choiceStat;
        DiceData.choiceStatReq = choice.choiceStatReq;

        //Activate RollDice Prefab
        DialogueManager.Instance.EnableDice();
    }

    void Update() {
        //Condition

        if(DialogueManager.Instance.stop == true) {
            //DisableHUD
            this.CheckRoll();
            SceneManager.UnloadSceneAsync("DiceRoll",UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            DialogueManager.Instance.DisableDice();
        }
    }   

    public void CheckRoll() {
        Debug.Log(DiceData.choiceStat);
        switch(DiceData.choiceStat) {
            case Stats.STR:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Strength Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Strength Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            case Stats.DEX:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Dexterity Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Dexterity Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            case Stats.CON:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Constitution Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Constitution Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            case Stats.INT:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Intelligence Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Intelligence Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            case Stats.WIS:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Wisdom Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Wisdom Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            case Stats.CHA:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Charisma Check Success!");
                    DialogueManager.Instance.EndDialogue();
                }

                else {
                    Debug.Log("Charisma Check Failed");
                    DialogueManager.Instance.EndDialogue();
                }
                break;
            default:
                break;
        } 
    }
}
