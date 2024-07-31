using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    private GameObject player; 
    [SerializeField] public Area area;
    public GameObject spawnPointRef;

    void Awake() {
        player = GameObject.Find("PlayerContainer");
    }

    void OnEnable() {
        player = GameObject.Find("PlayerContainer");
    }

    void Update() {
        switch(tag) {
            case "ReturnAreaSided":
                if(GameObject.Find("/" + area.ToString() + "/Objects/" + area.ToString() + "Container/SpawnPoint").gameObject != null) {
                    spawnPointRef = GameObject.Find(area.ToString() + "/Objects/" + area.ToString() + "Container/SpawnPoint").gameObject;
                }
                break;
            case "ReturnArea":
                if(GameObject.Find("/" + area.ToString() + "Container").transform.Find("SpawnPoint").gameObject != null) {
                    spawnPointRef = GameObject.Find("/" + area.ToString() + "Container").transform.Find("SpawnPoint").gameObject;
                }
                break;
            default:
                if(GameObject.Find("/" + area.ToString() + "/Objects/SpawnPoint").gameObject != null) {
                    spawnPointRef = GameObject.Find(area.ToString() + "/Objects/SpawnPoint").gameObject;
                }
                break;
        }
        
    }

    public void Teleport() {
        GameObject SpawnPoint = GameObject.Find("/" + area.ToString() + "/Objects/SpawnPoint").gameObject;
        player.transform.position = SpawnPoint.transform.position;
    }

    public void Return() {
        GameObject SpawnPoint = GameObject.Find("/" + area.ToString() + "Container").transform.Find("SpawnPoint").gameObject;
        player.transform.position = SpawnPoint.transform.position;
    }

    public void ReturnSided() {
        GameObject SpawnPoint = GameObject.Find("/" + area.ToString() + "/Objects/" + area.ToString() + "Container/SpawnPoint").gameObject;
        player.transform.position = SpawnPoint.transform.position;
    }
}
