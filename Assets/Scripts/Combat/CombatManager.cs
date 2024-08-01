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
    private GameObject TargetButton;
    private GameObject refArea;
    private List<Character> refCharacters;
    private List<Enemy> refEnemies;
    public int SkillIndex;
    public int TargetIndex;


    public enum CombatState { NONE, START, PLAYERTURN, COMPANIONTURN1, COMPANIONTURN2, ENEMYTURN0, ENEMYTURN1, ENEMYTURN2, WIN, LOST }

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

    //Setup
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
        UseButton = SideButtons.transform.Find("Use").gameObject;
        TargetButton = SideButtons.transform.Find("Target").gameObject;

        MainButtons.SetActive(true);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        TargetButtons.SetActive(false);

        //Set Unit
        for(int i = 0; i < refCharacters.Count; i++) {
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

    //Start | Step 1
    void PlayerTurn() {
        string text = "Choose An Action";
        StartCoroutine(TypeSentence(text));

        combatState = CombatState.PLAYERTURN;
    }

    void FriendlyTurn() {
        string text = "Choose An Action";
        StartCoroutine(TypeSentence(text));
    }

    void EnemyTurn() {
        SetupEnemyAttack();
    }

    void SetupEnemyAttack() {
        int skillIndex, targetIndex;
        switch(combatState) {
            case CombatState.ENEMYTURN0:    
                //Random Skill
                skillIndex = Random.Range(0, refEnemies[0].Skills.Count-1);

                //Random Target if Skill is Single Category
                if(refEnemies[0].Skills[skillIndex].Category == SkillCategory.Single) {
                    if(CheckType(refEnemies[0].Skills[skillIndex].Type)) targetIndex = SetupEnemyTarget();
                    else targetIndex = SetupFriendlyTarget();
                }
                else targetIndex = -1;

                //Init Skill
                StartCoroutine(EnemySkill(skillIndex, targetIndex));
                break;
            case CombatState.ENEMYTURN1:
                //Random Skill
                skillIndex = Random.Range(0, refEnemies[1].Skills.Count-1);

                //Random Target if Skill is Single Category
                if(refEnemies[1].Skills[skillIndex].Category == SkillCategory.Single) {
                    if(CheckType(refEnemies[1].Skills[skillIndex].Type)) targetIndex = SetupEnemyTarget();
                    else targetIndex = SetupFriendlyTarget();
                }
                else targetIndex = -1;

                //Init Skill
                StartCoroutine(EnemySkill(skillIndex, targetIndex));
                break;
            case CombatState.ENEMYTURN2:
                //Random Skill
                skillIndex = Random.Range(0, refEnemies[2].Skills.Count-1);

                //Random Target if Skill is Single Category
                if(refEnemies[2].Skills[skillIndex].Category == SkillCategory.Single) {
                    if(CheckType(refEnemies[2].Skills[skillIndex].Type)) targetIndex = SetupEnemyTarget();
                    else targetIndex = SetupFriendlyTarget();
                }
                else targetIndex = -1;

                //Init Skill
                StartCoroutine(EnemySkill(skillIndex, targetIndex));
                break;
        }
    }

    IEnumerator EnemySkill(int skillIndex, int targetIndex) {
        string text;
        switch(combatState) {
            case CombatState.ENEMYTURN0:
                text = $"{refEnemies[0].Name} Used {refEnemies[0].Skills[skillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

                refEnemies[0].CurrentMana -= refEnemies[0].Skills[skillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy0").GetComponent<Unit>().UpdateMP(refEnemies[0].CurrentMana);

                //Set Attack (All)
                if(refEnemies[0].Skills[skillIndex].Category == SkillCategory.All) {
                    if(CheckType(refEnemies[0].Skills[skillIndex].Type)) SkillAllEnemy_Offensive(refCharacters, refEnemies[0].Skills[skillIndex].ActualModifier);
                    else {
                        SkillAllEnemy_Support(refEnemies, refEnemies[0].Skills[skillIndex].ActualModifier, CheckSupport(refEnemies[0].Skills[skillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refEnemies[0].Skills[skillIndex].Type)) SkillEnemy_Offensive(refCharacters[targetIndex], refEnemies[0].Skills[skillIndex].ActualModifier, targetIndex);
                    else {
                        SkillEnemy_Support(refEnemies[targetIndex], refCharacters[0].Skills[skillIndex].ActualModifier, targetIndex, CheckSupport(refCharacters[0].Skills[skillIndex].Type));
                    }
                }

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    switch(refEnemies.Count) {
                        case 1:
                            text = $"{refCharacters[0].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.PLAYERTURN;
                            MainButtons.SetActive(true);
                            PlayerTurn();
                            break;
                        default:
                            text = $"{refEnemies[1].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.ENEMYTURN1;
                            EnemyTurn();
                            break;
                    }
                }
                
                else {
                    text = $"You and Your Companions Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;

            case CombatState.ENEMYTURN1:
                text = $"{refEnemies[1].Name} Used {refEnemies[1].Skills[skillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

                refEnemies[1].CurrentMana -= refEnemies[1].Skills[skillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy1").GetComponent<Unit>().UpdateMP(refEnemies[1].CurrentMana);

                //Set Attack (All)
                if(refEnemies[1].Skills[skillIndex].Category == SkillCategory.All) {
                    if(CheckType(refEnemies[1].Skills[skillIndex].Type)) SkillAllEnemy_Offensive(refCharacters, refEnemies[1].Skills[skillIndex].ActualModifier);
                    else {
                        SkillAllEnemy_Support(refEnemies, refEnemies[1].Skills[skillIndex].ActualModifier, CheckSupport(refEnemies[1].Skills[skillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refEnemies[1].Skills[skillIndex].Type)) SkillEnemy_Offensive(refCharacters[targetIndex], refEnemies[1].Skills[skillIndex].ActualModifier, targetIndex);
                    else {
                        SkillEnemy_Support(refEnemies[targetIndex], refCharacters[1].Skills[skillIndex].ActualModifier, targetIndex, CheckSupport(refCharacters[1].Skills[skillIndex].Type));
                    }
                }

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    switch(refEnemies.Count) {
                        case 2:
                            text = $"{refCharacters[0].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.PLAYERTURN;
                            MainButtons.SetActive(true);
                            PlayerTurn();
                            break;
                        case 3:
                            text = $"{refEnemies[2].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.ENEMYTURN2;
                            EnemyTurn();
                            break;
                    }
                }
                
                else {
                    text = $"You and Your Companions Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;

            case CombatState.ENEMYTURN2:
                text = $"{refEnemies[2].Name} Used {refEnemies[2].Skills[skillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

                refEnemies[2].CurrentMana -= refEnemies[2].Skills[skillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy2").GetComponent<Unit>().UpdateMP(refEnemies[2].CurrentMana);

                //Set Attack (All)
                if(refEnemies[2].Skills[skillIndex].Category == SkillCategory.All) {
                    if(CheckType(refEnemies[2].Skills[skillIndex].Type)) SkillAllEnemy_Offensive(refCharacters, refEnemies[2].Skills[skillIndex].ActualModifier);
                    else {
                        SkillAllEnemy_Support(refEnemies, refEnemies[2].Skills[skillIndex].ActualModifier, CheckSupport(refEnemies[2].Skills[skillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refEnemies[2].Skills[skillIndex].Type)) SkillEnemy_Offensive(refCharacters[targetIndex], refEnemies[2].Skills[skillIndex].ActualModifier, targetIndex);
                    else {
                        SkillEnemy_Support(refEnemies[targetIndex], refCharacters[2].Skills[skillIndex].ActualModifier, targetIndex, CheckSupport(refCharacters[2].Skills[skillIndex].Type));
                    }
                }

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    text = $"{refCharacters[0].Name}'s Turn";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    combatState = CombatState.PLAYERTURN;
                    MainButtons.SetActive(true);
                    PlayerTurn();
                    break;
                }
                
                else {
                    text = $"You and Your Companions Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;
        }
    }

    int SetupEnemyTarget() {
        return Random.Range(0, refCharacters.Count-1);
    }

    int SetupFriendlyTarget() {
        return Random.Range(0, refEnemies.Count-1);
    }

    //Step 2
    //Attack Button 
    public void OnAttackButton() {
        if (combatState == CombatState.PLAYERTURN || combatState == CombatState.COMPANIONTURN1 || combatState == CombatState.COMPANIONTURN2 ) {
            StartCoroutine(SetupAttack());
        }
    }

    //Step 5
    public void OnUseButton() {
        for(int i = 0; i < refEnemies.Count; i++) {
            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
        }

        for(int i = 0; i < refCharacters.Count; i++) {
            if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
            else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
        }

        for(int i = 0; i < 4; i++) {
            SkillButtons.transform.Find($"Skill{i}").GetComponent<Image>().color = Color.white;
        }

        for(int i = 0; i < 3; i++) {
            TargetButtons.transform.Find($"Target{i}").GetComponent<Image>().color = Color.white;
        }

        switch(combatState) {
            case CombatState.PLAYERTURN:
                if(refCharacters[0].Skills[SkillIndex].Category == SkillCategory.All) {
                    StartCoroutine(ActivateSkill());
                }
                else {
                    if(CheckType(refCharacters[0].Skills[SkillIndex].Type)) StartCoroutine(SetupTargetEnemy()); //True = Offensive
                    else StartCoroutine(SetupTargetFriendly());
                }
                break;
            case CombatState.COMPANIONTURN1:
                if(refCharacters[1].Skills[SkillIndex].Category == SkillCategory.All) {
                    StartCoroutine(ActivateSkill());
                }

                else {
                    if(CheckType(refCharacters[1].Skills[SkillIndex].Type)) StartCoroutine(SetupTargetEnemy()); //True = Offensive
                    else StartCoroutine(SetupTargetFriendly());
                }
                break;
            case CombatState.COMPANIONTURN2:
                if(refCharacters[2].Skills[SkillIndex].Category == SkillCategory.All) {
                    StartCoroutine(ActivateSkill());
                }
                else {
                    if(CheckType(refCharacters[2].Skills[SkillIndex].Type)) StartCoroutine(SetupTargetEnemy()); //True = Offensive
                    else StartCoroutine(SetupTargetFriendly());
                }
                break;
        }
    }

    //Step 5.3
    public void OnTargetButton() {
        for(int i = 0; i < refEnemies.Count; i++) {
            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
        }

        for(int i = 0; i < refCharacters.Count; i++) {
            if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
            else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
        }

        for(int i = 0; i < 4; i++) {
            SkillButtons.transform.Find($"Skill{i}").GetComponent<Image>().color = Color.white;
        }

        for(int i = 0; i < 3; i++) {
            TargetButtons.transform.Find($"Target{i}").GetComponent<Image>().color = Color.white;
        }

        StartCoroutine(ActivateSkill());

        // switch(combatState) {
        //     case CombatState.PLAYERTURN:
        //         if(refCharacters[0].Skills[SkillIndex].Category == SkillCategory.Single) {
        //             StartCoroutine(ActivateSkill());
        //         }
        //         break;
        //     case CombatState.COMPANIONTURN1:
        //         if(refCharacters[1].Skills[SkillIndex].Category == SkillCategory.Single) {
        //             StartCoroutine(ActivateSkill());
        //         }
        //         break;
        //     case CombatState.COMPANIONTURN2:
        //         if(refCharacters[2].Skills[SkillIndex].Category == SkillCategory.Single) {
        //             StartCoroutine(ActivateSkill());
        //         }
        //         break;
        // }
    }

    //Exit
    public void OnEscapeButton() {
        Escape();
    }

    //Step 3
    IEnumerator SetupAttack() {
        string text = "Loading Skills...";
        StartCoroutine(TypeSentence(text));

        yield return new WaitForSeconds(1);

        dialogueText.gameObject.SetActive(false);
        MainButtons.SetActive(false);
        SideButtons.SetActive(true);
        SkillButtons.SetActive(true);
        TargetButtons.SetActive(false);
        UseButton.SetActive(false);
        TargetButton.SetActive(false);

        SetupSkills();
    }

    //Step 4
    void SetupSkills() {
        //Clear Skills
        for(int i = 0; i < 4; i++) SkillButtons.transform.Find($"Skill{i}").gameObject.SetActive(false);

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
            case CombatState.COMPANIONTURN1:
                for(int i = 0; i < refCharacters[1].Skills.Count; i++) {
                    var skill = SkillButtons.transform.Find($"Skill{i}/Text").GetComponent<TextMeshProUGUI>();
                    if(skill == null) Debug.Log("error");
                    else {
                        skill.transform.parent.gameObject.SetActive(true);
                        skill.transform.parent.GetComponent<SkillController>().SetIndex(i);
                        skill.text = refCharacters[1].Skills[i].Type.ToString();
                        skill.transform.parent.GetComponent<Button>().onClick.AddListener(() => ButtonSelection());
                    }
                } 
                break;
            case CombatState.COMPANIONTURN2:
                for(int i = 0; i < refCharacters[2].Skills.Count; i++) {
                    var skill = SkillButtons.transform.Find($"Skill{i}/Text").GetComponent<TextMeshProUGUI>();
                    if(skill == null) Debug.Log("error");
                    else {
                        skill.transform.parent.gameObject.SetActive(true);
                        skill.transform.parent.GetComponent<SkillController>().SetIndex(i);
                        skill.text = refCharacters[2].Skills[i].Type.ToString();
                        skill.transform.parent.GetComponent<Button>().onClick.AddListener(() => ButtonSelection());
                    }
                } 
                break;
        }
    }

    //Step5.1
    IEnumerator SetupTargetEnemy() {
        Debug.Log("TargetEnemy");
        UseButton.SetActive(false);
        TargetButton.SetActive(false);
        SkillButtons.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        string text = "Loading Enemy Targets...";
        StartCoroutine(TypeSentence(text));

        yield return new WaitForSeconds(1);

        dialogueText.gameObject.SetActive(false);
        TargetButtons.SetActive(true);

        SelectTargetEnemy();
    }

    IEnumerator SetupTargetFriendly() {
        Debug.Log("TargetFriendly");
        UseButton.SetActive(false);
        TargetButton.SetActive(false);
        SkillButtons.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        string text = "Loading Friendly Targets...";
        StartCoroutine(TypeSentence(text));

        yield return new WaitForSeconds(1);

        dialogueText.gameObject.SetActive(false);
        TargetButtons.SetActive(true);

        SelectTargetFriendly();
    }

    //Step5.2

    void SelectTargetEnemy() {
        for(int i = 0; i < 3; i++) {
            TargetButtons.transform.Find($"Target{i}").gameObject.SetActive(false);
        }

        for(int i = 0; i < refEnemies.Count; i++) {
            var target = TargetButtons.transform.Find($"Target{i}/Text").GetComponent<TextMeshProUGUI>();
            if(target == null) Debug.Log("Error");
            else {
                target.transform.parent.gameObject.SetActive(true);
                target.transform.parent.GetComponent<TargetController>().SetTarget(i);
                target.text = refEnemies[i].Name.ToString();
                target.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
                target.transform.parent.GetComponent<Button>().onClick.AddListener(() => TargetSelection());
            }
        } 
    }

    void SelectTargetFriendly() {
        for(int i = 0; i < 3; i++) {
            TargetButtons.transform.Find($"Target{i}").gameObject.SetActive(false);
        }

        for(int i = 0; i < refCharacters.Count; i++) {
            var target = TargetButtons.transform.Find($"Target{i}/Text").GetComponent<TextMeshProUGUI>();
            if(target == null) Debug.Log("Error");
            else {
                target.transform.parent.gameObject.SetActive(true);
                target.transform.parent.GetComponent<TargetController>().SetTarget(i);
                target.text = refCharacters[i].Name.ToString();
                target.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
                target.transform.parent.GetComponent<Button>().onClick.AddListener(() => TargetSelectionFriendly());
            }
        } 
    }

    //Step 6
    IEnumerator ActivateSkill() {
        string text;
        SkillButtons.SetActive(false);
        TargetButtons.SetActive(false);
        SideButtons.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        switch(combatState) {
            //Player's Turn
            case CombatState.PLAYERTURN:
                text = $"{refCharacters[0].Name} Used {refCharacters[0].Skills[SkillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

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

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    switch(refCharacters.Count) {
                        case 1:
                            text = $"{refEnemies[0].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.ENEMYTURN0;
                            EnemyTurn();
                            break;
                        default:
                            text = $"{refCharacters[1].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.COMPANIONTURN1;
                            MainButtons.SetActive(true);
                            FriendlyTurn();
                            break;
                    }
                }
                
                else {
                    text = $"All Enemies Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;

            //CompanionTurn1
            case CombatState.COMPANIONTURN1:
                text = $"{refCharacters[1].Name} Used {refCharacters[1].Skills[SkillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

                //Receive Damage
                refCharacters[1].CurrentMana -= refCharacters[1].Skills[SkillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(refCharacters[1].CurrentMana);

                //Set Attack (All)
                if(refCharacters[1].Skills[SkillIndex].Category == SkillCategory.All) {
                    if(CheckType(refCharacters[1].Skills[SkillIndex].Type)) SkillAllFriendly_Offensive(refEnemies, refCharacters[1].Skills[SkillIndex].ActualModifier);
                    else {
                        Debug.Log($"Modifier: {refCharacters[1].Skills[SkillIndex].ActualModifier}");
                        SkillAllFriendly_Support(refCharacters, refCharacters[1].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[1].Skills[SkillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refCharacters[1].Skills[SkillIndex].Type)) SkillFriendly_Offensive(refEnemies[TargetIndex], refCharacters[1].Skills[SkillIndex].ActualModifier);
                    else {
                        SkillFriendly_Support(refCharacters[TargetIndex], refCharacters[1].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[1].Skills[SkillIndex].Type));
                    }
                }

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    switch(refCharacters.Count) {
                        case 2:
                            text = $"{refEnemies[0].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.ENEMYTURN0;
                            EnemyTurn();
                            break;
                        case 3:
                            text = $"{refCharacters[2].Name}'s Turn";
                            StartCoroutine(TypeSentence(text));
                            yield return new WaitForSeconds(1);
                            combatState = CombatState.COMPANIONTURN2;
                            MainButtons.SetActive(true);
                            FriendlyTurn();
                            break;
                    }
                }
                
                else {
                    text = $"All Enemies Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;

            //Companion2's Turn
            case CombatState.COMPANIONTURN2:
                text = $"{refCharacters[2].Name} Used {refCharacters[2].Skills[SkillIndex].Type}";
                StartCoroutine(TypeSentence(text));

                yield return new WaitForSeconds(1);

                //Receive Damage
                refCharacters[2].CurrentMana -= refCharacters[2].Skills[SkillIndex].Value;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(refCharacters[2].CurrentMana);

                //Set Attack (All)
                if(refCharacters[2].Skills[SkillIndex].Category == SkillCategory.All) {
                    if(CheckType(refCharacters[2].Skills[SkillIndex].Type)) SkillAllFriendly_Offensive(refEnemies, refCharacters[2].Skills[SkillIndex].ActualModifier);
                    else {
                        SkillAllFriendly_Support(refCharacters, refCharacters[2].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[2].Skills[SkillIndex].Type));
                    }
                }

                //Set Attack (Single)
                else {
                    if(CheckType(refCharacters[2].Skills[SkillIndex].Type)) SkillFriendly_Offensive(refEnemies[TargetIndex], refCharacters[2].Skills[SkillIndex].ActualModifier);
                    else {
                        SkillFriendly_Support(refCharacters[TargetIndex], refCharacters[2].Skills[SkillIndex].ActualModifier, CheckSupport(refCharacters[2].Skills[SkillIndex].Type));
                    }
                }

                yield return new WaitForSeconds(1);

                if(CheckEnemyCount()) {
                    text = $"{refEnemies[0].Name}'s Turn";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    combatState = CombatState.ENEMYTURN0;
                    EnemyTurn();
                    break;
                }
                
                else {
                    text = $"All Enemies Have Perished.";
                    StartCoroutine(TypeSentence(text));
                    yield return new WaitForSeconds(1);
                    Escape();
                }
                break;
        }
    }

    //Win Condition
    bool CheckEnemyCount() {
        switch(refEnemies.Count) {
            case 1:
                if(refEnemies[0].currentState == CharacterState.Dead) {
                    return false;
                }
                else return true;
            case 2:
                if(refEnemies[0].currentState == CharacterState.Dead && refEnemies[1].currentState == CharacterState.Dead) {
                    return false;
                }
                else return true;
            case 3:
                if(refEnemies[0].currentState == CharacterState.Dead && refEnemies[1].currentState == CharacterState.Dead && refEnemies[2].currentState == CharacterState.Dead) {
                    return false;
                }
                else return true;
        }
        return false;
    }

    void CheckDead(Enemy enemy) {
        if(enemy.CurrentHealth <= 0) {
            enemy.currentState = CharacterState.Dead;
        }
    }

    void CheckDeadFriendly(Character friendly) {
        if(friendly.CurrentHealth <= 0) {
            friendly.currentState = CharacterState.Dead;
        }
    }

    //Skills(Friendly)
    void SkillAllFriendly_Offensive(List<Enemy> enemy, int value) {
        string text;
        for(int i = 0; i < enemy.Count; i++) {
            enemy[i].CurrentHealth -= value;
            CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{i}").GetComponent<Unit>().UpdateHP(enemy[i].CurrentHealth);

            text = $"{enemy[i].Name} Took {value} Damage";
            StartCoroutine(TypeSentence(text));

            CheckDead(enemy[i]);
            if(enemy[i].currentState == CharacterState.Dead) {
                text = $"{enemy[i].Name} Has Perished";
                dialogueText.text = text;
            }
        }
    }

    void SkillAllFriendly_Support(List<Character> friendly, int modifier, bool type) {
        string text;
        Debug.Log($"Modifier: {modifier}");
        switch(type) {
            case true: //Heal HP
                for(int i = 0; i < friendly.Count; i++) {
                    friendly[i].CurrentHealth += modifier;
                    if(friendly[i].CurrentHealth > friendly[i].Health) friendly[i].CurrentHealth = friendly[i].Health;
                    
                    if(i == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly[0].CurrentHealth);
                    else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateHP(friendly[i].CurrentHealth);
                }
                text = $"You and Your Companions Received {modifier} HP";
                StartCoroutine(TypeSentence(text));
                break;
            case false: //Heal MP
                for(int i = 0; i < friendly.Count; i++) {
                    friendly[i].CurrentMana += modifier;
                    if(friendly[i].CurrentMana > friendly[i].Mana) friendly[i].CurrentMana = friendly[i].Mana;

                    if(i == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(friendly[0].CurrentMana);
                    else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateMP(friendly[i].CurrentMana);
                }
                text = $"You and Your Companions Received {modifier} MP";
                StartCoroutine(TypeSentence(text));
                break;
        }
    }

    void SkillFriendly_Offensive(Enemy enemy, int value) {
        string text;
        enemy.CurrentHealth -= value;
        CurrentBattleUI.transform.Find($"Characters/Enemy/Enemy{TargetIndex}").GetComponent<Unit>().UpdateHP(enemy.CurrentHealth);
        text = $"{enemy.Name} Took {value} Damage";
        StartCoroutine(TypeSentence(text));

        CheckDead(enemy);
        if(enemy.currentState == CharacterState.Dead) {
            text = $"{enemy.Name} Has Perished";
            dialogueText.text = text;
        }
    }

    void SkillFriendly_Support(Character friendly, int value, bool type) {
        string text;
        switch(type) {
            case true:
                friendly.CurrentHealth += value;
                if(friendly.CurrentHealth > friendly.Health) friendly.CurrentHealth = friendly.Health;

                if(TargetIndex == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);
                else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{TargetIndex}").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);
                text = $"{friendly.Name} Received {value} HP";
                StartCoroutine(TypeSentence(text));
                break;
            case false:
                friendly.CurrentMana += value;
                if(friendly.CurrentMana > friendly.Mana) friendly.CurrentMana = friendly.Mana;

                if(TargetIndex == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateMP(friendly.CurrentMana);
                else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{TargetIndex}").GetComponent<Unit>().UpdateMP(friendly.CurrentMana);
                text = $"{friendly.Name} Received {value} MP";
                StartCoroutine(TypeSentence(text));
                break;
        }
    }

    //Skills(Enemy)
    void SkillAllEnemy_Offensive(List<Character> friendly, int value) {
        for(int i = 0; i < friendly.Count; i++) {
            if(i == 0) {
                friendly[i].CurrentHealth -= value;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly[i].CurrentHealth);

                string text = $"{friendly[i].Name} Took {value} Damage";
                StartCoroutine(TypeSentence(text));

                CheckDeadFriendly(friendly[i]);
                if(friendly[i].currentState == CharacterState.Dead) {
                    text = $"{friendly[i].Name} Has Perished";
                    dialogueText.text = text;
                }
            }
            else {
                friendly[i].CurrentHealth -= value;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateHP(friendly[i].CurrentHealth);

                string text = $"{friendly[i].Name} Took {value} Damage";
                StartCoroutine(TypeSentence(text));

                CheckDeadFriendly(friendly[i]);
                if(friendly[i].currentState == CharacterState.Dead) {
                    text = $"{friendly[i].Name} Has Perished";
                    dialogueText.text = text;
                }
            }
            
        }
    }

    void SkillAllEnemy_Support(List<Enemy> enemy, int value, bool type) {
        string text;
        switch(type) {
            case true: //Heal HP
                for(int i = 0; i < enemy.Count; i++) {
                    enemy[i].CurrentHealth += value;
                    if(enemy[i].CurrentHealth > enemy[i].Health) enemy[i].CurrentHealth = enemy[i].Health;
                    CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateHP(enemy[i].CurrentHealth);
                }
                text = $"The Enemy Has Received {value} HP";
                StartCoroutine(TypeSentence(text));
                break;
            case false: //Heal MP
                for(int i = 0; i < enemy.Count; i++) {
                    enemy[i].CurrentMana += value;
                    if(enemy[i].CurrentMana > enemy[i].Mana) enemy[i].CurrentMana = enemy[i].Mana;
                    CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{i}").GetComponent<Unit>().UpdateMP(enemy[i].CurrentMana);
                }
                text = $"The Enemy Has Received {value} MP";
                StartCoroutine(TypeSentence(text));
                break;
        }
    }

    void SkillEnemy_Offensive(Character friendly, int value, int targetIndex) {
        friendly.CurrentHealth -= value;

        if(targetIndex == 0) CurrentBattleUI.transform.Find($"Characters/Friendly/Player").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);
        else CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{targetIndex}").GetComponent<Unit>().UpdateHP(friendly.CurrentHealth);

        string text = $"{friendly.Name} Took {value} Damage";
        StartCoroutine(TypeSentence(text));

        CheckDeadFriendly(friendly);

        if(friendly.currentState == CharacterState.Dead) {
            text = $"{friendly.Name} Has Perished";
            dialogueText.text = text;
        }
    }

    void SkillEnemy_Support(Enemy enemy, int value, int targetIndex, bool type) {
        string text;
        switch(type) {
            case true:
                enemy.CurrentHealth += value;
                if(enemy.CurrentHealth > enemy.Health) enemy.CurrentHealth = enemy.Health;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{targetIndex}").GetComponent<Unit>().UpdateHP(enemy.CurrentHealth);

                text = $"{enemy.Name} Received {value} HP";
                StartCoroutine(TypeSentence(text));
                break;
            case false:
                enemy.CurrentMana += value;
                if(enemy.CurrentMana > enemy.Mana) enemy.CurrentMana = enemy.Mana;
                CurrentBattleUI.transform.Find($"Characters/Friendly/Companion{targetIndex}").GetComponent<Unit>().UpdateMP(enemy.CurrentMana);

                text = $"{enemy.Name} Received {value} MP";
                StartCoroutine(TypeSentence(text));
                break;
        }
    }


    //Misc
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
                    if(CheckType(refCharacters[0].Skills[SkillIndex].Type)) {
                        for(int i = 0; i < refEnemies.Count; i++) {
                            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                    else {
                        for(int i = 0; i < refCharacters.Count; i++) {
                            if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(true);
                            else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                }
                else {
                    for(int i = 0; i < refEnemies.Count; i++) {
                        CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }

                    for(int i = 0; i < refCharacters.Count; i++) {
                        if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
                        else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }
                } 
                break;

            case CombatState.COMPANIONTURN1:
                //Reset Selected
                for(int i = 0; i < refCharacters[1].Skills.Count; i++) {
                    if(SkillIndex == i) {
                        SkillButtons.transform.Find($"Skill{SkillIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    }
                    else SkillButtons.transform.Find($"Skill{i}").GetComponent<Image>().color = Color.white;
                }

                //Skill
                if(refCharacters[1].Skills[SkillIndex].Category == SkillCategory.All) {
                    if(CheckType(refCharacters[1].Skills[SkillIndex].Type)) {
                        for(int i = 0; i < refEnemies.Count; i++) {
                            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                    else {
                        for(int i = 0; i < refCharacters.Count; i++) {
                            if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(true);
                            else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                }
                else {
                    for(int i = 0; i < refEnemies.Count; i++) {
                        CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }

                    for(int i = 0; i < refCharacters.Count; i++) {
                        if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
                        else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }
                } 
                break;
            
            case CombatState.COMPANIONTURN2:
                //Reset Selected
                for(int i = 0; i < refCharacters[2].Skills.Count; i++) {
                    if(SkillIndex == i) {
                        SkillButtons.transform.Find($"Skill{SkillIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    }
                    else SkillButtons.transform.Find($"Skill{i}").GetComponent<Image>().color = Color.white;
                }

                //Skill
                if(refCharacters[2].Skills[SkillIndex].Category == SkillCategory.All) {
                    if(CheckType(refCharacters[2].Skills[SkillIndex].Type)) {
                        for(int i = 0; i < refEnemies.Count; i++) {
                            CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                    else {
                        for(int i = 0; i < refCharacters.Count; i++) {
                            if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(true);
                            else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(true);
                        }
                    }
                }
                else {
                    for(int i = 0; i < refEnemies.Count; i++) {
                        CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }

                    for(int i = 0; i < refCharacters.Count; i++) {
                        if(i == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
                        else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    }
                } 
                break;
        }
        UseButton.SetActive(true);
    }

    void TargetSelection() {
        for(int i = 0; i < refEnemies.Count; i++) {
            if(TargetIndex == i) {
                TargetButtons.transform.Find($"Target{TargetIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            }
            else TargetButtons.transform.Find($"Target{i}").GetComponent<Image>().color = Color.white;
        }

        for(int i = 0; i < refEnemies.Count; i++) {
            if(TargetIndex == i) CurrentArea.transform.Find($"Characters/Enemy{TargetIndex}").GetComponent<OutlineEntity>().HighlightUnit(true);
            else CurrentArea.transform.Find($"Characters/Enemy{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
        }

        TargetButton.SetActive(true);
    }

    void TargetSelectionFriendly() {
        for(int i = 0; i < refCharacters.Count; i++) {
            if(TargetIndex == i) {
                TargetButtons.transform.Find($"Target{TargetIndex}").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            }
            else TargetButtons.transform.Find($"Target{i}").GetComponent<Image>().color = Color.white;
        }

        for(int i = 0; i < refCharacters.Count; i++) {
            switch(i) {
                case 0:
                    if(TargetIndex == 0) CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(true);
                    else CurrentArea.transform.Find($"Characters/Player").GetComponent<OutlineEntity>().HighlightUnit(false);
                    break;
                default:
                    if(TargetIndex == i) CurrentArea.transform.Find($"Characters/Companion{TargetIndex}").GetComponent<OutlineEntity>().HighlightUnit(true);
                    else CurrentArea.transform.Find($"Characters/Companion{i}").GetComponent<OutlineEntity>().HighlightUnit(false);
                    break;
            }
        }

        TargetButton.SetActive(true);
    }

    bool CheckType(SkillType type) {
        Debug.Log(type);
        switch(type) {
            case SkillType.Heal:
                return false;
            case SkillType.AreaHeal:
                return false;
            case SkillType.Restore:
                return false;
            case SkillType.AreaRestore:
                return false;
            default:
                return true;
        }
    }

    bool CheckSupport(SkillType type) {
        switch(type) {
            case SkillType.Heal:
                return true;
            case SkillType.AreaHeal:
                return true;
            case SkillType.Restore:
                return false;
            case SkillType.AreaRestore:
                return false;
            default:
                return false;
        }
    }

    //Return
    public void ReturnMenu() {

        dialogueText.gameObject.SetActive(true);
        MainButtons.SetActive(true);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        TargetButtons.SetActive(false);
        UseButton.SetActive(false);
        TargetButton.SetActive(false);

        FriendlyTurn();
    }

    //Escape
    void Escape() {
        combatState = CombatState.NONE;

        //Battle UI Disabled
        CurrentArea.transform.Find("Camera").gameObject.SetActive(false);
        CurrentArea.transform.Find("Base").gameObject.SetActive(false);
        CurrentArea.transform.Find("Characters").gameObject.SetActive(false);
        CurrentBattleUI.SetActive(false);
        MainButtons.SetActive(false);
        SideButtons.SetActive(false);
        SkillButtons.SetActive(false);
        TargetButtons.SetActive(false);
        UseButton.SetActive(false);
        TargetButtons.SetActive(false);

        //Re-Enable Everything
        Camera.SetActive(true);
        MainUI.SetActive(true);

        //Update Player and Companion Stats
        PlayerManager.Instance.UpdateLevel();
        CompanionManager.Instance.UpdateLevelAll();

        //Update UI
        GameManager.Instance.UpdateUI();

        //Give Rewards(optional)
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }
}