using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string name;
    public bool[] orders; //0 - only dialogue | 1 - w/ choice
    [TextArea(3, 10)] public string[] sentences;
    public Choice[] choice;
}
