using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody rb;

    public bool canMove;
    public float speed = 20f;
    private Vector3 playerSpeed;
    public Vector3 joyInput;
    public DirectionH horDirection;
    public DirectionV vertDirection;
    public EntityState entityState; 

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
        canMove = true;
    }
    
    private void Update() {
        joyInput = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        if(canMove) { 
            rb.MovePosition(transform.position + joyInput * speed * Time.deltaTime);

            //Debug Speed
            playerSpeed = joyInput * speed * Time.deltaTime;
        }

        SetState();
        SetDirection();
        horDirection = PlayerData.directionH;
        vertDirection = PlayerData.directionV;
        entityState = PlayerData.entityState;
}

    private void SetState() {
        if(playerSpeed != Vector3.zero) PlayerData.entityState = EntityState.Moving;
        else PlayerData.entityState = EntityState.Idle;
    }

    private void SetDirection() {
        //Horizontal
        if(joystick.Horizontal > 0) {
            PlayerData.directionH = DirectionH.Right;
        }
        else if(joystick.Horizontal < 0) {
            PlayerData.directionH = DirectionH.Left;
        }
        else PlayerData.directionH = DirectionH.None;

        //Vertical
        if(joystick.Vertical > 0) {
            PlayerData.directionV = DirectionV.Up;
        }
        else if(joystick.Vertical < 0) {
            PlayerData.directionV = DirectionV.Down;
        }
        else PlayerData.directionV = DirectionV.None;
    }
}
