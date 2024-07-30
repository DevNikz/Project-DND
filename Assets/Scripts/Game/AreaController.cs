using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    private GameObject player; 
    [SerializeField] public Area area;

    void Awake() {
        player = GameObject.Find("PlayerContainer");
    }

    void OnEnable() {
        player = GameObject.Find("PlayerContainer");
    }

    public void Teleport() {
        GameObject SpawnPoint = GameObject.Find(area.ToString()).transform.Find("SpawnPoint").gameObject;
        player.transform.position = SpawnPoint.transform.position;
    }

    public void Return() {
        GameObject SpawnPoint = GameObject.Find(area.ToString() + "Container").transform.Find("SpawnPoint").gameObject;
        player.transform.position = SpawnPoint.transform.position;
    }
}
