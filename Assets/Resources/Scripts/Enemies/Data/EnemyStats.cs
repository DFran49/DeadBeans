using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    //base fields
    public int enemy_id;
    public string name;
    public string description;
    
    //combat fields
    public int health;
    public int strength;
    public int speed;
    public float attack_speed;
    public int def;
    public int magic_def;
    
    //DropTables
    public DropTableList dropTables;
}
