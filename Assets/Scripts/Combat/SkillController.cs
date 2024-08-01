using UnityEngine;

public class SkillController : MonoBehaviour {
    public int index = -1;

    public void SetIndex(int index) {
        this.index = index;
    }

    public void TransferIndex() {
        CombatManager.Instance.SkillIndex = index;
    }

}