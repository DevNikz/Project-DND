using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragReceiver : MonoBehaviour {
    [SerializeField] private Vector3 _targetPosition = Vector3.zero;
    [SerializeField] private float _speed = 5f;

    private void Start() {
        GestureManager.Instance.OnDrag += this.OnDrag;
    }

    private void OnDisable() {
        GestureManager.Instance.OnDrag -= this.OnDrag;
    }

    public void OnDrag(object sender, DragEventArgs args) {
        if(args.hitObject == this.gameObject) {
            Vector2 position = args.TrackedFinger.position;
            Ray ray = Camera.main.ScreenPointToRay(position);
            Vector2 worldPosition = ray.GetPoint(10);
            this._targetPosition = worldPosition;
            this.transform.position = worldPosition;
        }
    }

    private void Update() {
        if(this.transform.position != this._targetPosition) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition,  _speed * Time.deltaTime);
            
        }
    }
}