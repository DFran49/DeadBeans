using UnityEngine;

[System.Serializable]
public class DropTable
{
    public int item_id;
    public string item_name;
    public int max_quantity;
    public int min_quantity;
    public int drop_rate;
    
    // Constructor por defecto
    public DropTable()
    {
    }
    
    // Constructor con parámetros para facilitar la creación
    public DropTable(int itemId, string itemName, int maxQty, int minQty, int dropRate)
    {
        item_id = itemId;
        item_name = itemName;
        max_quantity = maxQty;
        min_quantity = minQty;
        drop_rate = dropRate;
    }
}