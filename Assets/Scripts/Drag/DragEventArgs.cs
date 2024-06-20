using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEventArgs : MonoBehaviour
{
    private Touch _trackedFinger;
    private GameObject _hitObject;

    public Touch TrackedFinger {
        get { return _trackedFinger; }
    }

    public GameObject hitObject {
        get { return _hitObject; }
    }

    public DragEventArgs(Touch trackedFinger, GameObject hitObject = null) {
        _trackedFinger = trackedFinger;
        _hitObject = hitObject;
    }
}
