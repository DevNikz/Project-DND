using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public Collider dialogueOther;

    void OnTriggerStay(Collider other) {
        if(other.name == "D") {
            dialogueOther = other;
            // other.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    void OnTriggerExit(Collider other) {
        dialogueOther = null;
    }

    public void PrepDialogueInteract() {
        if(dialogueOther != null) {
            dialogueOther.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else Debug.Log("DialogueTrigger not found.");
    }
}
