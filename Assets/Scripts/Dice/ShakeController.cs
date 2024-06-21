using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiceRoll))]
public class ShakeController : MonoBehaviour
{
    public float shakeThreshold;
    public float minShakeInterval;

    private float sqrShakeThreshold;
    private float deltaShake;
    private DiceRoll diceRoll;
    public float shakeMagnitude;

    [SerializeField] public bool isShaking;

    void Start() {
        sqrShakeThreshold = Mathf.Pow(shakeThreshold, 2);
        diceRoll = GetComponent<DiceRoll>();
    }
    
    void OnEnable() {
        isShaking = false;
        DiceData.stop = false;
    }

    void OnDisable() {
        isShaking = false;
        DiceData.stop = false;
    }

    void Update() {
        if(!DiceData.stop) {
            ShakePhone();
            shakeMagnitude = Input.acceleration.sqrMagnitude;
        }
        else {
            shakeMagnitude = 0f;
        }
    }

    void ShakePhone() {
        if(Input.acceleration.sqrMagnitude >= sqrShakeThreshold &&
            Time.unscaledTime >= deltaShake + minShakeInterval) {
            isShaking = true;
            diceRoll.RollMobile(Input.acceleration);
            deltaShake = Time.unscaledTime;
        }
        else if(Input.acceleration.sqrMagnitude < sqrShakeThreshold) {
            isShaking = false;
        }
    }
}
