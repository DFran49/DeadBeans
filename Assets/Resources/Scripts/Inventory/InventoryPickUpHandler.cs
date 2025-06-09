using UnityEngine;

public class InventoryPickupHandler : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public LayerMask itemLayer = 1;
    
    [Header("UI References")]
    public GameObject pickupPrompt;
    
    private InventoryManager inventoryManager;
    private DroppedItem nearestItem;
    private bool canPickup = false;
    
    void Start()
    {
        // Buscar el InventoryManager en la escena
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryPickupHandler: No se encontró InventoryManager en la escena");
        }
        
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }
    
    void Update()
    {
        CheckForNearbyItems();
        HandlePickupInput();
    }
    
    private void CheckForNearbyItems()
    {
        // Buscar items cercanos
        Collider2D[] nearbyItems = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);
        
        DroppedItem closestItem = null;
        float closestDistance = float.MaxValue;
        
        foreach (Collider2D col in nearbyItems)
        {
            DroppedItem droppedItem = col.GetComponent<DroppedItem>();
            if (droppedItem != null)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = droppedItem;
                }
            }
        }
        
        // Actualizar estado
        if (closestItem != nearestItem)
        {
            nearestItem = closestItem;
            canPickup = nearestItem != null;
            
            if (pickupPrompt != null)
                pickupPrompt.SetActive(canPickup);
        }
    }
    
    private void HandlePickupInput()
    {
        if (canPickup && nearestItem != null/* && Input.GetKeyDown(pickupKey)*/)
        {
            PickupItem(nearestItem);
        }
    }
    
    private void PickupItem(DroppedItem item)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryPickupHandler: InventoryManager no encontrado, no se puede recoger el item");
            return;
        }
        
        // Intentar añadir al inventario
        bool success = inventoryManager.AddItemToInventory(item.itemId, item.quantity);
        
        if (success)
        {
            string itemName = item.itemData != null ? item.itemData.name : $"Item ID: {item.itemId}";
            Debug.Log($"¡Item recogido! {itemName} x{item.quantity}");
            
            //todo Ponerle color blanco y que se mueva al jugador
            
            // Destruir el item del mundo
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
            
            Destroy(item.gameObject);
            nearestItem = null;
            canPickup = false;
        }
        else
        {
            Debug.Log("¡Inventario lleno! No se puede recoger el item.");
            // Aquí podrías mostrar una notificación de inventario lleno
        }
    }
    
    // Método público para recoger items (también se puede llamar desde otros scripts)
    public bool TryPickupItem(DroppedItem item)
    {
        if (inventoryManager == null) return false;
        
        bool success = inventoryManager.AddItemToInventory(item.itemId, item.quantity);
        if (success)
        {
            Destroy(item.gameObject);
        }
        return success;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}