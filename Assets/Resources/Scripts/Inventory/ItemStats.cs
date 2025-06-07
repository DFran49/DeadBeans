using UnityEngine;

[System.Serializable]
public class ItemStats
{
    //supertype fields
    public int id;
    public string name;
    public string description;
    public int value;
    public ItemSubtype subtype;
    
    //material fields "weapon", "armor", "material", "consumable"
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
