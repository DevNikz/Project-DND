using System;
using UnityEngine;

[Serializable]
public class Choice
{
    public Stats choiceStat;
    [Range(0,20)]public int choiceStatReq;
    [TextArea(3, 10)] public string sentences;

    public Outcome SuccessDialogue;
    public Outcome FailDialogue;
}
