using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpreadProperty
{
    [Tooltip("Minimum distance change between fingers to be a spread")]
    [SerializeField] private float _minDistanceChange = 0.5f;

    public float MinDistanceChange {
        get { return this._minDistanceChange; }
    }

    public SpreadProperty(float minDistanceChange) {
        this._minDistanceChange = minDistanceChange;
    }
}
