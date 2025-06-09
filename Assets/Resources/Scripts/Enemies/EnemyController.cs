using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyGenerator generator;
    private EnemyMovement movement;
    private CombatController combat;
    private EnemyAttack attack;
    private GameObject player;
    
    [Header("Enemy Data")]
    public ScriptableEnemy enemyData; // Referencia al ScriptableObject del enemigo
    
    private void Awake()
    {
        combat = GetComponent<CombatController>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponentInChildren<EnemyAttack>();
        
        // Cargar datos del ScriptableObject basado en el nombre del prefab
        LoadEnemyDataFromScriptableObject();
    }

    private void Start()
    {
        attack.Initialize(combat);
        
        // Suscribirse al evento de muerte del HealthComponent
        if (combat != null && combat.health != null)
        {
            combat.health.OnDeath += HandleEnemyDeath;
        }
    }

    private void LoadEnemyDataFromScriptableObject()
    {
        // Obtener el nombre del prefab (sin espacios y en minúsculas para consistencia)
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim().ToLower();
        
        // Buscar el ScriptableObject que contenga el nombre del prefab
        ScriptableEnemy[] allEnemyData = Resources.LoadAll<ScriptableEnemy>("Scripts/Enemies/Data/Enemies");
        
        foreach (ScriptableEnemy data in allEnemyData)
        {
            if (data.name.ToLower().Contains(prefabName))
            {
                enemyData = data;
                ApplyEnemyStats(data);
                Debug.Log($"Datos cargados para {prefabName} desde {data.name}");
                break;
            }
        }
        
        if (enemyData == null)
        {
            Debug.LogWarning($"No se encontró ScriptableObject para el enemigo: {prefabName}");
        }
    }

    private void ApplyEnemyStats(ScriptableEnemy data)
    {
        if (data == null || combat == null || combat.stats == null) return;
        
        // Aplicar stats del ScriptableObject al StatsComponent
        combat.stats.maxHp = data.health;
        combat.stats.str = data.strength;
        combat.stats.spd = data.speed;
        combat.stats.def = data.def;
        combat.stats.mgcDef = data.magic_def;
        combat.stats.atkSpeed = Mathf.RoundToInt(data.attack_speed);
        
        // Reinicializar el HealthComponent con las nuevas stats
        if (combat.health != null)
        {
            combat.health.Initialize(combat.stats);
        }
        
        Debug.Log($"Stats aplicadas a {gameObject.name}: HP={data.health}, STR={data.strength}, SPD={data.speed}");
    }

    private void HandleEnemyDeath()
    {
        Debug.Log($"{gameObject.name} ha muerto. Generando drops...");
        GenerateDrops();
    }

    private void GenerateDrops()
    {
        if (enemyData == null || enemyData.dropTables == null || enemyData.dropTables.drops == null)
        {
            Debug.Log($"No hay drop tables para {gameObject.name}");
            return;
        }

        Debug.Log($"Procesando {enemyData.dropTables.drops.Count} drop tables para {gameObject.name}");

        foreach (DropTable dropTable in enemyData.dropTables.drops)
        {
            // Calcular si el item dropea basado en el drop_rate (asumiendo que es un porcentaje de 0-100)
            int randomChance = UnityEngine.Random.Range(0, 101);
            
            if (randomChance <= dropTable.drop_rate)
            {
                // Calcular cantidad aleatoria entre min y max
                int quantity = UnityEngine.Random.Range(dropTable.min_quantity, dropTable.max_quantity + 1);
                
                // Log del item generado
                Debug.Log($"¡DROP GENERADO! Item ID: {dropTable.item_id}, Cantidad: {quantity}, Probabilidad: {dropTable.drop_rate}%");
                
                // *** NUEVA FUNCIONALIDAD: Spawnear el item físicamente ***
                SpawnDroppedItem(dropTable.item_id, quantity);
            }
            else
            {
                Debug.Log($"Item ID: {dropTable.item_id} no dropeó (Probabilidad: {dropTable.drop_rate}%, Roll: {randomChance})");
            }
        }
    }
    
    private void SpawnDroppedItem(int itemId, int quantity)
    {
        // Verificar que existe el ItemSpawner
        if (ItemSpawner.Instance != null)
        {
            // Spawnear en la posición del enemigo (convertir a Vector2 para 2D)
            ItemSpawner.Instance.SpawnItem(itemId, quantity, (Vector2)transform.position);
        }
        else
        {
            Debug.LogError("EnemyController: No se encontró ItemSpawner en la escena!");
            Debug.Log($"FALLBACK - Item que se habría spawneado: ID {itemId}, Cantidad: {quantity}");
        }
    }

    public void Initialize(GameObject player)
    {
        this.player = player;
        movement.SetPlayer(player);
    }

    public bool PlayerExists()
    {
        return player != null;
    }

    public void StopEnemies()
    {
        movement.SetSpeed(0);
    }

    public void StartEnemies()
    {
        movement.SetSpeed(combat.getSpd());
    }

    public void EndHurtEnemy()
    {
        GetComponent<Animator>().SetFloat("State", 0);
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar memory leaks
        if (combat != null && combat.health != null)
        {
            combat.health.OnDeath -= HandleEnemyDeath;
        }
    }
}