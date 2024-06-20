using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlsTest : MonoBehaviour
{
    public Transform player;

    //FolowCamera
    private Vector3 _offset;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    void Awake() {
        _offset = transform.position - player.position;
    }

    void LateUpdate() {
        Vector3 targetPosition = player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
    
    // Update is called once per frame
    // void Update () {
    //     int i = 0;
    //     while(i < Input.touchCount){
    //         Touch t = Input.GetTouch(i);
    //         Vector2 touchPos = getTouchPosition(t.position); // * -1 for perspective cameras
    //         if(t.phase == TouchPhase.Began){
    //             if(t.position.x > Screen.width / 2){
    //             }else{
    //                 leftTouch = t.fingerId;
    //                 startingPoint = touchPos;
    //             }
    //         }else if(t.phase == TouchPhase.Moved && leftTouch == t.fingerId){
    //             Vector2 offset = touchPos - startingPoint;
    //             Vector3 direction = Vector3.ClampMagnitude(new Vector3(offset.x, 0f, offset.y), 1.0f);
    //             //Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

    //             moveCharacter(direction);

    //             circle.transform.position = new Vector3(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y, 0f);

    //         }else if(t.phase == TouchPhase.Ended && leftTouch == t.fingerId){
    //             leftTouch = 99;
    //             circle.transform.position = new Vector3(outerCircle.transform.position.x, outerCircle.transform.position.y, 0f);
    //         }
    //         ++i;
    //     }
    // }
    // Vector2 getTouchPosition(Vector2 touchPosition){
    //     return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    // }

    // void moveCharacter(Vector3 direction){
    //     player.Translate(direction * speed * Time.deltaTime);
    // }
}
