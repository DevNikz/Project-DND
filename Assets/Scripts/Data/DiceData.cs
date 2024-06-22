using UnityEngine;

public static class DiceData {

    //Choice
    public static Stats choiceStat { get; set; }
    public static int choiceStatReq { get; set; }

    //Outcome | Success
    public static string name_Success { get; set; }
    public static string outcomeSentence_Success { get; set; } 

    //Outcome | Failed
    public static string name_Failed { get; set; }
    public static string outcomeSentence_Failed { get; set; } 

    //Debug
    public static bool isShaking { get; set; }
    public static bool stop { get; set; }
    public static int diceRoll { get; set; }
    public static Vector3 deviceAcceleration { get; set; }
}