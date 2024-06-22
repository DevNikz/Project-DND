using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] public GameObject MainUI;
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Dice;

    public int diceRoll;
    public bool stop;

    private Animator animator;

    private GameObject bg;
    private GameObject dgBox;
    private GameObject outBox;
    private GameObject nextBtn;
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

    //Temp
    private GameObject tempParent;

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

    void Update() {
        diceRoll = DiceData.diceRoll;
        stop = DiceData.stop;
    }

    public void StartDialogue(GameObject parent, Dialogue dialogue, Choice[] choice = null) {
        if(parent.GetComponent<DialogueTrigger>().outcomeFinish == true) {
            outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;

            MainUI.SetActive(false);

            bg = parent.transform.Find("Canvas").transform.Find("BG").gameObject;
            bg.SetActive(true);

            outBox.SetActive(true);

            TextMeshProUGUI outcomeSentence = outBox.transform.Find("Border").transform.Find("Base").transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
            string tempText = outcomeSentence.text;
            outcomeSentence.text = "";
            foreach(char letter in tempText.ToCharArray()) {
                outcomeSentence.text += letter;
            }
        }

        else {
            //temp
            tempParent = parent;

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
            
            nextBtn = parent.transform.Find("Canvas").gameObject.transform.Find("DialogueBox").gameObject.transform.Find("Border").gameObject.transform.Find("Base").gameObject.transform.Find("Button").gameObject;
            nameText.text = dialogue.names;

            nextBtn.SetActive(true);

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
                    nextBtn.SetActive(false);
                    DisplayChoice();
                    break;
                case 2:
                    nextBtn.SetActive(false);
                    DisplayChoice();
                    break;
                case 3:
                    nextBtn.SetActive(false);
                    DisplayChoice3();
                    break;
                case 4:
                    nextBtn.SetActive(false);
                    DisplayChoice4();
                    break;
                default:
                    break;
            }
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
                nextBtn.SetActive(false);
                DisplayChoice();
                break;
            case 2:
                nextBtn.SetActive(false);
                DisplayChoice();
                break;
            case 3:
                nextBtn.SetActive(false);
                DisplayChoice3();
                break;
            case 4:
                nextBtn.SetActive(false);
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

        choiceBox1.GetComponent<ChoiceController>().UpdateChoice(tempchoice1.choiceStat, tempchoice1.choiceStatReq);
        choiceBox2.GetComponent<ChoiceController>().UpdateChoice(tempchoice2.choiceStat, tempchoice2.choiceStatReq);

        choiceBox1.GetComponent<ChoiceController>().UpdateSuccess(tempchoice1.SuccessDialogue.name, tempchoice1.SuccessDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateSuccess(tempchoice2.SuccessDialogue.name, tempchoice2.SuccessDialogue.sentences);

        choiceBox1.GetComponent<ChoiceController>().UpdateFailed(tempchoice1.FailDialogue.name, tempchoice1.FailDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateFailed(tempchoice2.FailDialogue.name, tempchoice2.FailDialogue.sentences);


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

        choiceBox1.GetComponent<ChoiceController>().UpdateChoice(tempchoice1.choiceStat, tempchoice1.choiceStatReq);
        choiceBox2.GetComponent<ChoiceController>().UpdateChoice(tempchoice2.choiceStat, tempchoice2.choiceStatReq);
        choiceBox3.GetComponent<ChoiceController>().UpdateChoice(tempchoice3.choiceStat, tempchoice3.choiceStatReq);

        choiceBox1.GetComponent<ChoiceController>().UpdateSuccess(tempchoice1.SuccessDialogue.name, tempchoice1.SuccessDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateSuccess(tempchoice2.SuccessDialogue.name, tempchoice2.SuccessDialogue.sentences);
        choiceBox3.GetComponent<ChoiceController>().UpdateSuccess(tempchoice3.SuccessDialogue.name, tempchoice3.SuccessDialogue.sentences);

        choiceBox1.GetComponent<ChoiceController>().UpdateFailed(tempchoice1.FailDialogue.name, tempchoice1.FailDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateFailed(tempchoice2.FailDialogue.name, tempchoice2.FailDialogue.sentences);
        choiceBox3.GetComponent<ChoiceController>().UpdateFailed(tempchoice3.FailDialogue.name, tempchoice3.FailDialogue.sentences);

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

        choiceBox1.GetComponent<ChoiceController>().UpdateChoice(tempchoice1.choiceStat, tempchoice1.choiceStatReq);
        choiceBox2.GetComponent<ChoiceController>().UpdateChoice(tempchoice2.choiceStat, tempchoice2.choiceStatReq);
        choiceBox3.GetComponent<ChoiceController>().UpdateChoice(tempchoice3.choiceStat, tempchoice3.choiceStatReq);
        choiceBox4.GetComponent<ChoiceController>().UpdateChoice(tempchoice4.choiceStat, tempchoice4.choiceStatReq);

        choiceBox1.GetComponent<ChoiceController>().UpdateSuccess(tempchoice1.SuccessDialogue.name, tempchoice1.SuccessDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateSuccess(tempchoice2.SuccessDialogue.name, tempchoice2.SuccessDialogue.sentences);
        choiceBox3.GetComponent<ChoiceController>().UpdateSuccess(tempchoice3.SuccessDialogue.name, tempchoice3.SuccessDialogue.sentences);
        choiceBox4.GetComponent<ChoiceController>().UpdateSuccess(tempchoice4.SuccessDialogue.name, tempchoice4.SuccessDialogue.sentences);

        choiceBox1.GetComponent<ChoiceController>().UpdateFailed(tempchoice1.FailDialogue.name, tempchoice1.FailDialogue.sentences);
        choiceBox2.GetComponent<ChoiceController>().UpdateFailed(tempchoice2.FailDialogue.name, tempchoice2.FailDialogue.sentences);
        choiceBox3.GetComponent<ChoiceController>().UpdateFailed(tempchoice3.FailDialogue.name, tempchoice3.FailDialogue.sentences);
        choiceBox4.GetComponent<ChoiceController>().UpdateFailed(tempchoice4.FailDialogue.name, tempchoice4.FailDialogue.sentences);

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
        dgBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);
    }

    public void EnableDice() {
        Debug.Log("Loading Dice Roll Scene");
        Canvas canvas = tempParent.transform.Find("Canvas").GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        SceneManager.LoadSceneAsync("DiceRoll",LoadSceneMode.Additive);
    }

    public void CheckOutcome(bool outcome) {
        if(PlayerManager.Instance.alwaysSucceed == true && PlayerManager.Instance.alwaysFail == false) {
            SetSuccessOutcome();
        }
        else if(PlayerManager.Instance.alwaysSucceed == false && PlayerManager.Instance.alwaysFail == true) {
            SetFailedOutcome();
        }
        else {
            switch(outcome) {
                case true:
                    SetSuccessOutcome();
                    break;
                case false:
                    SetFailedOutcome();
                    break;
            }
        }
    }

    void SetSuccessOutcome() {
        Debug.Log("Unload Dice");
        Canvas canvas = tempParent.transform.Find("Canvas").GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        dgBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);

        //Set Dir
        outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;
        TextMeshProUGUI outcomeName = outBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();

        //Set Vars
        outcomeName.text = DiceData.name_Success;
        StopAllCoroutines();
        StartCoroutine(TypeOutcomeSuccess());
        
        //Set Active
        outBox.SetActive(true);
    }

    void SetFailedOutcome() {
        Debug.Log("Unload Dice");
        Canvas canvas = tempParent.transform.Find("Canvas").GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        dgBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);

        //Set Dir
        outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;
        TextMeshProUGUI outcomeName = outBox.transform.Find("Name").GetComponent<TextMeshProUGUI>();

        //Set Vars
        outcomeName.text = DiceData.name_Failed;
        StopAllCoroutines();
        StartCoroutine(TypeOutcomeFailed());
        
        //Set Active
        outBox.SetActive(true);
    }

    IEnumerator TypeOutcomeSuccess() {
        outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;
        TextMeshProUGUI outcomeSentence = outBox.transform.Find("Border").transform.Find("Base").transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
        outcomeSentence.text = "";
        foreach(char letter in DiceData.outcomeSentence_Success.ToString().ToCharArray()) {
            outcomeSentence.text += letter;
            yield return null;
        }
    }

    IEnumerator TypeOutcomeFailed() {
        outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;
        TextMeshProUGUI outcomeSentence = outBox.transform.Find("Border").transform.Find("Base").transform.Find("Sentence").GetComponent<TextMeshProUGUI>();
        outcomeSentence.text = "";
        foreach(char letter in DiceData.outcomeSentence_Failed.ToString().ToCharArray()) {
            outcomeSentence.text += letter;
            yield return null;
        }
    }

    public void EndOutcome() {
        outBox = tempParent.transform.Find("Canvas").transform.Find("OutcomeBox").gameObject;

        tempParent.GetComponent<DialogueTrigger>().outcomeFinish = true;

        animator.SetBool("IsOpen",true);

        MainUI.SetActive(true);
        bg.SetActive(false);
        dgBox.SetActive(false);
        outBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        choiceBox3.SetActive(false);
        choiceBox4.SetActive(false);
    }
}
