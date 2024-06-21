using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public bool endFrame;

    [SerializeField] public GameObject MainUI;
    private GameObject bg;
    private GameObject baseUI;
    private Animator bgAnimate;
    private Animator baseUIAnimate;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void OpenInventory() {
        MainUI.SetActive(false);

        //Dir
        bg = this.transform.Find("Inventory").transform.Find("BG").gameObject;
        baseUI = this.transform.Find("Inventory").transform.Find("Base").gameObject;

        //Animate
        bgAnimate = bg.GetComponent<Animator>();
        baseUIAnimate = baseUI.GetComponent<Animator>();
        bgAnimate.Play("BGOpen");
        baseUIAnimate.SetBool("IsOpen", true);

        //Active
        bg.SetActive(true);
        baseUI.SetActive(true);
    }

    void Update() {
        if(this.endFrame == true) {
            bg.SetActive(false);
            baseUI.SetActive(false);
            MainUI.SetActive(true);
            this.endFrame = false;
        }
    }

    public void CloseInventory() {
        baseUIAnimate.SetBool("IsOpen",false);
    }
}
