using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class SwipeProperty
{
    [Tooltip("Maximum allowable time to be considered a swipe")]
    [SerializeField] private float _time = 2.0f;

    [Tooltip("Maximum allowable distance to be considered a swipe")]
    [SerializeField] private float _minDistance = 0.7f;

    public float Time {
        get { return this._time; }
        set { this._time = value; }
    }

    public float MinDistance {
        get { return this._minDistance; }
        set { this._minDistance = value; }
    }
}
