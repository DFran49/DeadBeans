using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Inventory/Items/Item")]
public class Item : ScriptableObject
{
    [Header("Identificación")]
    public int id;           // Clave del JSON, debe coincidir exactamente
    public Sprite icono;

    [Header("Atributos comunes")]
    [HideInInspector] public string name;
    [HideInInspector] public string description;
    [HideInInspector] public int value;
    [HideInInspector] public ItemSubtype subtype;

    [Header("Material")]
    [HideInInspector] public string type;

    [Header("Arma")]
    [HideInInspector] public int strength;
    [HideInInspector] public int range;
    [HideInInspector] public float attack_speed;
    [HideInInspector] public string category;

    [Header("Armadura")]
    [HideInInspector] public int def;
    [HideInInspector] public int magic_def;
    [HideInInspector] public int agility;
    [HideInInspector] public int speed;

    [Header("Consumible")]
    [HideInInspector] public int duration;
    [HideInInspector] public bool reusable;
    [HideInInspector] public int power;
    
    public void AplicarDatosDesdeJson(ItemStats stats)
    {
        if (stats == null /*|| stats.name != this.name*/)
        {
            Debug.LogWarning($"ItemStats no válido o nombre no coincide con el ScriptableObject: {this.name}");
            return;
        }

        // Comunes
        name = stats.name;
        description = stats.description;
        value = stats.value;
        subtype = stats.subtype;

        // Por subtipo
        switch (stats.subtype)
        {
            case ItemSubtype.material:
                type = stats.type;
                Debug.Log("material");
                break;

            case ItemSubtype.weapon:
                strength = stats.strength;
                range = stats.range;
                attack_speed = stats.attack_speed;
                category = stats.category;
                Debug.Log("Weapon");
                break;

            case ItemSubtype.armor:
                def = stats.def;
                magic_def = stats.magic_def;
                agility = stats.agility;
                speed = stats.speed;
                Debug.Log("Armor");
                break;

            case ItemSubtype.consumable:
                duration = stats.duration;
                reusable = stats.reusable;
                power = stats.power;
                Debug.Log("Consumible");
                break;
        }
    }
}