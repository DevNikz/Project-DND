using UnityEngine;

public class TargetController : MonoBehaviour {
    public int targetIndex;
    
    public void SetTarget(int targetIndex) {
        this.targetIndex = targetIndex;
    }

    public void TransferTarget() {
        CombatManager.Instance.TargetIndex = targetIndex;
    }
}