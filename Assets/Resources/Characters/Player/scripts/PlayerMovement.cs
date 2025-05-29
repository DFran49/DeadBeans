using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    //enum Direction { Down = 0, Left = 1, Right = 3, Up = 2 }
    //enum AnimState { Idle = 0, Walking = 1, Attacking = 2, Hurt = 3 }
    
    
    float speed;
    
    Vector2 movement;
    Rigidbody2D rigidBody;
    PlayerInputs inputs;
    
    public Vector2 CurrentMovement => movement;

    public void setSpd(int spd)
    {
        speed = spd;
    }

    public void StopMovement()
    {
        movement = Vector2.zero;
    }
    
    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        inputs = new PlayerInputs();
    }

    private void OnEnable() {
        inputs.Enable();

        inputs.Gameplay.Movement.performed += OnMovement;
        inputs.Gameplay.Movement.canceled += OnMovement;
    }

    private void OnDisable() {
        inputs.Gameplay.Movement.performed -= OnMovement;
        inputs.Gameplay.Movement.canceled -= OnMovement;
    }

    private void OnMovement(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }
    
    public void RefreshMovement() {
        movement = inputs.Gameplay.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        if (movement != Vector2.zero) {
            rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);
        }
        else
        {
            rigidBody.linearVelocity = Vector2.zero;
            rigidBody.angularVelocity = 0f;
        }
    }
}
