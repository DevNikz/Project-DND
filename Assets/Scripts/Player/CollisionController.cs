using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [Header("Area")]
    public GameObject areaOther;
    public GameObject returnAreaOther;

    [Header("Dialogue")]
    public GameObject dialogueOther;

    void OnTriggerStay(Collider other) {
        //Area
        if(other.CompareTag("Area")) {
            areaOther = other.gameObject;
        }

        if(other.CompareTag("ReturnArea")) {
            returnAreaOther = other.gameObject;
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
    }

    void OnTriggerExit(Collider other) {
        if(dialogueOther != null) dialogueOther = null;
        if(areaOther != null) areaOther = null;
        if(returnAreaOther != null) returnAreaOther = null;

        if(other.transform.Find("Context") != null) other.transform.Find("Context").gameObject.SetActive(false);
    }

    public void PrepDialogueInteract() {
        if(dialogueOther != null) {
            dialogueOther.GetComponent<DialogueTrigger>().InteractDialogue();
        }
    }

    public void EnterArea() {
        if(areaOther != null) {
            areaOther.GetComponent<AreaController>().Teleport();
        }
    }

    public void ReturnArea() {
        if(returnAreaOther != null) {
            returnAreaOther.GetComponent<AreaController>().Return();
        }
    }
}
