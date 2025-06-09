using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject droppedItemPrefab; // Prefab base para items dropeados
    
    [Header("Spawn Settings")]
    public float spawnRadius = 0.5f; // Reducido para spawn más cercano
    public float spawnHeightOffset = 0.2f; // Reducido para menor separación vertical
    public LayerMask groundLayer = 1;
    
    private static ItemSpawner instance;
    
    public static ItemSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemSpawner>();
                if (instance == null)
                {
                    GameObject spawnerObj = new GameObject("ItemSpawner");
                    instance = spawnerObj.AddComponent<ItemSpawner>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void SpawnItem(int itemId, int quantity, Vector2 spawnPosition)
    {
        if (droppedItemPrefab == null)
        {
            Debug.LogError("ItemSpawner: droppedItemPrefab no está asignado!");
            return;
        }
        
        // Buscar los datos del item
        Item itemData = GetItemDataById(itemId);
        
        if (itemData == null)
        {
            Debug.LogWarning($"ItemSpawner: No se encontraron datos para el item ID {itemId}");
            // Aún así spawear el item con datos básicos
        }
        
        // CAMBIO PRINCIPAL: Usar Random.Range con valores específicos para mayor control
        Vector2 randomOffset = new Vector2(
            Random.Range(-1f, 1f), // Entre -1 y +1 en X
            Random.Range(-1f, 1f)  // Entre -1 y +1 en Y
        );
        
        Vector2 finalSpawnPosition = spawnPosition + randomOffset;
        
        // Ajustar altura al suelo si es necesario (raycast 2D hacia abajo)
        // Solo aplicar offset vertical si hay colisión con el suelo
        RaycastHit2D hit = Physics2D.Raycast(finalSpawnPosition + Vector2.up * 5f, Vector2.down, 10f, groundLayer);
        if (hit.collider != null)
        {
            finalSpawnPosition.y = hit.point.y + spawnHeightOffset;
        }
        
        // Crear el item dropeado
        GameObject droppedItem = Instantiate(droppedItemPrefab, finalSpawnPosition, Quaternion.identity);
        
        // Configurar el componente DroppedItem
        DroppedItem droppedItemComponent = droppedItem.GetComponent<DroppedItem>();
        if (droppedItemComponent != null)
        {
            droppedItemComponent.Initialize(itemId, quantity, itemData);
        }
        else
        {
            Debug.LogError("ItemSpawner: El prefab droppedItemPrefab debe tener el componente DroppedItem!");
        }
        
        Debug.Log($"ItemSpawner: Spaweado item ID {itemId} x{quantity} en posición {finalSpawnPosition}");
    }
    
    // Sobrecarga del método para permitir spawn con rango personalizado
    public void SpawnItem(int itemId, int quantity, Vector2 spawnPosition, float customRange)
    {
        if (droppedItemPrefab == null)
        {
            Debug.LogError("ItemSpawner: droppedItemPrefab no está asignado!");
            return;
        }
        
        Item itemData = GetItemDataById(itemId);
        
        // Usar el rango personalizado
        Vector2 randomOffset = new Vector2(
            Random.Range(-customRange, customRange),
            Random.Range(-customRange, customRange)
        );
        
        Vector2 finalSpawnPosition = spawnPosition + randomOffset;
        
        RaycastHit2D hit = Physics2D.Raycast(finalSpawnPosition + Vector2.up * 5f, Vector2.down, 10f, groundLayer);
        if (hit.collider != null)
        {
            finalSpawnPosition.y = hit.point.y + spawnHeightOffset;
        }
        
        GameObject droppedItem = Instantiate(droppedItemPrefab, finalSpawnPosition, Quaternion.identity);
        
        DroppedItem droppedItemComponent = droppedItem.GetComponent<DroppedItem>();
        if (droppedItemComponent != null)
        {
            droppedItemComponent.Initialize(itemId, quantity, itemData);
        }
        
        Debug.Log($"ItemSpawner: Spaweado item ID {itemId} x{quantity} en posición {finalSpawnPosition} con rango {customRange}");
    }
    
    private Item GetItemDataById(int itemId)
    {
        // Buscar el ScriptableObject del item por ID
        Item[] allItems = Resources.LoadAll<Item>("Scripts/Inventory/Items");
        
        foreach (Item item in allItems)
        {
            if (item.item_id == itemId)
            {
                return item;
            }
        }
        
        return null;
    }
    
    // Método para visualizar el radio de spawn en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}