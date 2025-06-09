using UnityEngine;

[CreateAssetMenu(fileName = "NuevoEnemigo", menuName = "Scripts/Enemies/Data/Enemies/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    [Header("Identificación")] 
    public int enemy_id;
    public string sprite;
    public string animator;

    [Header("Atributos")]
    //base fields
    [HideInInspector] public string name;
    [HideInInspector] public string description;
    //combat fields
    [HideInInspector] public int health;
    [HideInInspector] public int strength;
    [HideInInspector] public float speed;
    [HideInInspector] public float attack_speed;
    [HideInInspector] public int def;
    [HideInInspector] public int magic_def;
    //DropTables
    [HideInInspector] public DropTableList dropTables;
    
    public void AplicarDatosDesdeJson(EnemyStats stats)
    {
        if (stats == null)
        {
            Debug.LogWarning($"EnemyStats no válido para ScriptableObject: {this.name}");
            return;
        }

        // Limpiar valores por defecto antes de asignar
        LimpiarValoresPorDefecto();

        // Aplicar datos del JSON
        name = stats.name;
        description = stats.description;
        health = stats.health;
        strength = stats.strength;
        speed = stats.speed;
        attack_speed = stats.attack_speed;
        def = stats.def;
        magic_def = stats.magic_def;
        
        // Convertir la lista de drops a DropTableList
        dropTables = stats.GetDropTableList();
        
        // Configurar sprite y animator basado en el nombre
        sprite = name + ".png";
        animator = name + ".controller";
        
        Debug.Log($"Aplicando datos de enemigo para ID {stats.enemy_id}: {stats.name} - Drops: {dropTables.drops.Count}");
    }

    // Método para limpiar valores por defecto
    private void LimpiarValoresPorDefecto()
    {
        name = "";
        description = "";
        health = 0;
        strength = 0;
        speed = 0f;
        attack_speed = 0f;
        def = 0;
        magic_def = 0;
        dropTables = new DropTableList();
    }
}