using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class SpawnReciever : MonoBehaviour, ITappable, ISwipeable, IDraggable
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector3 _targetPosition = Vector3.zero;

    [SerializeField] public int _type = 0;

    public void OnTap(TapEventArgs args) {
        //Destroy(args.HitObject);
    }

    public void OnSwipe(SwipeEventArgs args) {
        switch(this._type) {
            case 0:
                this.MovePerpendicular(args);
                break;
            case 1:
                this.MoveDiagonal(args);
                break;
        }
    }

    public void OnDrag(DragEventArgs args) {
        if(args.hitObject == this.gameObject) {
            Vector2 position = args.TrackedFinger.position;
            Ray ray = Camera.main.ScreenPointToRay(position);
            Vector2 worldPosition = ray.GetPoint(10);
            this._targetPosition = worldPosition;
            this.transform.position = worldPosition;
        }
    }

    public void MovePerpendicular(SwipeEventArgs args) {
        // Vector3 direction = Vector3.zero;
        Vector3 direction = new Vector3(args.Direction.x, args.Direction.y, 0f);
        switch(args.SwipeDirection) {
            case ESwipeDirection.Up:
                Debug.Log("Up");
                direction = Vector3.up;
                // direction.y = 1;
                break;
            case ESwipeDirection.Down:
                Debug.Log("Down");
                direction = Vector3.down;
                // direction.y = -1;
                break;
            case ESwipeDirection.Left:
                Debug.Log("Left");
                direction = Vector3.left;
                // direction.x = -1;
                break;
            case ESwipeDirection.Right:
                Debug.Log("Right");
                direction = Vector3.right;
                // direction.x = 1;
                break;
        }
        this._targetPosition += direction * 5;
    }

    public void MoveDiagonal(SwipeEventArgs args) {
        Vector3 direction = new Vector3(args.Direction.x, args.Direction.y, 0f);
        switch(args.SwipeDirection) {
            case ESwipeDirection.UpLeft:
                Debug.Log("Up");
                direction = Vector3.up + Vector3.left;
                break;
            case ESwipeDirection.UpRight:
                Debug.Log("Down");
                direction = Vector3.up + Vector3.right;
                break;
            case ESwipeDirection.DownLeft:
                Debug.Log("Left");
                direction = Vector3.down + Vector3.left;
                break;
            case ESwipeDirection.DownRight:
                Debug.Log("Right");
                direction = Vector3.down + Vector3.right;
                break;
        }
        this._targetPosition += direction * 5;
    }

    private void OnEnable() {
        this._targetPosition = this.transform.position;
    }

    private void Update() {
        if(this.transform.position != this._targetPosition) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition, this._speed * Time.deltaTime);
            
        }
    }
}
