using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPlayer : MonoBehaviour
{
    [SerializeField] public GameObject MainUI;
    private GameObject invParent;
    private GameObject bg;
    private GameObject baseUI;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();

        invParent = this.transform.parent.transform.parent.gameObject;
        bg = invParent.transform.Find("Inventory").transform.Find("BG").gameObject;
        baseUI = invParent.transform.Find("Inventory").transform.Find("Base").gameObject;
    }

    public void PlaySFX() {
        SFXManager.Instance.Play("IconBlink");
    }

    public void StopSFX() {
        SFXManager.Instance.Stop("IconBlink");
    }

    public void BGClose() {
        bg.GetComponent<Animator>().Play("BGClose");
    }

    public void EndFrame_Close() {
        bg.SetActive(false);
        baseUI.SetActive(false);
        MainUI.SetActive(true);
    }
}
