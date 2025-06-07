using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ItemStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/items.json";
    private const string resourcesItemsPath = "Scripts/Inventory/Items";

    public static void CargarYAplicarStatsDesdeJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"[ItemStatsLoader] No se encontró el archivo JSON en {jsonPath}");
            return;
        }

        string jsonText = File.ReadAllText(jsonPath);

        ItemStatsList statsList;
        try
        {
            statsList = JsonUtility.FromJson<ItemStatsList>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ItemStatsLoader] Error al parsear JSON: {e.Message}");
            return;
        }

        if (statsList == null || statsList.items == null)
        {
            Debug.LogError("[ItemStatsLoader] Lista de items vacía o mal formateada.");
            return;
        }

        // Diccionario int->ItemStats
        Dictionary<int, ItemStats> statsDict = new Dictionary<int, ItemStats>();
        foreach (var stat in statsList.items)
        {
            Debug.Log($"ItemStats id leído: '{stat.id}'");
            if (stat.id != 0)
                statsDict[stat.id] = stat;
        }

        Item[] items = Resources.LoadAll<Item>(resourcesItemsPath);
        foreach (var item in items)
        {
            if (item.id == 0) return;

            if (statsDict.TryGetValue(item.id, out var stat))
            {
                Debug.Log($"Cargando item {stat.id} con subtype {stat.subtype}");
                item.AplicarDatosDesdeJson(stat);
                Debug.Log($"[ItemStatsLoader] Aplicados datos a '{item.id}'");
            }
            else
            {
                Debug.LogWarning($"[ItemStatsLoader] No se encontraron datos JSON para el item '{item.id}'");
            }
        }
    }
}