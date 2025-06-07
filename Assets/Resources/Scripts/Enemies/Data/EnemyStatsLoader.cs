using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/enemies.json";
    private const string resourcesItemsPath = "Scripts/Inventory/Items";

    public static void CargarYAplicarStatsDesdeJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"[ItemStatsLoader] No se encontró el archivo JSON en {jsonPath}");
            return;
        }

        string jsonText = File.ReadAllText(jsonPath);

        EnemyStatsList enemiesList;
        try
        {
            enemiesList = JsonUtility.FromJson<EnemyStatsList>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[EnemyStatsLoader] Error al parsear JSON: {e.Message}");
            return;
        }

        if (enemiesList == null || enemiesList.enemies == null)
        {
            Debug.LogError("[ItemStatsLoader] Lista de items vacía o mal formateada.");
            return;
        }

        // Diccionario int->ItemStats
        Dictionary<int, EnemyStats> enemiesDict = new Dictionary<int, EnemyStats>();
        foreach (var stat in enemiesList.enemies)
        {
            Debug.Log($"ItemStats id leído: '{stat.enemy_id}'");
            if (stat.enemy_id != 0)
                enemiesDict[stat.enemy_id] = stat;
        }

        ScriptableEnemy[] items = Resources.LoadAll<ScriptableEnemy>(resourcesItemsPath);
        foreach (var item in items)
        {
            if (item.enemy_id == 0) return;

            if (enemiesDict.TryGetValue(item.enemy_id, out var stat))
            {
                Debug.Log($"Cargando item {stat.enemy_id}");
                item.AplicarDatosDesdeJson(stat);
                Debug.Log($"[ItemStatsLoader] Aplicados datos a '{item.enemy_id}'");
            }
            else
            {
                Debug.LogWarning($"[EnemyStatsLoader] No se encontraron datos JSON para el item '{item.enemy_id}'");
            }
        }
    }
}
