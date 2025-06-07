using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Enemies/Data/Enemies/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    [Header("Identificación")] 
    public int enemy_id;

    [Header("Atributos")]
    //base fields
    [HideInInspector] public string name;
    [HideInInspector] public string description;
    //combat fields
    [HideInInspector] public int health;
    [HideInInspector] public int strength;
    [HideInInspector] public int speed;
    [HideInInspector] public float attack_speed;
    [HideInInspector] public int def;
    [HideInInspector] public int magic_def;
    //DropTables
    [HideInInspector] public DropTableList dropTables;
    
    public void AplicarDatosDesdeJson(EnemyStats stats)
    {
        if (stats == null)
        {
            Debug.LogWarning($"EnemyStats no válido o nombre no coincide con el ScriptableObject: {this.name}");
            return;
        }

        name = stats.name;
        description = stats.description;
        health = stats.health;
        strength = stats.strength;
        speed = stats.speed;
        attack_speed = stats.attack_speed;
        def = stats.def;
        magic_def = stats.magic_def;
        dropTables = stats.dropTables;
    }
}
