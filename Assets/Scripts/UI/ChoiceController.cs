using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceController : MonoBehaviour
{
    public Choice choice;
    public Outcome success;
    public Outcome failed;
    public Stats stat;

    public void UpdateChoice(Stats stat, int statReq) {
        choice.choiceStat = stat;
        choice.choiceStatReq = statReq;
    }

    public void UpdateSuccess(string nameSuccess, string sentencesSuccess) {
        success.name = nameSuccess;
        success.sentences = sentencesSuccess;
    }

    public void UpdateFailed(string nameFailed, string sentencesFailed) {
        failed.name = nameFailed;
        failed.sentences = sentencesFailed;
    }

    public void RollDice() {
        //Save Data | Choice
        DiceData.choiceStat = choice.choiceStat;
        DiceData.choiceStatReq = choice.choiceStatReq;

        //Save Data | Success
        DiceData.name_Success = success.name;
        DiceData.outcomeSentence_Success = success.sentences;

        //Save Data | Failure
        DiceData.name_Failed = failed.name;
        DiceData.outcomeSentence_Failed = failed.sentences;

        //Activate RollDice Prefab
        DialogueManager.Instance.EnableDice();
    }

    void Update() {
        //Condition

        if(DialogueManager.Instance.stop == true) {
            //DisableHUD
            SceneManager.UnloadSceneAsync("DiceRoll",UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            //DialogueManager.Instance.DisableDice();
            this.CheckRoll();
        }
    }   

    public void CheckRoll() {
        switch(DiceData.choiceStat) {
            case Stats.STR:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Strength Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Strength Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            case Stats.DEX:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Dexterity Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Dexterity Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            case Stats.CON:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Constitution Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Constitution Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            case Stats.INT:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Intelligence Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Intelligence Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            case Stats.WIS:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Wisdom Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Wisdom Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            case Stats.CHA:
                if(DiceData.diceRoll >= DiceData.choiceStatReq) {
                    Debug.Log("Charisma Check Success!");
                    DialogueManager.Instance.CheckOutcome(true);
                }

                else {
                    Debug.Log("Charisma Check Failed");
                    DialogueManager.Instance.CheckOutcome(false);
                }
                break;
            default:
                break;
        } 
    }
}
