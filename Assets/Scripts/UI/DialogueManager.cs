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
    [SerializeField] public GameObject Player;

    private Animator animator;

    private GameObject bg;
    private GameObject dgBox;
    private GameObject choiceBox1;
    private GameObject choiceBox2;
    private GameObject choiceBox3;
    private GameObject choiceBox4;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI choiceText1;
    private TextMeshProUGUI choiceText2;
    private TextMeshProUGUI choiceText3;
    private TextMeshProUGUI choiceText4;
    private Queue<string> sentences;
    private Queue<Choice> choice1;
    private Queue<Choice> choice2;
    private Queue<Choice> choice3;
    private Queue<Choice> choice4;
    public Queue<int> numChoices;

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
        choice3 = new Queue<Choice>();
        choice4 = new Queue<Choice>();
        numChoices = new Queue<int>();
    }

    public void StartDialogue(GameObject parent, Dialogue dialogue, Choice[] choice = null) {
        //Disable Main UI
        MainUI.SetActive(false);

        //Set Path for Dialogue
        bg = parent.transform.Find("Canvas").transform.Find("BG").gameObject;
        dgBox = parent.transform.Find("Canvas").transform.Find("DialogueBox").gameObject;
        choiceBox1 = parent.transform.Find("Canvas").transform.Find("ChoiceContainer").transform.Find("ChoiceBox1").gameObject;
        choiceBox2 = parent.transform.Find("Canvas").transform.Find("ChoiceContainer").transform.Find("ChoiceBox2").gameObject;
        choiceBox3 = parent.transform.Find("Canvas").transform.Find("ChoiceContainer").transform.Find("ChoiceBox3").gameObject;
        choiceBox4 = parent.transform.Find("Canvas").transform.Find("ChoiceContainer").transform.Find("ChoiceBox4").gameObject;

        animator = dgBox.GetComponent<Animator>();
        animator.SetBool("IsOpen",false);

        //Enable Main Dialogue
        bg.SetActive(true);
        dgBox.SetActive(true);

        //Main Dialogue Texts
        nameText = parent.transform.Find("Canvas").gameObject.transform.Find("DialogueBox").gameObject.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = parent.transform.Find("Canvas").gameObject.transform.Find("DialogueBox").gameObject.transform.Find("Border").gameObject.transform.Find("Base").gameObject.transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
        nameText.text = dialogue.names;

        //Clear queue
        sentences.Clear();

        //Enqueue new set of sentences
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        //Checks for Choices
        foreach(int order in dialogue.hasChoices) {
            numChoices.Enqueue(order);
        }

        //Enqueue choice
        if(choice != null) {
            switch(choice.Length) {
                case 2:
                    choice1.Enqueue(choice[0]);
                    choice2.Enqueue(choice[1]);
                    break;
                case 3:
                    choice1.Enqueue(choice[0]);
                    choice2.Enqueue(choice[1]);
                    choice3.Enqueue(choice[2]);
                    break;
                case 4:
                    choice1.Enqueue(choice[0]);
                    choice2.Enqueue(choice[1]);
                    choice3.Enqueue(choice[2]);
                    choice4.Enqueue(choice[3]);
                    break;
            }
        }

        DisplayNextSentence();

        //Check if first set of dialogue has a choice
        switch(numChoices.Peek()) {
            case 1:
                DisplayChoice();
                break;
            case 2:
                DisplayChoice();
                break;
            case 3:
                DisplayChoice3();
                break;
            case 4:
                DisplayChoice4();
                break;
            default:
                break;
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

        switch(numChoices.Peek()) {
            case 1:
                DisplayChoice();
                break;
            case 2:
                DisplayChoice();
                break;
            case 3:
                DisplayChoice3();
                break;
            case 4:
                DisplayChoice4();
                break;
            default:
                break;
        }

        numChoices.Dequeue();
    }

    private void DisplayChoice() {
        choiceBox1.SetActive(true);
        choiceBox2.SetActive(true);

        Choice tempchoice1 = choice1.Dequeue();
        Choice tempchoice2 = choice2.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeChoice(tempchoice1,tempchoice2));
    }

    private void DisplayChoice3() {
        choiceBox1.SetActive(true);
        choiceBox2.SetActive(true);
        choiceBox3.SetActive(true);

        Choice tempchoice1 = choice1.Dequeue();
        Choice tempchoice2 = choice2.Dequeue();
        Choice tempchoice3 = choice3.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeChoice3(tempchoice1,tempchoice2,tempchoice3));
    }

    private void DisplayChoice4() {
        choiceBox1.SetActive(true);
        choiceBox2.SetActive(true);
        choiceBox3.SetActive(true);
        choiceBox4.SetActive(true);

        Choice tempchoice1 = choice1.Dequeue();
        Choice tempchoice2 = choice2.Dequeue();
        Choice tempchoice3 = choice3.Dequeue();
        Choice tempchoice4 = choice4.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeChoice4(tempchoice1,tempchoice2,tempchoice3,tempchoice4));
    }
    IEnumerator TypeChoice(Choice choice1, Choice choice2) {
        choiceText1 = choiceBox1.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText2 = choiceBox2.transform.Find("Choice").GetComponent<TextMeshProUGUI>();

        choiceText1.text = "";
        choiceText2.text = "";

        choiceText1.text += "[ ";
        choiceText2.text += "[ ";

        foreach(char letter in choice1.choiceStat.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStat.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        choiceText1.text += " | ";
        choiceText2.text += " | ";

        foreach(char letter in choice1.choiceStatReq.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStatReq.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        choiceText1.text += " ] ";
        choiceText2.text += " ] ";

        foreach(char letter in choice1.sentences.ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.sentences.ToCharArray()) {
            choiceText2.text += letter;
        }

        yield return null;
    }

    IEnumerator TypeChoice3(Choice choice1, Choice choice2, Choice choice3) {
        choiceText1 = choiceBox1.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText2 = choiceBox2.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText3 = choiceBox3.transform.Find("Choice").GetComponent<TextMeshProUGUI>();

        choiceText1.text = "";
        choiceText2.text = "";
        choiceText3.text = "";

        choiceText1.text += "[ ";
        choiceText2.text += "[ ";
        choiceText3.text += "[ ";

        foreach(char letter in choice1.choiceStat.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStat.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.choiceStat.ToString().ToCharArray()) {
            choiceText3.text += letter;
        }


        choiceText1.text += " | ";
        choiceText2.text += " | ";
        choiceText3.text += " | ";

        foreach(char letter in choice1.choiceStatReq.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStatReq.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.choiceStatReq.ToString().ToCharArray()) {
            choiceText3.text += letter;
        }

        choiceText1.text += " ] ";
        choiceText2.text += " ] ";
        choiceText3.text += " ] ";

        foreach(char letter in choice1.sentences.ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.sentences.ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.sentences.ToCharArray()) {
            choiceText3.text += letter;
        }

        yield return null;
    }

    IEnumerator TypeChoice4(Choice choice1, Choice choice2, Choice choice3, Choice choice4) {
        choiceText1 = choiceBox1.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText2 = choiceBox2.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText3 = choiceBox3.transform.Find("Choice").GetComponent<TextMeshProUGUI>();
        choiceText4 = choiceBox4.transform.Find("Choice").GetComponent<TextMeshProUGUI>();

        choiceText1.text = "";
        choiceText2.text = "";
        choiceText3.text = "";
        choiceText4.text = "";

        choiceText1.text += "[ ";
        choiceText2.text += "[ ";
        choiceText3.text += "[ ";
        choiceText4.text += "[ ";

        foreach(char letter in choice1.choiceStat.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStat.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.choiceStat.ToString().ToCharArray()) {
            choiceText3.text += letter;
        }

        foreach(char letter in choice4.choiceStat.ToString().ToCharArray()) {
            choiceText4.text += letter;
        }

        choiceText1.text += " | ";
        choiceText2.text += " | ";
        choiceText3.text += " | ";
        choiceText4.text += " | ";

        foreach(char letter in choice1.choiceStatReq.ToString().ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.choiceStatReq.ToString().ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.choiceStatReq.ToString().ToCharArray()) {
            choiceText3.text += letter;
        }

        foreach(char letter in choice4.choiceStatReq.ToString().ToCharArray()) {
            choiceText4.text += letter;
        }

        choiceText1.text += " ] ";
        choiceText2.text += " ] ";
        choiceText3.text += " ] ";
        choiceText4.text += " ] ";

        foreach(char letter in choice1.sentences.ToCharArray()) {
            choiceText1.text += letter;
        }

        foreach(char letter in choice2.sentences.ToCharArray()) {
            choiceText2.text += letter;
        }

        foreach(char letter in choice3.sentences.ToCharArray()) {
            choiceText3.text += letter;
        }

        foreach(char letter in choice4.sentences.ToCharArray()) {
            choiceText4.text += letter;
        }

        yield return null;
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen",true);

        MainUI.SetActive(true);
        bg.SetActive(false);
        // dgBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);
    }
}
