using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    [SerializeField] public GameObject cubeCopy;

    private void Start() {
        GestureManager.Instance.OnTap += this.OnTap;
    }

    private void OnDisable() {
        GestureManager.Instance.OnTap -= this.OnTap;
    }

    public void OnTap(object sender, TapEventArgs args) {
        if(args.HitObject == null) {
            Debug.Log("Test");

            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            Quaternion rotation = Quaternion.identity;

            this.cubeCopy.SetActive(true);
            Instantiate(this.cubeCopy, ray.GetPoint(10), rotation);
        }

        else Debug.Log("Object is Obstructing.");
    }

}
