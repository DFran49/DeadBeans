using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    [Header("Storage Inventory")]
    public List<InventorySlotData> storageSlots;
    
    [Header("Equipment Slots")]
    public InventorySlotData weaponSlot;
    public InventorySlotData armorSlot;
    public InventorySlotData consumableSlot;
    
    public InventoryData()
    {
        // Inicializar 25 slots de almacenamiento
        storageSlots = new List<InventorySlotData>();
        for (int i = 0; i < 25; i++)
        {
            storageSlots.Add(new InventorySlotData());
        }
        
        // Inicializar slots de equipamiento
        weaponSlot = new InventorySlotData();
        armorSlot = new InventorySlotData();
        consumableSlot = new InventorySlotData();
    }
    
    // NUEVO: Método para obtener el límite máximo por tipo de item
    private int GetMaxStackSize(ItemSubtype itemType)
    {
        switch (itemType)
        {
            case ItemSubtype.material:
                return 10;
            case ItemSubtype.consumable:
                return 5;
            case ItemSubtype.weapon:
            case ItemSubtype.armor:
                return 1;
            default:
                return 1;
        }
    }
    
    public bool AddItem(int itemId, int quantity)
    {
        Item itemData = GetItemDataById(itemId);
        if (itemData == null) return false;
        
        int maxStackSize = GetMaxStackSize(itemData.subtype);
        int remainingQuantity = quantity;
        
        // Intentar stackear con items existentes del mismo tipo
        for (int i = 0; i < storageSlots.Count && remainingQuantity > 0; i++)
        {
            if (storageSlots[i].itemId == itemId)
            {
                int availableSpace = maxStackSize - storageSlots[i].quantity;
                if (availableSpace > 0)
                {
                    int quantityToAdd = Mathf.Min(remainingQuantity, availableSpace);
                    storageSlots[i].quantity += quantityToAdd;
                    remainingQuantity -= quantityToAdd;
                }
            }
        }
        
        // Si todavía queda cantidad por añadir, buscar slots vacíos
        for (int i = 0; i < storageSlots.Count && remainingQuantity > 0; i++)
        {
            if (storageSlots[i].IsEmpty())
            {
                int quantityToAdd = Mathf.Min(remainingQuantity, maxStackSize);
                storageSlots[i].itemId = itemId;
                storageSlots[i].quantity = quantityToAdd;
                remainingQuantity -= quantityToAdd;
            }
        }
        
        // Retornar true si se añadió algo (aunque no sea todo)
        return remainingQuantity < quantity;
    }
    
    public bool RemoveItem(int slotIndex, int quantity = -1)
    {
        if (slotIndex < 0 || slotIndex >= storageSlots.Count) return false;
        if (storageSlots[slotIndex].IsEmpty()) return false;
        
        if (quantity == -1 || quantity >= storageSlots[slotIndex].quantity)
        {
            storageSlots[slotIndex].Clear();
        }
        else
        {
            storageSlots[slotIndex].quantity -= quantity;
        }
        
        return true;
    }
    
    public bool EquipItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= storageSlots.Count) return false;
        if (storageSlots[slotIndex].IsEmpty()) return false;
        
        Item itemData = GetItemDataById(storageSlots[slotIndex].itemId);
        if (itemData == null) return false;
        
        InventorySlotData targetEquipSlot = null;
        
        switch (itemData.subtype)
        {
            case ItemSubtype.weapon:
                targetEquipSlot = weaponSlot;
                break;
            case ItemSubtype.armor:
                targetEquipSlot = armorSlot;
                break;
            case ItemSubtype.consumable:
                targetEquipSlot = consumableSlot;
                break;
            default:
                return false; // No se puede equipar
        }
        
        // Si ya hay algo equipado, moverlo al inventario
        if (!targetEquipSlot.IsEmpty())
        {
            if (!AddItem(targetEquipSlot.itemId, targetEquipSlot.quantity))
                return false; // No hay espacio
        }
        
        // Equipar el item (solo 1 cantidad)
        targetEquipSlot.itemId = storageSlots[slotIndex].itemId;
        targetEquipSlot.quantity = 1;
        
        // Reducir cantidad en el inventario
        storageSlots[slotIndex].quantity--;
        if (storageSlots[slotIndex].quantity <= 0)
        {
            storageSlots[slotIndex].Clear();
        }
        
        return true;
    }
    
    public bool UnequipItem(ItemSubtype equipType)
    {
        InventorySlotData sourceEquipSlot = null;
        
        switch (equipType)
        {
            case ItemSubtype.weapon:
                sourceEquipSlot = weaponSlot;
                break;
            case ItemSubtype.armor:
                sourceEquipSlot = armorSlot;
                break;
            case ItemSubtype.consumable:
                sourceEquipSlot = consumableSlot;
                break;
            default:
                return false;
        }
        
        if (sourceEquipSlot.IsEmpty()) return false;
        
        // Mover al inventario
        if (AddItem(sourceEquipSlot.itemId, sourceEquipSlot.quantity))
        {
            sourceEquipSlot.Clear();
            return true;
        }
        
        return false; // No hay espacio en el inventario
    }
    
    public bool IsItemEquipped(int itemId)
    {
        return (weaponSlot.itemId == itemId) || 
               (armorSlot.itemId == itemId) || 
               (consumableSlot.itemId == itemId);
    }
    
    // NUEVO: Método público para verificar si se puede añadir una cantidad específica de un item
    public bool CanAddItem(int itemId, int quantity)
    {
        Item itemData = GetItemDataById(itemId);
        if (itemData == null) return false;
        
        int maxStackSize = GetMaxStackSize(itemData.subtype);
        int remainingQuantity = quantity;
        
        // Verificar espacio en stacks existentes
        for (int i = 0; i < storageSlots.Count && remainingQuantity > 0; i++)
        {
            if (storageSlots[i].itemId == itemId)
            {
                int availableSpace = maxStackSize - storageSlots[i].quantity;
                remainingQuantity -= Mathf.Min(remainingQuantity, availableSpace);
            }
        }
        
        // Verificar slots vacíos
        for (int i = 0; i < storageSlots.Count && remainingQuantity > 0; i++)
        {
            if (storageSlots[i].IsEmpty())
            {
                remainingQuantity -= Mathf.Min(remainingQuantity, maxStackSize);
            }
        }
        
        return remainingQuantity <= 0;
    }
    
    // NUEVO: Método para obtener la cantidad total de un item en el inventario
    public int GetItemCount(int itemId)
    {
        int totalCount = 0;
        
        for (int i = 0; i < storageSlots.Count; i++)
        {
            if (storageSlots[i].itemId == itemId)
            {
                totalCount += storageSlots[i].quantity;
            }
        }
        
        return totalCount;
    }
    
    private Item GetItemDataById(int itemId)
    {
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
}