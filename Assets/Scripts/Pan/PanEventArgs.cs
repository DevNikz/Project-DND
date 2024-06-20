using System;
using UnityEngine;

public class PanEventArgs : EventArgs{
    private Touch[] _trackedFingers;

    public Touch[] TrackedFingers {
        get { return this._trackedFingers; }
    }
 
    public PanEventArgs(Touch[] trackedFingers) {
        this._trackedFingers = trackedFingers;
    }
}