using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    [SerializeField] public GameObject MainUI;
    [SerializeField] public GameObject bg;
    [SerializeField] public GameObject choiceBox1;
    [SerializeField] public GameObject choiceBox2;
    [SerializeField] public GameObject choiceBox3;
    [SerializeField] public GameObject choiceBox4;

    void DisableUI() {
        MainUI.SetActive(true);
        bg.SetActive(false);
        this.gameObject.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);
    }
}
