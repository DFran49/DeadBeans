using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Player/Player")]
public class ScriptablePlayer : ScriptableObject
{
    [Header("Atributos")]
    //combat fields
    [HideInInspector] public int health = 0;
    [HideInInspector] public int cryptos;
    [HideInInspector] public bool tutoCompleted;
    [HideInInspector] public string lastScene;
    
    [Header("Inventario")]
    [HideInInspector] public InventoryData inventoryData;
    
    public void AplicarDatosDesdeJson(PlayerStats stats)
    {
        if (stats == null)
        {
            Debug.LogWarning($"PlayerStats no válido o nombre no coincide con el ScriptableObject: {this.name}");
            return;
        }

        health = stats.health;
        cryptos = stats.cryptos;
        tutoCompleted = stats.tutoCompleted;
        lastScene = stats.lastScene;
        
        // Cargar inventario
        if (stats.inventoryData != null)
        {
            inventoryData = stats.inventoryData;
        }
        else
        {
            // Inicializar inventario vacío si no existe
            inventoryData = new InventoryData();
        }
        
        Debug.Log($"Inventario {inventoryData.storageSlots[0].itemId}");
    }

    public void InicializarDatosvVacios()
    {
        health = 100;
        cryptos = 0;
        tutoCompleted = false;
        lastScene = "City";
        Debug.Log("Inicializados Datos");
    }

    public bool IsNew()
    {
        if (health == 0)
            return true;
        return false;
    }
}