using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{

    //Mobile
    [Header("Roll(Mobile)")]
    public float shakeForceMultiplier;

    //References
    private Rigidbody rb;

    private float yDot, xDot, zDot;
    public int rollValue;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void RollMobile(Vector3 deviceAcceleration) {
        rb.AddForce(deviceAcceleration * shakeForceMultiplier, ForceMode.Impulse);
        StartCoroutine(WaitForStop());
    }

    IEnumerator WaitForStop() {
        yield return new WaitForFixedUpdate();
        while(rb.angularVelocity.sqrMagnitude > 0.1) {
            yield return new WaitForFixedUpdate();
        }
        CheckRoll();
    }

    void CheckRoll() {
        DiceData.stop = true;
        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        switch(yDot) {
            case 1:
                rollValue = 2;
                DiceData.diceRoll = 2;
                break;
            case -1:
                rollValue = 5;
                DiceData.diceRoll = 5;
                break;
        }

        switch(xDot) {
            case 1:
                rollValue = 4;
                DiceData.diceRoll = 4;
                break;
            case -1:
                rollValue = 3;
                DiceData.diceRoll = 3;
                break;
        }

        switch(zDot) {
            case 1:
                rollValue = 1;
                DiceData.diceRoll = 1;
                break;
            case -1:
                rollValue = 6;
                DiceData.diceRoll = 6;
                break;
        }
    }
}
