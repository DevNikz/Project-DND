using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class Dialogue
{
    public string names;
    public int[] hasChoices; // # of choices | 0 - only dialogue
    [TextArea(3, 10)] public string[] sentences;
}
