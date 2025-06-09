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
        PlayerStatsLoader.CargarYAplicarStatsDesdeJson();
        ScriptablePlayer playerData = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
        Debug.Log("Cargado " + playerData.cryptos + " - " + playerData.health);
        money.AddCryptos(playerData.cryptos);
        combat.health.SetHp(playerData.health);
        
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
        inputs.Gameplay.Interact.performed += OnInteraction;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void EnableInputs()
    {
        inputs.Gameplay.Attack.performed += OnAttack;
        inputs.Gameplay.Hurt.performed += OnHurt;
    }
    
    private void OnDisable() {
        DisableInputs();
        inputs.Gameplay.Interact.performed -= OnInteraction;
        SceneManager.activeSceneChanged -= OnSceneChanged;
        inputs.Disable();
    }

    public void DisableInputs()
    {
        inputs.Gameplay.Attack.performed -= OnAttack;
        inputs.Gameplay.Hurt.performed -= OnHurt;
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
                    PauseMenu pause = FindObjectOfType<PauseMenu>();
                    pause.OpenShop();
                    Debug.Log("Tienda "+interactable.name);
                    break;
                /*case "npcCompra":
                    //abrir tienda de compra
                    Debug.Log("Compra "+interactable.name);
                    break;*/
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
        //combat.PlayerReceiveDamage(10,transform.position,"Physical");
        // Spawner un item
        //ItemSpawner.Instance.SpawnItem(2, 5, transform.position);

        // Guardar inventario
        /*FindObjectOfType<InventoryManager>().SaveInventoryToPlayer();
        PlayerStatsLoader.GuardarStatsAJson();

        // Cargar inventario
        PlayerStatsLoader.CargarYAplicarStatsDesdeJson();*/
        //OnHurt();
    }
    
    public void OnHurt() {
        movementController.StopMovement();
        animationController.OnHurt();
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

    public void Save()
    {
        ScriptablePlayer playerData = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        playerData.cryptos = money.GetCryptos();
        playerData.health = combat.health.GetHp();
        Debug.Log("Guardados " + playerData.health + " - " + playerData.cryptos);
        playerData.inventoryData = inventoryManager.GetInventoryData();
        playerData.lastScene = SceneManager.GetActiveScene().name;
        PlayerStatsLoader.GuardarStatsAJson();
    }
    
    void OnSceneChanged(Scene prev, Scene next)
    {
        if (prev.name == "MainMenu")
            Save();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void OnDestroy()
    {
        Save();
    }
}
