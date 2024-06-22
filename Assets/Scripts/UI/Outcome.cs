using System;
using UnityEngine;

[Serializable]
public class Outcome
{
    public string name;
    [TextArea(3, 10)] public string sentences;
}
