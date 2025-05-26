using UnityEngine;

public class AnimationCompController : MonoBehaviour
{
    Animator animator;
    int facingDirection = 0;
    int state = 0;
    
    private AttackHitboxController attackHitboxController;
    
    private void Awake() {
        animator = GetComponent<Animator>();
        attackHitboxController = GetComponentInChildren<AttackHitboxController>();
    }
    
    public void UpdateAnimation(Vector2 movement) {
        if (state < 2 && movement != Vector2.zero) {
            facingDirection = GetDirectionIndex(movement.normalized);
            state = 1;
        } else if (state < 2) {
            state = 0;
        }

        LaunchAnimation();
    }
    
    /*public void RefreshMovement() {
        movement = inputs.Gameplay.Movement.ReadValue<Vector2>();

        /*if (movement != Vector2.zero) {
            facingDirection = GetFacingDirection(movement.normalized);
            state = 1;
        } else {
            state = 0;
        }
    }*/
    
    public void OnAttack() {
        if (state < 2) {
            state = 2;
        }
        LaunchAnimation();
    }
    
    public void OnAttackEndInside() {
        state = 0;
        LaunchAnimation();
    }
    
    public void OnHurt() {
        state = 3;
        LaunchAnimation();
    }
    
    public void OnHurtEndInside(Vector2 movement)
    {
        state = 0;
        UpdateAnimation(movement);
    }
    
    private int GetDirectionIndex(Vector2 dir) {
        dir = new Vector2(Mathf.Round(dir.x), Mathf.Round(dir.y));
        if (dir == Vector2.up) return 2;
        if (dir == Vector2.down) return 0;
        if (dir == Vector2.left) return 1;
        if (dir == Vector2.right) return 3;
        return facingDirection;
    }
    
    public void CreateAttackHitbox()
    {
        if (attackHitboxController != null)
            attackHitboxController.SetHitboxFrame(facingDirection);
    }

    public void DisableHitboxes()
    {
        if (attackHitboxController != null)
            attackHitboxController.DisableHitboxes();
    }
    
    private void LaunchAnimation() {
        animator.SetFloat("Direction", facingDirection);
        animator.SetFloat("State", state);
    }
}
