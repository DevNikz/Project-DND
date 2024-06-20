using System;
using UnityEngine;

public class SpreadEventArgs : EventArgs{
    private Touch[] _trackedFingers;

    public Touch[] TrackedFingers {
        get { return this._trackedFingers; }
    }
 
    public SpreadEventArgs(Touch[] trackedFingers) {
        this._trackedFingers = trackedFingers;
    }
}