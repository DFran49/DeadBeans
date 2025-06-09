using UnityEngine;

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Scripts/Inventory/Items/Item")]
public class Item : ScriptableObject
{
    [Header("Identificación")]
    public int item_id;           // Clave del JSON, debe coincidir exactamente
    public string icono;

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
        if (stats == null)
        {
            Debug.LogWarning($"ItemStats no válido para ScriptableObject: {this.name}");
            return;
        }

        // Comunes
        name = stats.name;
        description = stats.description;
        value = stats.value;
        subtype = stats.subtype;
        icono = name + ".asset";

        // Limpiar valores por defecto antes de asignar
        LimpiarValoresPorDefecto();

        // Por subtipo
        switch (stats.subtype)
        {
            case ItemSubtype.material:
                type = stats.type;
                Debug.Log($"Aplicando datos de material para item {stats.item_id}");
                break;

            case ItemSubtype.weapon:
                strength = stats.strength;
                range = stats.range;
                attack_speed = stats.attack_speed;
                category = stats.category;
                Debug.Log($"Aplicando datos de arma para item {stats.item_id}");
                break;

            case ItemSubtype.armor:
                def = stats.def;
                magic_def = stats.magic_def;
                agility = stats.agility;
                speed = stats.speed;
                Debug.Log($"Aplicando datos de armadura para item {stats.item_id}");
                break;

            case ItemSubtype.consumable:
                duration = stats.duration;
                reusable = stats.reusable;
                power = stats.power;
                Debug.Log($"Aplicando datos de consumible para item {stats.item_id}");
                break;
        }
    }

    // Método para limpiar valores que no corresponden al subtipo actual
    private void LimpiarValoresPorDefecto()
    {
        // Material
        type = "";
        
        // Arma
        strength = 0;
        range = 0;
        attack_speed = 0f;
        category = "";
        
        // Armadura
        def = 0;
        magic_def = 0;
        agility = 0;
        speed = 0;
        
        // Consumible
        duration = 0;
        reusable = false;
        power = 0;
    }
}