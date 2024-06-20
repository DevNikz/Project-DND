using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public float rotation = 45f;
    public DirectionH currentFace;
    public EntityState entityState;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        this.transform.rotation = Quaternion.Euler(rotation,0f,0f);
        SetDirection();
        currentFace = PlayerData.directionH;
        entityState = PlayerData.entityState;
    }

    public void SetDirection() {
        if(PlayerData.entityState == EntityState.Moving) {
            SetRun();
        }
        else {
            SetIdle();
        }
    }

    public void SetRun() {
        switch(PlayerData.directionH) {
            case DirectionH.Right:
                anim.Play("MageAnimR");
                break;
            case DirectionH.Left:
                anim.Play("MageAnimL");
                break;
        }
    }

    public void SetIdle() {
        switch(PlayerData.directionH) {
            case DirectionH.Right:
                anim.Play("MageR");
                break;
            case DirectionH.Left:
                anim.Play("MageL");
                break;
        }
    }
}
