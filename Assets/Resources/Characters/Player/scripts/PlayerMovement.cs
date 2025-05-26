using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    //enum Direction { Down = 0, Left = 1, Right = 3, Up = 2 }
    //enum AnimState { Idle = 0, Walking = 1, Attacking = 2, Hurt = 3 }
    
    
    float speed;
    
    //Animator animator;
    Vector2 movement;
    /*int facingDirection = 0;
    int state = 0;
    int previousState = 0;*/
    Rigidbody2D rigidBody;
    PlayerInputs inputs;
    
    public Vector2 CurrentMovement => movement;
    
    //private AttackHitboxController attackHitboxController;

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
        //animator = GetComponent<Animator>();
        //attackHitboxController = GetComponentInChildren<AttackHitboxController>();
    }

    private void OnEnable() {
        inputs.Enable();

        inputs.Gameplay.Movement.performed += OnMovement;
        inputs.Gameplay.Movement.canceled += OnMovement;
        
        //inputs.Gameplay.Hurt.performed += OnHurt;
        //inputs.Gameplay.Hurt.canceled += OnHurt;
        
        //inputs.Gameplay.Attack.performed += OnAttack;
        //inputs.Gameplay.Attack.canceled += OnAttack;
    }

    private void OnDisable() {
        inputs.Gameplay.Movement.performed -= OnMovement;
        inputs.Gameplay.Movement.canceled -= OnMovement;
        
        //inputs.Gameplay.Hurt.performed -= OnHurt;
        //inputs.Gameplay.Hurt.canceled -= OnHurt;
        
        //inputs.Gameplay.Attack.performed -= OnAttack;
        //inputs.Gameplay.Attack.canceled -= OnAttack;
    }

    private void OnMovement(InputAction.CallbackContext context) {
        //if (state < 2) {
            movement = context.ReadValue<Vector2>();
                    
            /*if (movement != Vector2.zero) {
                facingDirection = GetFacingDirection(movement.normalized);
                state = 1;
            } else {
                state = 0;
            }*/
        //}
    }
    
    /*private int GetFacingDirection(Vector2 direction) {
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
            movement = Vector2.zero;
        }
    }
    
    public void OnHurtEnd() {
        RefreshMovement();
        animator.SetFloat("State", state);
    }
    
    private void OnAttack(InputAction.CallbackContext context) {
        if (state < 2) {
            previousState = state; 
            state = 2;
            movement = Vector2.zero;
        }
    }

    private void CreateAttackHitbox()
    {
        if (attackHitboxController != null)
            attackHitboxController.SetHitboxFrame(facingDirection);
    }

    private void DisableHitboxes()
    {
        if (attackHitboxController != null)
            attackHitboxController.DisableHitboxes();
    }
    
    public void OnAttackEnd() {
        RefreshMovement();
        animator.SetFloat("State", previousState);
    }*/
    
    public void RefreshMovement() {
        movement = inputs.Gameplay.Movement.ReadValue<Vector2>();

        /*if (movement != Vector2.zero) {
            facingDirection = GetFacingDirection(movement.normalized);
            state = 1;
        } else {
            state = 0;
        }*/
    }

    /*private void LaunchAnimation() {
        animator.SetFloat("Direction",facingDirection);
        animator.SetFloat("State",state);
    }
    
    private void Update() {
        LaunchAnimation();
        //Debug.Log($"estado: {animator.GetFloat("State")}, Dirección de mirada: {animator.GetFloat("Direction")}");
    }*/

    private void FixedUpdate() {
        if (movement != Vector2.zero) {
            rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);
        }
    }
}
