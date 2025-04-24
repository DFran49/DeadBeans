using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    //enum Direction { Down = 0, Left = 1, Right = 3, Up = 2 }
    //enum AnimState { Idle = 0, Walking = 1, Attacking = 2, Hurt = 3 }
    
    
    [SerializeField] float speed;
    
    Animator animator;
    Vector2 movement;
    int facingDirection = 0;
    int state = 0;
    int previousState = 0;
    Rigidbody2D rigidBody;
    PlayerInputs inputs;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        inputs = new PlayerInputs();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        inputs.Enable();

        inputs.Gameplay.Movement.performed += OnMovement;
        inputs.Gameplay.Movement.canceled += OnMovement;
        
        inputs.Gameplay.Hurt.performed += OnHurt;
        inputs.Gameplay.Hurt.canceled += OnHurt;
        
        inputs.Gameplay.Attack.performed += OnAttack;
        inputs.Gameplay.Attack.canceled += OnAttack;
    }

    private void OnDisable() {
        inputs.Gameplay.Movement.performed -= OnMovement;
        inputs.Gameplay.Movement.canceled -= OnMovement;
        
        inputs.Gameplay.Hurt.performed -= OnHurt;
        inputs.Gameplay.Hurt.canceled -= OnHurt;
        
        inputs.Gameplay.Attack.performed -= OnAttack;
        inputs.Gameplay.Attack.canceled -= OnAttack;
    }

    private void OnMovement(InputAction.CallbackContext context) {
        if (state < 2) {
            movement = context.ReadValue<Vector2>();
                    
            if (movement != Vector2.zero) {
                facingDirection = GetFacingDirection(movement.normalized);
                state = 1;
            } else {
                state = 0;
            }
        }
    }
    
    private int GetFacingDirection(Vector2 direction) {
        direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

        return direction switch {
            Vector2 v when v == Vector2.up    => 2,
            Vector2 v when v == Vector2.down  => 0,
            Vector2 v when v == Vector2.left  => 1,
            Vector2 v when v == Vector2.right => 3,
            _ => facingDirection
        };
    }
    
    private void OnHurt(InputAction.CallbackContext context) {
        if (state < 3) {
            state = 3;
        }
    }
    
    public void OnHurtEnd() {
        state = 0;
        animator.SetFloat("State", state);
    }
    
    private void OnAttack(InputAction.CallbackContext context) {
        if (state < 2) {
            previousState = state; 
            state = 2;
        }
    }
    
    public void OnAttackEnd() {
        state = previousState;
        animator.SetFloat("State", previousState);
    }

    private void LaunchAnimation() {
        animator.SetFloat("Direction",facingDirection);
        animator.SetFloat("State",state);
    }
    
    private void Update() {
        LaunchAnimation();
        //Debug.Log($"estado: {animator.GetFloat("State")}, Dirección de mirada: {animator.GetFloat("Direction")}");
    }

    private void FixedUpdate() {
        rigidBody.linearVelocity = movement * speed;
    }
}
