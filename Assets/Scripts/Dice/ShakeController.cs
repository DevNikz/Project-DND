using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeThreshold = 2.5f;
    public float minShakeInterval = 0.2f;

    private float sqrShakeThreshold;
    private float deltaShake;
    private DiceRoller diceRoll;
    public float shakeMagnitude;

    [SerializeField] public bool isShaking;

    void Start() {
        sqrShakeThreshold = Mathf.Pow(shakeThreshold, 2);
        diceRoll = GetComponent<DiceRoller>();
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
            //diceRoll.RollMobile(Input.acceleration);
            diceRoll.OnShake();
            diceRoll.RollDice();
            DiceData.deviceAcceleration = Input.acceleration;
            deltaShake = Time.unscaledTime;
        }
        else if(Input.acceleration.sqrMagnitude < sqrShakeThreshold) {
            isShaking = false;
        }
    }
}
