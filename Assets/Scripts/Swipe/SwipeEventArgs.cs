using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEventArgs : EventArgs
{
    private ESwipeDirection _swipeDirection;
    private Vector2 _direction;
    private Vector2 _position;
    private GameObject _hitObject;
    
    public ESwipeDirection SwipeDirection {
        get { return this._swipeDirection; }
    } 

    public Vector2 Direction {
        get { return this._direction; }
        set { this._direction = value; }
    }

    public Vector2 Position {
        get { return this._position; }
        set { this._position = value; }
    }

    public GameObject HitObject {
        get { return this._hitObject; }
    } 

    public SwipeEventArgs(ESwipeDirection swipeDirection, Vector2 direction, Vector2 position, GameObject hitObject) {
        this._swipeDirection = swipeDirection;
        this._direction = direction;
        this._position = position;
        this._hitObject = hitObject;
    }
}
