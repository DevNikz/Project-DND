using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [Header("Area")]
    public GameObject area;
    public GameObject returnArea;
    public GameObject returnAreaSided;
    public GameObject save;

    [Header("Dialogue")]
    public GameObject dialogueOther;

    void OnTriggerStay(Collider other) {
        //Area
        if(other.CompareTag("Area")) {
            area = other.gameObject;
        }

        if(other.CompareTag("ReturnArea")) {
            returnArea = other.gameObject;
        }

        if(other.CompareTag("ReturnAreaSided")) {
            returnAreaSided = other.gameObject;
        }

        if(other.CompareTag("Save")) {
            save = other.gameObject;
        }

        //Dialogue
        if(other.name == "DialogueInteract") {
            dialogueOther = other.gameObject;
            other.transform.Find("Context").gameObject.SetActive(true);
        }

        if(other.name == "DialogueTrigger") {
            this.gameObject.GetComponent<PlayerMovement>().joystick.input = Vector2.zero;
            this.gameObject.GetComponent<PlayerMovement>().joystick.handle.anchoredPosition = Vector2.zero;
            other.GetComponent<DialogueTrigger>().TriggerDialogue();
            Destroy(other);
        }

        //Combat
        if(other.name == "CombatTrigger") {
            this.gameObject.GetComponent<PlayerMovement>().joystick.input = Vector2.zero;
            this.gameObject.GetComponent<PlayerMovement>().joystick.handle.anchoredPosition = Vector2.zero;
            other.GetComponent<CombatTrigger>().TriggerCombat();
            Destroy(other);
        }
    }

    void OnTriggerExit(Collider other) {
        if(dialogueOther != null) dialogueOther = null;
        if(area != null) area = null;
        if(returnArea != null) returnArea = null;
        if(returnAreaSided != null) returnAreaSided = null;
        if(save != null) save = null;

        if(other.transform.Find("Context") != null) other.transform.Find("Context").gameObject.SetActive(false);
    }

    public void PrepDialogueInteract() {
        if(dialogueOther != null) {
            dialogueOther.GetComponent<DialogueTrigger>().InteractDialogue();
        }
    }

    public void EnterArea() {
        if(area != null) {
            area.GetComponent<AreaController>().Teleport();
        }
    }

    public void ReturnArea() {
        if(returnArea != null) {
            returnArea.GetComponent<AreaController>().Return();
        }
        if(returnAreaSided != null) {
            returnAreaSided.GetComponent<AreaController>().ReturnSided();
        }
    }

    public void Save() {
        if(save != null) {
            PlayerManager.Instance.SavePlayer();
        }
    }
}
