using System;
using UnityEngine;

[Serializable] public class PanProperty {
    [Tooltip("Max Distance between two fingers")]
    [SerializeField] private float _maxDistance = 0.7f;

    public float MaxDistance {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }
}