using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void LateUpdate() {
        this.transform.LookAt(Camera.main.transform);
    }
}
