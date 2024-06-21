using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public Choice[] choice;

    public void InteractDialogue() {
        DialogueManager.Instance.StartDialogue(this.gameObject, dialogue, choice);
    }

    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(this.gameObject, dialogue, choice);
    }
}