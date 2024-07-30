using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    void OnTriggerStay(Collider other) {
        if(other.name == "PlayerContainer") {
            this.GetComponent<Outline>().enabled = true;
        }   
    }

    void OnTriggerExit(Collider other) {
        this.GetComponent<Outline>().enabled = false;
    }
}


