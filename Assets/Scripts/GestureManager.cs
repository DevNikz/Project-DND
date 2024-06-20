using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;

    private Touch[] _trackedFingers = new Touch[2];

    private float _gestureTime;

    [SerializeField] private TapProperty _tapProperty;

    [SerializeField] private SwipeProperty _swipeProperty;

    [SerializeField] private DragProperty _dragProperty;

    [SerializeField] private PanProperty _panProperty;

    [SerializeField] private SpreadProperty _spreadProperty;

    public EventHandler<TapEventArgs> OnTap;

    public EventHandler<SwipeEventArgs> OnSwipe;

    public EventHandler<DragEventArgs> OnDrag;

    public EventHandler<PanEventArgs> OnPan;

    public EventHandler<SpreadEventArgs> OnSpread;

    private Vector2 _startPoint = Vector2.zero;
    
    private Vector2 _endPoint = Vector2.zero;
    
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void CheckTap() {
        if(this._gestureTime <= this._tapProperty.Time &&
            Vector2.Distance(this._startPoint, this._endPoint) <=
            (Screen.dpi + this._tapProperty.MaxDistance)) {
            this.FireTapEvent();
        }
    }

    private void FireTapEvent() {
        GameObject hitObject = this.GetHitObject(this._startPoint);
        TapEventArgs args = new TapEventArgs(this._startPoint, hitObject);

        if(this.OnTap != null) {
            this.OnTap(this, args);
        }

        if(hitObject != null) {
            ITappable handler = hitObject.GetComponent<ITappable>();
            if(handler != null)
                handler.OnTap(args);
        }

    }

    private void CheckSwipe() {
        if(this._gestureTime <= this._swipeProperty.Time &&
            Vector2.Distance(this._startPoint, this._endPoint) >=
            (Screen.dpi * this._swipeProperty.MinDistance)) {
                this.FireSwipeEvent();
            }
    }

    private void FireSwipeEvent() {
        Vector2 position = this._trackedFingers[0].position;
        GameObject hitObject = this.GetHitObject(this._startPoint);
        Vector2 rawDirection = this._endPoint - this._startPoint; //inverse
        ESwipeDirection direction = this.GetSwipeDirection(rawDirection);
        SwipeEventArgs args = new SwipeEventArgs(direction, rawDirection, position, hitObject);

        if(this.OnSwipe != null) {
            this.OnSwipe(this, args);
        }

        if(hitObject != null) {
            ISwipeable handler = hitObject.GetComponent<ISwipeable>();
            if(handler != null)
                handler.OnSwipe(args);
        }
    }

    private ESwipeDirection GetSwipeDirection(Vector2 rawDirection) {
        if(rawDirection.x > rawDirection.y) {
            //Either Left or Right;
            if(rawDirection.x > 0) return ESwipeDirection.Right;
            else return ESwipeDirection.Left; 
        }
        else {
            if(rawDirection.y > 0) return ESwipeDirection.Up;
            else return ESwipeDirection.Down;
        }
    }

    private void CheckDrag() {
        if(this._gestureTime >= this._dragProperty.Time) {
            this.FireDragEvent();
        }
    }

    private void FireDragEvent() {
        Vector2 position = this._trackedFingers[0].position;
        GameObject hitObject = this.GetHitObject(position);
        DragEventArgs args = new DragEventArgs(this._trackedFingers[0], hitObject);

        if(this.OnDrag != null) {
            this.OnDrag(this, args);
        }

        if(hitObject != null) {
            IDraggable handler = hitObject.GetComponent<IDraggable>();
            if(handler!= null) handler.OnDrag(args);
        }
    }

    private void CheckPan() {
        if(Vector2.Distance(this._trackedFingers[0].position, this._trackedFingers[1].position) 
            <= (Screen.dpi * this._panProperty.MaxDistance)) {
                this.FirePanEvent();
            }
    }

    private void FirePanEvent() {
        PanEventArgs args = new PanEventArgs(this._trackedFingers);

        if(this.OnPan != null) {
            this.OnPan(this, args);
        }
    }

    private void CheckSpread() {
        float previousDistance = Vector2.Distance(this._trackedFingers[0].deltaPosition, this._trackedFingers[1].deltaPosition);
        float currentDistance = Vector2.Distance(this._trackedFingers[0].position, this._trackedFingers[1].position);
    }

    private void FireSpreadEvent(float distanceDelta) {
        SpreadEventArgs args = new SpreadEventArgs(this._trackedFingers);

        if(this.OnSpread != null) {
            this.OnSpread(this, args);
        }
    }

    private Vector2 GetPreviousPoint(Touch finger) {
        return finger.position - finger.deltaPosition;
    }

    private void CheckSingleFingerInput() {
        this._trackedFingers[0] = Input.GetTouch(0);
        switch(this._trackedFingers[0].phase) {
            case TouchPhase.Began:
                this._startPoint = this._trackedFingers[0].position;
                this._gestureTime = 0;
                break;
            
            case TouchPhase.Ended:
                this._endPoint = this._trackedFingers[0].position;
                this.CheckTap();
                this.CheckSwipe();
                break;
            
            default:
                this._gestureTime += Time.deltaTime;
                this.CheckDrag();
                break;
        }
    }

    private void CheckDualFingerInput() {
        this._trackedFingers[0] = Input.GetTouch(0);
        this._trackedFingers[1] = Input.GetTouch(1);

        switch(this._trackedFingers[0].phase, this._trackedFingers[1].phase) {
            case (TouchPhase.Moved, TouchPhase.Moved):
                this.CheckPan();
                break;
            case (TouchPhase.Moved, _):
                this.CheckSpread();
                break;
        }
    }

    private void Update() {
        switch(Input.touchCount) {
            case 1:
                CheckSingleFingerInput();
                break;
            case 2:
                CheckDualFingerInput();
                break;

        }
    }

    private GameObject GetHitObject(Vector2 screenPoint) {
        GameObject hitObject = null;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            hitObject = hit.collider.gameObject;
        }

        return hitObject;
    }
}
