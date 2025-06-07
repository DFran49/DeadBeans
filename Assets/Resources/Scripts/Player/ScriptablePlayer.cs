using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Player/Player")]
public class ScriptablePlayer : ScriptableObject
{
    [Header("Atributos")]
    //combat fields
    [HideInInspector] public int health;
    [HideInInspector] public int cryptos;
    [HideInInspector] public bool tutoCompleted;
    [HideInInspector] public string lastScene;
    //Meeter inventario
    
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
        //Cargar inventario
    }
}
