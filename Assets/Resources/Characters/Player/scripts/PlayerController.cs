using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movementController;
    private AnimationCompController animationController;
    private PlayerInputs inputs;
    private CombatController combat;

    private CryptosComponent money;
    private void Awake()
    {
        combat = GetComponent<CombatController>();
        
        movementController = GetComponent<PlayerMovement>();
        animationController = GetComponent<AnimationCompController>();
        money = GetComponent<CryptosComponent>();
        inputs = new PlayerInputs();

        var hitbox = GetComponentInChildren<AttackHitboxController>();
        if (hitbox != null)
        {
            hitbox.OnHitboxTriggerEnter += HandleHitboxTrigger;
        }
    }

    private void Start()
    {
        movementController.setSpd(combat.getSpd());
    }
    
    private void HandleHitboxTrigger(Collider2D other)
    {
        Debug.Log("El jugador golpeó a: " + other.name + " e hizo " + combat.getStr() + " puntos de daño");
        
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(combat.getStr(), "Physical");
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
    
    public void OnHurt() {
        //movementController.StopMovement();
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

    public void ApplyMoney(int amount)
    {
        if (amount > 0)
        {
            money.AddCryptos(amount);
        } else if (amount < 0)
        {
            money.RemoveCryptos(amount);
        }
    }

    public int GetMoney()
    {
        return money.GetCryptos();
    }

    public void Die()
    {
        combat.SetHp(combat.stats.maxHp);
        combat.healthBar.SetHealth(combat.health.GetHp());
        money.ResetCryptos();
    }
}
