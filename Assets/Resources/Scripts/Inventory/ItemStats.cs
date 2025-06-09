using UnityEngine;

[System.Serializable]
public class ItemStats
{
    //supertype fields
    public int item_id;
    public string name;
    public string description;
    public int value;
    public string item_type;
    
    // Propiedad para convertir string a enum
    public ItemSubtype subtype
    {
        get
        {
            switch(item_type)
            {
                case "weapon": return ItemSubtype.weapon;
                case "armor": return ItemSubtype.armor;
                case "material": return ItemSubtype.material;
                case "consumable": return ItemSubtype.consumable;
                default: 
                    Debug.LogWarning($"Tipo de item desconocido: {item_type}");
                    return ItemSubtype.material;
            }
        }
    }
    
    //material fields
    public string type;
    
    //weapon fields
    public int strength;
    public int range;
    public float attack_speed;
    public string category;
    
    //armor fields
    public int def;
    public int magic_def;
    public int agility;
    public int speed;
    
    //consumable fields
    public int duration;
    public bool reusable;
    public int power;
}
