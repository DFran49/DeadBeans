using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movementController;
    private AnimationCompController animationController;
    private PlayerInputs inputs;
    private CombatController combat;
    private CryptosComponent money;

    private GameObject interactable;
    
    private Vector2 knockbackVelocity;
    private float knockbackTime = 0f;
    
    private void Awake()
    {
        combat = GetComponent<CombatController>();
        
        movementController = GetComponent<PlayerMovement>();
        animationController = GetComponent<AnimationCompController>();
        money = GetComponent<CryptosComponent>();
        inputs = new PlayerInputs();
    }

    private void Start()
    {
        movementController.setSpd(combat.getSpd());
        var hitbox = GetComponentInChildren<AttackHitboxController>();
        if (hitbox != null)
        {
            hitbox.SetStats(combat.getStr());
        }
    }
    
    private void OnEnable() {
        inputs.Enable();
        EnableInputs();
    }

    public void EnableInputs()
    {
        inputs.Gameplay.Attack.performed += OnAttack;
        inputs.Gameplay.Hurt.performed += OnHurt;
        inputs.Gameplay.Interact.performed += OnInteraction;
    }
    
    private void OnDisable() {
        DisableInputs();
        inputs.Disable();
    }

    public void DisableInputs()
    {
        inputs.Gameplay.Attack.performed -= OnAttack;
        inputs.Gameplay.Hurt.performed -= OnHurt;
        inputs.Gameplay.Interact.performed -= OnInteraction;
    }
    
    private void Update() {
        animationController.UpdateAnimation(movementController.CurrentMovement);
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (interactable != null)
        {
            string npc = interactable.name;
            Debug.Log(npc);
            switch (npc)
            {
                case "npcSalon":
                    Restore();
                    Debug.Log("Salon "+interactable.name);
                    break;
                case "npcTienda":
                    //tienda
                    Debug.Log("Tienda "+interactable.name);
                    break;
                case "npcSalida":
                    EnterDungeon();
                    Debug.Log("Salida "+interactable.name);
                    break;
            }
        }
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
        // Spawner un item
        ItemSpawner.Instance.SpawnItem(2, 5, transform.position);

        // Guardar inventario
        FindObjectOfType<InventoryManager>().SaveInventoryToPlayer();
        PlayerStatsLoader.GuardarStatsAJson();

        // Cargar inventario
        PlayerStatsLoader.CargarYAplicarStatsDesdeJson();
        //OnHurt();
    }
    
    public void OnHurt() {
        movementController.StopMovement();
        animationController.OnHurt();
        var origen = transform.position;
        origen.y += 20;
        combat.PlayerReceiveDamage(7, origen, "Physical");
    }
    
    public void OnAttackEnd() 
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            interactable = other.gameObject;
    }

    private void Restore()
    {
        combat.ReceiveHeal(100000000);
    }

    private void EnterDungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            interactable = null;
    }
    
    /*void FixedUpdate()
    {
        if (knockbackTime > 0)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTime -= Time.fixedDeltaTime;
            return;
        }
    }*/
}
