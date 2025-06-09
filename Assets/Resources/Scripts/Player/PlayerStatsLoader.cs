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
            Debug.LogWarning($"[PlayerStatsLoader] No se encontró el archivo JSON en {jsonPath}");
            GenerateBaseJson();
        }
        
        if (File.Exists(resourcesItemsPath))
        {
            File.Delete(resourcesItemsPath);
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
            Debug.LogError("[PlayerStatsLoader] Stats de jugador vacíos o mal formateados.");
            return;
        }

        ScriptablePlayer item = Resources.Load<ScriptablePlayer>(resourcesItemsPath);
        if (item == null)
        {
            Debug.LogWarning($"[PlayerStatsLoader] No se encontró el ScriptablePlayer en '{resourcesItemsPath}', creando uno nuevo.");
            item = ScriptableObject.CreateInstance<ScriptablePlayer>();
            
            // Crear la ruta completa en Assets/Resources/Scripts/Player/
            string fullPath = "Assets/Resources/" + resourcesItemsPath + ".asset";
            string directory = Path.GetDirectoryName(fullPath);
            
            // Crear el directorio si no existe
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
#if UNITY_EDITOR
            // Crear el asset en el proyecto (solo funciona en el editor)
            UnityEditor.AssetDatabase.CreateAsset(item, fullPath);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"[PlayerStatsLoader] ScriptablePlayer creado en '{fullPath}'");
#endif
            Debug.Log("A iniz");
            item.InicializarDatosvVacios();
            GuardarStatsAJson();
        }
        
        Debug.Log($"Cargando datos del jugador");
        item.AplicarDatosDesdeJson(stats);
        Debug.Log($"[PlayerStatsLoader] Aplicados datos a jugador");
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
            lastScene = item.lastScene,
            // Guardar inventario
            inventoryData = item.inventoryData
        };

        string jsonText = JsonUtility.ToJson(stats, true);
        File.WriteAllText(jsonPath, jsonText);
        Debug.Log($"[PlayerStatsLoader] Guardado player.json correctamente en {jsonPath}");
    }
    
    public static void GuardarStatsAJsonBase()
    {
        ScriptablePlayer item = Resources.Load<ScriptablePlayer>("Scripts/Player/PlayerBase");
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
            lastScene = item.lastScene,
            // Guardar inventario
            inventoryData = item.inventoryData
        };

        string jsonText = JsonUtility.ToJson(stats, true);
        File.WriteAllText(jsonPath, jsonText);
        Debug.Log($"[PlayerStatsLoader] Guardado player.json correctamente en {jsonPath}");
    }

    public static void GenerateBaseJson()
    {
        TextAsset playerJson = Resources.Load<TextAsset>("Scripts/Player/player");
        if (playerJson == null)
        {
            Debug.LogError($"[PlayerStatsLoader] No se pudo cargar el archivo desde Resources en '{resourcesItemsPath}.json'");
            return;
        }
            
        Debug.Log(playerJson.text);

        File.WriteAllText(jsonPath, playerJson.text);
    }
}