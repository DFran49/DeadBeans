using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    public int itemId;
    public int quantity;
    
    public InventorySlotData()
    {
        itemId = 0;
        quantity = 0;
    }
    
    public InventorySlotData(int id, int qty)
    {
        itemId = id;
        quantity = qty;
    }
    
    public bool IsEmpty()
    {
        return itemId == 0 || quantity <= 0;
    }
    
    public void Clear()
    {
        itemId = 0;
        quantity = 0;
    }
}