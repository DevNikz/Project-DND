using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    //Shadow and Entity has Animator. Set them based on their name
    public void LoadUnit(string name, bool value) {
        GameObject Shadow = transform.Find("Shadow").gameObject;
        GameObject Entity = transform.Find("Entity").gameObject;

        Shadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{name}");
        Shadow.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<AnimatorController>($"Characters/{name}");

        Entity.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{name}");
        Entity.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<AnimatorController>($"Characters/{name}");

        Shadow.GetComponent<SpriteRenderer>().flipX = value;
        Entity.GetComponent<SpriteRenderer>().flipX = value;
    }

    public void LoadPlayer(Character character) {
        TextMeshProUGUI Text = this.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();
        Image Icon = this.transform.Find("Icon").GetComponent<Image>();
        Slider HP = this.transform.Find("HUD/Health").GetComponent<Slider>();
        Slider MP = this.transform.Find("HUD/Mana").GetComponent<Slider>();

        Text.text = $"{character.Name} | Level {character.Level}";
        Icon.sprite = Resources.Load<Sprite>($"Characters/{character.Class}Player");

        HP.wholeNumbers = true;
        HP.maxValue = character.Health;
        HP.value = character.CurrentHealth;

        MP.wholeNumbers = true;
        MP.maxValue = character.Mana;
        MP.value = character.CurrentMana;
    }

    public void LoadCompanion(Character character) {
        TextMeshProUGUI Text = this.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();
        Image Icon = this.transform.Find("Icon").GetComponent<Image>();
        Slider HP = this.transform.Find("HUD/Health").GetComponent<Slider>();
        Slider MP = this.transform.Find("HUD/Mana").GetComponent<Slider>();

        Text.text = $"{character.Name} | Level {character.Level}";
        Icon.sprite = Resources.Load<Sprite>($"Characters/{character.Name}");

        HP.wholeNumbers = true;
        HP.maxValue = character.Health;
        HP.value = character.CurrentHealth;

        MP.wholeNumbers = true;
        MP.maxValue = character.Mana;
        MP.value = character.CurrentMana;
    }

    public void LoadEnemy(Enemy enemy, bool value) {
        TextMeshProUGUI Text = this.transform.Find("HUD/Text").GetComponent<TextMeshProUGUI>();
        Image Icon = this.transform.Find("Icon").GetComponent<Image>();
        float Scale = this.transform.Find("Icon").transform.localScale.x;
        Slider HP = this.transform.Find("HUD/Health").GetComponent<Slider>();
        Slider MP = this.transform.Find("HUD/Mana").GetComponent<Slider>();

        Text.text = $"{enemy.Name} | Level {enemy.Level}";
        Icon.sprite = Resources.Load<Sprite>($"Characters/{enemy.Name}{enemy.Class}");

        HP.wholeNumbers = true;
        HP.maxValue = enemy.Health;
        HP.value = enemy.CurrentHealth;

        MP.wholeNumbers = true;
        MP.maxValue = enemy.Mana;
        MP.value = enemy.CurrentMana;

        if(value) {
            Vector3 temp = this.transform.Find("Icon").transform.localScale;
            temp.x = -1f;
            this.transform.Find("Icon").transform.localScale = temp;
        }
    }

    public void UpdateHP(int current) {
        Slider HP = this.transform.Find("HUD/Health").GetComponent<Slider>();
        HP.value = current;
    }

    public void UpdateMP(int current) {
        Slider MP = this.transform.Find("HUD/Mana").GetComponent<Slider>();
        MP.value = current;
    }
}
