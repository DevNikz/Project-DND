using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DragProperty
{
    [SerializeField]
    private float _time = 0.8f;

    public float Time {
        get { return _time; }
        set { _time = value; }
    }
}