using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public Collider dialogueOther;

    void OnTriggerStay(Collider other) {
        if(other.name == "DialogueInteract") {
            dialogueOther = other;
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
        dialogueOther = null;
        other.transform.Find("Context").gameObject.SetActive(false);
    }

    public void PrepDialogueInteract() {
        if(dialogueOther != null) {
            dialogueOther.GetComponent<DialogueTrigger>().InteractDialogue();
        }
        else Debug.Log("DialogueTrigger not found.");   
    }
}
