using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/player.json";
    private const string resourcesItemsPath = "Scripts/Player/Player";

    public static void CargarYAplicarStatsDesdeJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"[PlayerStatsLoader] No se encontró el archivo JSON en {jsonPath}");
            return;
        }

        string jsonText = File.ReadAllText(jsonPath);

        PlayerStats stats;
        try
        {
            stats = JsonUtility.FromJson<PlayerStats>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PlayerStatsLoader] Error al parsear JSON: {e.Message}");
            return;
        }

        if (stats == null)
        {
            Debug.LogError("[PlayerStatsLoader] Lista de items vacía o mal formateada.");
            return;
        }

        ScriptablePlayer item = Resources.Load<ScriptablePlayer>(resourcesItemsPath);
        if (item == null)
        {
            Debug.LogError($"[PlayerStatsLoader] No se encontró el ScriptableEnemy en '{resourcesItemsPath}'");
            return;
        }
        
        Debug.Log($"Cargando item player");
        item.AplicarDatosDesdeJson(stats);
        Debug.Log($"[PlayerStatsLoader] Aplicados datos a player");
    }
    
    public static void GuardarStatsAJson()
    {
        ScriptablePlayer item = Resources.Load<ScriptablePlayer>(resourcesItemsPath);
        if (item == null)
        {
            Debug.LogError($"[PlayerStatsLoader] No se encontró el ScriptablePlayer en '{resourcesItemsPath}'");
            return;
        }

        PlayerStats stats = new PlayerStats
        {
            health = item.health,
            cryptos = item.cryptos,
            tutoCompleted = item.tutoCompleted,
            lastScene = item.lastScene
            // Guardar inventario
        };

        string jsonText = JsonUtility.ToJson(stats, true);
        File.WriteAllText(jsonPath, jsonText);
        Debug.Log($"[PlayerStatsLoader] Guardado player.json correctamente en {jsonPath}");
    }
}
