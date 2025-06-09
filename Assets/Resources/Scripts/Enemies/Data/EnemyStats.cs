using System.Collections.Generic;

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
    public float speed;  // Cambiado a float para coincidir con el JSON
    public float attack_speed;
    public int def;
    public int magic_def;
    
    // DropTables - Cambiado a lista directa para coincidir con el JSON
    public List<DropTable> drops;
    
    // Método para convertir a DropTableList si es necesario
    public DropTableList GetDropTableList()
    {
        DropTableList dropTableList = new DropTableList();
        dropTableList.drops = drops ?? new List<DropTable>();
        return dropTableList;
    }
}