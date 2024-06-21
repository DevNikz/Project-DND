using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPlayer : MonoBehaviour
{
    public void PlaySFX() {
        SFXManager.Instance.Play("IconBlink");
    }

    public void StopSFX() {
        SFXManager.Instance.Stop("IconBlink");
    }

    public void EndFrame() {
        InventoryManager.Instance.endFrame = true;
    }
}
