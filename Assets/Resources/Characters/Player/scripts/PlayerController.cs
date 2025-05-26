using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movementController;
    private AnimationCompController animationController;
    private PlayerInputs inputs;
    private CombatController combat;
    //private PlayerMovement movement;

    private void Awake()
    {
        combat = GetComponent<CombatController>();
        
        movementController = GetComponent<PlayerMovement>();
        animationController = GetComponent<AnimationCompController>();
        inputs = new PlayerInputs();
        movementController.setSpd(combat.stats.spd);
        
        var hitbox = GetComponentInChildren<AttackHitboxController>();
        if (hitbox != null)
        {
            hitbox.OnHitboxTriggerEnter += HandleHitboxTrigger;
        }
    }
    
    private void HandleHitboxTrigger(Collider2D other)
    {
        Debug.Log("El jugador fue golpeado por: " + other.name + " e hizo " + combat.getStr() + " puntos de daño");
        // Aquí puedes llamar a métodos del jugador, reducir vida, etc.
        //Debug.Log("Hitbox triggered with: " + other.name);
    
        
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(combat.getStr(), "Physical"); // o el daño que corresponda
            }
        }
    }
    
    private void OnEnable() {
        inputs.Enable();
        inputs.Gameplay.Attack.performed += OnAttack;
        inputs.Gameplay.Hurt.performed += OnHurt;
        
    }
    
    private void OnDisable() {
        inputs.Gameplay.Attack.performed -= OnAttack;
        inputs.Gameplay.Hurt.performed -= OnHurt;
        inputs.Disable();
    }
    
    private void Update() {
        animationController.UpdateAnimation(movementController.CurrentMovement);
    }
    
    private void OnAttack(InputAction.CallbackContext ctx) {
        movementController.StopMovement();
        animationController.OnAttack();
    }

    public void CreateAtkHitbox()
    {
        animationController.CreateAttackHitbox();
    }
    
    public void DisableAtkHitbox()
    {
        animationController.DisableHitboxes();
    }
    
    private void OnHurt(InputAction.CallbackContext ctx) {
        movementController.StopMovement();
        animationController.OnHurt();
    }
    
    public void OnAttackEnd() {
        
        animationController.OnAttackEndInside();
        movementController.RefreshMovement();
    }
    
    public void OnHurtEnd() {
        
        animationController.OnHurtEndInside(movementController.CurrentMovement);
        movementController.RefreshMovement();
    }
}
