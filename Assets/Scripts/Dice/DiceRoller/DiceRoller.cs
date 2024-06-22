using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DiceSides))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DiceRoller : MonoBehaviour {
    [Header("Dice Rolling Settings")]
    [SerializeField] float rollForce = 50f;
    [SerializeField] float torqueAmount = 5f;
    [SerializeField] float maxRollTime = 3f;
    [SerializeField] float minAngularVelocity = 0.1f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;
    
    [Header("UI References")]
    [SerializeField] TMPro.TextMeshProUGUI resultText;

    [Header("Audio & Particle Effects")]
    [SerializeField] AudioClip shakeClip;
    [SerializeField] AudioClip rollClip;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip finalResultClip;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject finalResultEffect;

    DiceSides diceSides;
    AudioSource audioSource;
    Rigidbody rb;

    CountdownTimer rollTimer;
    
    Vector3 originPosition;
    Vector3 currentVelocity;
    bool finalize;

    public bool stop;

    void Awake() {
        diceSides = GetComponent<DiceSides>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        resultText.text = "SHAKE TO ROLL";
        originPosition = transform.position;
        
        rollTimer = new CountdownTimer(maxRollTime);
        rollTimer.OnTimerStart += RollDice;
        rollTimer.OnTimerStop += () => finalize = true;
        DiceData.stop = false;
    }

    public void OnShake() {
        if(rollTimer.IsRunning) return;
        rollTimer.Start();
    }

    public void OnEnable() {
        ResetDiceState();
        resultText.text = "SHAKE TO ROLL";
        originPosition = transform.position;

        rollTimer = new CountdownTimer(maxRollTime);
        rollTimer.OnTimerStart += RollDice;
        rollTimer.OnTimerStop += () => finalize = true;
        DiceData.stop = false;
    }

    public void OnDisable() {
        ResetDiceState();

        rollTimer.OnTimerStart -= RollDice;
        rollTimer.OnTimerStop -= () => finalize = false;
    }

    void Update() {
        rollTimer.Tick(Time.deltaTime);

        if (finalize) {
            MoveDiceToCenter();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (rollTimer.IsRunning && rollTimer.Progress < 0.5f && rb.angularVelocity.magnitude < minAngularVelocity) {
            finalize = true;
        }
        
        //audioSource.PlayOneShot(impactClip);
        //var particles = InstantiateFX(impactEffect, col.contacts[0].point, 1f);
        //Destroy(particles, 1f);
    }

    [ContextMenu("Roll Dice (Debug For PC)")]
    public void DebugRoll() {
        OnShake();
        resultText.text = "";

        Vector3 tempAccel = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        //Add Force
        rb.AddForce(tempAccel * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);
    }

    public void RollDice() {
        resultText.text = "";

        //Add Force
        rb.AddForce(DiceData.deviceAcceleration * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);
    }

    void MoveDiceToCenter() {
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, smoothTime, maxSpeed);

        if (originPosition.InRangeOf(transform.position, 0.1f)) {
            FinalizeRoll();
        }
    }

    void FinalizeRoll() {
        rollTimer.Stop();
        finalize = false;
        
        // audioSource.loop = false;
        // audioSource.Stop();
        // audioSource.PlayOneShot(finalResultClip);
        
        //var particles = InstantiateFX(finalResultEffect, transform.position, 5f);
        //Destroy(particles, 3f);
        
        int result = diceSides.GetMatch();
        DiceData.diceRoll = result;
        Debug.Log($"Dice landed on {result}");
        resultText.text = result.ToString();

        StartCoroutine(UnloadThis());
    }

    IEnumerator UnloadThis() {
        yield return new WaitForSeconds(1f);

        DiceData.stop = true;
        DialogueManager.Instance.stop = true;
    }

    void ResetDiceState() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originPosition;
    }
    
    GameObject InstantiateFX(GameObject fx, Vector3 position, float size) {
        var particles = Instantiate(fx, position, Quaternion.identity);
        particles.transform.localScale = Vector3.one * size;
        return particles;
    }
}