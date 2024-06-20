using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] public GameObject MainUI;

    private GameObject bg;
    private GameObject dgBox;
    private GameObject choiceBox1;
    private GameObject choiceBox2;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI choiceText1;
    private TextMeshProUGUI choiceText2;
    private Queue<string> sentences;
    private Queue<Choice> choice1;
    private Queue<Choice> choice2;
    public Queue<bool> orders;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start(){
        sentences = new Queue<string>();
        choice1 = new Queue<Choice>();
        choice2 = new Queue<Choice>();
        orders = new Queue<bool>();
    }

    public void StartDialogue(GameObject parent, Dialogue dialogue) {
        //Disable Main UI
        MainUI.SetActive(false);

        //Set Path for Dialogue
        bg = parent.transform.Find("Canvas").transform.Find("BG").gameObject;
        dgBox = parent.transform.Find("Canvas").transform.Find("DialogueBox").gameObject;
        choiceBox1 = parent.transform.Find("Canvas").transform.Find("ChoiceBox1").gameObject;
        choiceBox2 = parent.transform.Find("Canvas").transform.Find("ChoiceBox2").gameObject;

        //Enable Main Dialogue
        bg.SetActive(true);
        dgBox.SetActive(true);

        //Main Dialogue Texts
        nameText = parent.transform.Find("Canvas").gameObject.transform.Find("DialogueBox").gameObject.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = parent.transform.Find("Canvas").gameObject.transform.Find("DialogueBox").gameObject.transform.Find("Border").gameObject.transform.Find("Base").gameObject.transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
        nameText.text = dialogue.name;

        //Clear queue
        sentences.Clear();

        //Enqueue new set of sentences
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        //Checks for Choices
        foreach(bool order in dialogue.orders) {
            orders.Enqueue(order);
        }

        //Enqueue choice
        if(dialogue.choice != null) {
            choice1.Enqueue(dialogue.choice[0]);
            choice2.Enqueue(dialogue.choice[1]);
        }

        DisplayNextSentence();

        //Check if first set of dialogue has a choice
        if(orders.Peek() == true) {
            choiceBox1.SetActive(true);
            choiceBox2.SetActive(true);
            DisplayChoice();
        }
    }

    public void DisplayNextSentence() {
        
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }

        //Check if next set of dialogue has a choice
        if(orders.Peek() == true) {
            choiceBox1.SetActive(true);
            choiceBox2.SetActive(true);
            DisplayChoice();
        }

        orders.Dequeue();
    }

    private void DisplayChoice() {
        Choice tempchoice1 = choice1.Dequeue();
        Choice tempchoice2 = choice2.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeChoice(tempchoice1,tempchoice2));
    }

    IEnumerator TypeChoice(Choice choice1, Choice choice2) {
        choiceText1 = choiceBox1.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText2 = choiceBox2.transform.Find("Choice").GetComponent<TextMeshProUGUI>();

        choiceText1.text = "";
        choiceText2.text = "";

        choiceText1.text += "[";
        choiceText2.text += "[";

        foreach(char letter in choice1.choiceStat.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStat.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        choiceText1.text += "|";
        choiceText2.text += "|";

        foreach(char letter in choice1.choiceStatReq.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStatReq.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        choiceText1.text += "]";
        choiceText2.text += "]";

        choiceText1.text += " ";
        choiceText2.text += " ";

        foreach(char letter in choice1.sentences.ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.sentences.ToCharArray()) {
            choiceText2.text += letter;
        }

        yield return null;
    }


    private void EndDialogue() {
        MainUI.SetActive(true);
        bg.SetActive(false);
        dgBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
    }
}
