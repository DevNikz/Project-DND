using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {
    public static CombatManager Instance;

    [Header("Properties")] 
    public CombatState combatState;

    private GameObject Camera;
    private GameObject MainUI;

    [Header("Reference")]
    public GameObject CurrentArea;
    public GameObject CurrentBattleUI;
    private TextMeshProUGUI dialogueText;
    private GameObject MainButtons;
    private GameObject SideButtons;
    private GameObject SkillButtons;
    private GameObject TargetButtons;
    private GameObject UseButton;
    private GameObject refArea;
    private List<Character> refCharacters;
    private List<Enemy> refEnemies;
    public int SkillIndex;
    public int TargetIndex;


    public enum CombatState { NONE, START, PLAYERTURN, COMPANIONTURN1, COMPANIONTURN2, ENEMYTURN, WIN, LOST }

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    void Start() {
        Camera = GameObject.Find("/MainCamera");
        MainUI = GameObject.Find("/MainUI");
        combatState = CombatState.NONE;
    }

    public void StartCombat(GameObject area, List<Character> characters, List<Enemy> enemies) {
        refArea = area;
        refCharacters = characters;
        refEnemies = enemies;
        StartCoroutine(InitArea());
    }

    IEnumerator InitArea() {
        combatState = CombatState.START;
        SetupArea();
        SetupDialogue();
        
        yield return new WaitForSeconds(1);
        PlayerTurn();
    }


    void SetupArea() {
        //Disable Main Camera
        Camera.SetActive(false);

        //Disable Main UI
        MainUI.SetActive(false);

        //Init BattleArea
        CurrentArea = GameObject.Find($"/Battle{refArea.name}");
        CurrentArea.transform.Find("Camera").gameObject.SetActive(true);
        CurrentArea.transform.Find("Base").gameObject.SetActive(true);
        CurrentArea.transform.Find("Characters").gameObject.SetActive(true);
        CurrentBattleUI = CurrentArea.transform.Find("UI").gameObject;
        CurrentBattleUI.SetActive(true);

        dialogueText = CurrentBattleUI.transform.Find("Main/Base/Text (TMP)").GetComponent<TextMeshProUGUI>();
        MainButtons = CurrentBattleUI.transform.Find("Main/MainButtons").gameObject;
        SideButtons = CurrentBattleUI.transform.Find("Main/SideButtons").gameObject;
        SkillButtons = CurrentBattleUI.transform.Find("Main/Skills").gameObject;
        TargetButtons = CurrentBattleUI.transform.Find("Main/Targets").gameObject;
        //ItemButtons = CurrentBattleUI.transform.Find("Main/Items").gameObject;
        UseButton = SideButtons.transform.Find("Use").gameObject;

        MainButtons.SetActive(true);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        TargetButtons.SetActive(false);

        //Set Unit
        for(int i = 0; i < refCharacters.Count; i++) {
            Debug.Log(refCharacters[i].Name);
            switch(i) {
                case 0: // Player
                    //Set Sprite
                    CurrentArea.transform.Find("Characters/Player").gameObject.SetActive(true);
                    CurrentArea.transform.Find("Characters/Player").GetComponent<Unit>().LoadUnit($"{refCharacters[i].Class}Player", true);
                    //Set UI
                    CurrentBattleUI.transform.Find("Characters/Friendly/Player").gameObject.SetActive(true);
                    CurrentBattleUI.transform.Find("Characters/Friendly/Player").GetComponent<Unit>().LoadPlayer(refCharacters[i]);
                    break;
                default:
                    CurrentArea.transform.Find($"Characters/Companion{i}").gameObject.SetActive(true);
                    CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<Unit>().LoadUnit(refCharacters[i].Name, true);

                    CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").gameObject.SetActive(true);
                    CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().LoadCompanion(refCharacters[i]);
                    break;
            }
        }

        for(int i = 0; i < refEnemies.Count; i++) {
            CurrentArea.transform.Find($"Characters/Enemy{i}").gameObject.SetActive(true);
            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<Unit>().LoadUnit($"{refEnemies[i].Name}{refEnemies[i].Class}", true);

            CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{i}").gameObject.SetActive(true);
            CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{i}").GetComponent<Unit>().LoadEnemy(refEnemies[i], true);
        }
    }

    void SetupDialogue() {
        if(refEnemies.Count == 1) {
            string text = $"A {refEnemies[0].Name} Appeared...";
            StartCoroutine(TypeSentence(text));
        }
        else {
            string text = $"{refEnemies.Count} {refEnemies[0].Name} Appeared..."; 
            StartCoroutine(TypeSentence(text));
        }
        
    }

    void PlayerTurn() {
        string text = "Choose An Action";
        StartCoroutine(TypeSentence(text));

        combatState = CombatState.PLAYERTURN;
    }

    //Attack Button 
    public void OnAttackButton() {
        if (combatState == CombatState.PLAYERTURN || combatState == CombatState.COMPANIONTURN1 || combatState == CombatState.COMPANIONTURN2 ) {
            StartCoroutine(SetupAttack());
        }
    }

    public void OnUseButton() {
        switch(combatState) {
            case CombatState.PLAYERTURN:
                if(refCharacters[0].Skills[SkillIndex].Category == SkillCategory.All) {
                    StartCoroutine(ActivateSkill());
                }
                else {
                    StartCoroutine(SetupTarget());
                }
                break;
        }
        //StartCoroutine(ActivateSkill());
    }

    public void OnEscapeButton() {
        Escape();
    }

    IEnumerator SetupAttack() {
        string text = "Loading Skills...";
        StartCoroutine(TypeSentence(text));

        yield return new WaitForSeconds(1);

        dialogueText.gameObject.SetActive(false);
        MainButtons.SetActive(false);
        SideButtons.SetActive(true);
        SkillButtons.SetActive(true);
        UseButton.SetActive(false);

        SetupSkills();
    }

    void SetupSkills() {
        switch(combatState) {
            case CombatState.PLAYERTURN:
                for(int i = 0; i < refCharacters[0].Skills.Count; i++) {
                    var skill = SkillButtons.transform.Find($"Skill{i}/Text").GetComponent<TextMeshProUGUI>();
                    if(skill == null) Debug.Log("error");
                    else {
                        skill.transform.parent.gameObject.SetActive(true);
                        skill.transform.parent.GetComponent<SkillController>().SetIndex(i);
                        skill.text = refCharacters[0].Skills[i].Type.ToString();
                        skill.transform.parent.GetComponent<Button>().onClick.AddListener(() => ButtonSelection());
                    }
                } 
                break;
        }
    }

    IEnumerator SetupTarget() {
        UseButton.SetActive(false);
        SkillButtons.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        string text = "Loading Targets...";
        StartCoroutine(TypeSentence(text));

        yield return new WaitForSeconds(1);

        dialogueText.gameObject.SetActive(false);
        TargetButtons.SetActive(true);

        SelectTargetEntity();
    }

    void SelectTargetEntity() {
        switch(combatState) {
            case CombatState.PLAYERTURN:
                for(int i = 0; i < refEnemies.Count; i++) {
                    var target = TargetButtons.transform.Find($"Target{i}/Text").GetComponent<TextMeshProUGUI>();
                    if(target == null) Debug.Log("Error");
                    else {
                        target.transform.parent.gameObject.SetActive(true);
                        target.transform.parent.GetComponent<TargetController>().SetTarget(i);
                        target.text = refEnemies[i].Name.ToString();
                        target.transform.parent.GetComponent<Button>().onClick.AddListener(() => TargetSelection());
                    }
                } 
                break;
        }
    }

    void ButtonSelection() {
        switch(combatState) {
            case CombatState.PLAYERTURN:
                //Reset Selected
                for(int i = 0; i < refCharacters[0].Skills.Count; i++) {
                    if(SkillIndex == i) {
                        SkillButtons.transform.Find($"Skill{SkillIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    }
                    else SkillButtons.transform.Find($"Skill{i}").GetComponent<Image>().color = Color.white;
                }

                //Skill
                if(refCharacters[0].Skills[SkillIndex].Category == SkillCategory.All) {
                    for(int i = 0; i < refEnemies.Count; i++) {
                        CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                    }
                }
                else {
                    for(int i = 0; i < refEnemies.Count; i++) {
                        CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }
                } 
                break;
        }
        UseButton.SetActive(true);
    }

    void TargetSelection() {
        for(int i = 0; i < refEnemies.Count; i++) {
            if(TargetIndex == i) {
                CurrentArea.transform.Find($"Characters/Enemy{TargetIndex}").GetComponent<OutlineEntity>().HighlightUnit(true);
                TargetButtons.transform.Find($"Target{TargetIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            }
            else {
                CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                TargetButtons.transform.Find($"Target{i}").GetComponent<Image>().color = Color.white;
            }
        }
        UseButton.SetActive(true);
    }

    IEnumerator ActivateSkill() {
        SkillButtons.SetActive(false);
        SideButtons.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        switch(combatState) {
            case CombatState.PLAYERTURN:
                string text = $"{refCharacters[0].Name} Used {refCharacters[0].Skills[SkillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                //Receive Damage
                refCharacters[0].CurrentMana -= refCharacters[0].Skills[SkillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(refCharacters[0].CurrentMana);

                //Set Attack (All)
                if(refCharacters[0].Skills[SkillIndex].Category == SkillCategory.All) {
                    if(CheckType(refCharacters[0].Skills[SkillIndex].Type)) SkillAllFriendly_Offensive(refEnemies, refCharacters[0].Skills[SkillIndex].ActualModifier);
                    else {
                        SkillAllFriendly_Support(refCharacters, refCharacters[0].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[0].Skills[SkillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refCharacters[0].Skills[SkillIndex].Type)) SkillFriendly_Offensive(refEnemies[TargetIndex], refCharacters[0].Skills[SkillIndex].ActualModifier);
                    else {
                        SkillFriendly_Support(refCharacters[TargetIndex], refCharacters[0].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[0].Skills[SkillIndex].Type));
                    }
                }

                if(CheckEnemyCount()) {
                    Debug.Log("No Enemies Left.");
                }
                
                else {
                    Debug.Log($"Enemies Left: {refEnemies.Count}" );
                }
                break;
        }

        
        yield return new WaitForSeconds(1);

        
        //Switch combat state to next
    }

    bool CheckEnemyCount() {
        int deadCounter = 0;
        for(int i = 0; i < refEnemies.Count; i++) {
            if(refEnemies[i].CurrentHealth == 0) {
                CurrentArea.transform.Find($"Characters/Enemy{i}").gameObject.SetActive(false);
                deadCounter += 1;
            }
        }

        if(deadCounter == refEnemies.Count) return true;
        else return false;
    }

    void SkillAllFriendly_Offensive(List<Enemy> enemy, int value) {
        for(int i = 0; i < enemy.Count; i++) {
            enemy[i].CurrentHealth -= value;
            CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{i}").GetComponent<Unit>().UpdateHP(enemy[i].CurrentHealth);
        }
    }

    void SkillAllFriendly_Support(List<Character> friendly, int value, bool type) {
        switch(type) {
            case true: //Heal HP
                for(int i = 0; i < friendly.Count; i++) {
                    friendly[i].CurrentHealth += value;
                    if(i == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly[0].CurrentHealth);
                    else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateHP(friendly[i].CurrentHealth);
                }
                break;
            case false: //Heal MP
                for(int i = 0; i < friendly.Count; i++) {
                    friendly[i].CurrentMana += value;
                    if(i == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(friendly[0].CurrentMana);
                    else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateMP(friendly[i].CurrentMana);
                }
                break;
        }
    }

    void SkillFriendly_Offensive(Enemy enemy, int value) {
        enemy.CurrentHealth -= value;
        CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{TargetIndex}").GetComponent<Unit>().UpdateHP(enemy.CurrentHealth);
    }

    void SkillFriendly_Support(Character friendly, int value, bool type) {
        switch(type) {
            case true:
                friendly.CurrentHealth += value;
                if(TargetIndex == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);
                else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{TargetIndex}").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);
                break;
        }
    }

    bool CheckType(SkillType type) {
        if(type != SkillType.AreaHeal || type != SkillType.AreaRestore || type != SkillType.Heal || type != SkillType.Restore) {
            return true;
        }
        else return false;
    }

    bool CheckSupport(SkillType type) {
        if(type == SkillType.Heal ||  type == SkillType.AreaHeal) {
            return true;
        }
        else return false;
    }


    public void ReturnMenu() {

        dialogueText.gameObject.SetActive(true);
        MainButtons.SetActive(true);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        UseButton.SetActive(false);

        PlayerTurn();
    }

    void Escape() {
        combatState = CombatState.NONE;
        Camera.SetActive(true);
        MainUI.SetActive(false);

        //Battle UI Disabled
        CurrentBattleUI.SetActive(true);
        MainButtons.SetActive(false);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        UseButton.SetActive(false);
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }
}