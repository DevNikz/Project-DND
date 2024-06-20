using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanReceiver : MonoBehaviour
{
    [SerializeField] private float _speed = 50.0f;

    private void Start() {
        GestureManager.Instance.OnPan += this.OnPan;
    }

    private void OnDisable() {
        GestureManager.Instance.OnPan -= this.OnPan;
    }

    public void OnPan(object sender, PanEventArgs args) {
        Vector2 deltaPosition0 = args.TrackedFingers[0].deltaPosition;
        Vector2 deltaPosition1 = args.TrackedFingers[1].deltaPosition;

        Vector2 averagePosition = (deltaPosition0 + deltaPosition1) / 2;
        averagePosition = averagePosition / Screen.dpi;

        Vector3 change = averagePosition * (this._speed * Time.deltaTime);
        this.transform.position += change;
        // Vector2 touchDeltaPos = args.TrackedFingers[0].deltaPosition;
        // transform.Translate(-touchDeltaPos.x * _speed, -touchDeltaPos.y * _speed, 0);
    }
}
