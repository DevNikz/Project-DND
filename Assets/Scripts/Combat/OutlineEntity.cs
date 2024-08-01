using UnityEngine;

public class OutlineEntity : MonoBehaviour {
    
    public bool highlight;

    void Update() {
        if(transform.Find("Outline").gameObject != null) {
            if(highlight) {
                this.transform.Find("Outline").gameObject.SetActive(true);
            }
            else {
                this.transform.Find("Outline").gameObject.SetActive(false);
            }
        }

    }

    public void HighlightUnit(bool value) {
        highlight = value;
    }

}