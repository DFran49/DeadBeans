using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Enemies/Data/DropTables/DropTable")]
public class ScriptableDropTable : ScriptableObject
{
    [Header("Identificación")] 
    public int enemy_id;
    public int item_id;

    [Header("Atributos")]
    [HideInInspector] public int max_quantity;
    [HideInInspector] public int min_quantity;
    [HideInInspector] public int drop_rate;
    
    public void AplicarDatosDesdeJson(DropTable table)
    {
        if (table == null)
        {
            Debug.LogWarning($"DropTable no válido o nombre no coincide con el ScriptableObject: {this.name}");
            return;
        }

        max_quantity = table.max_quantity;
        min_quantity = table.min_quantity;
        drop_rate = table.drop_rate;
    }
}
