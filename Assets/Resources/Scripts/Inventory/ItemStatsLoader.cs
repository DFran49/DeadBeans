using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ItemStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/items.json";
    private const string assetsItemsPath = "Assets/Resources/Scripts/Inventory/Items"; // Ruta completa en Assets

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

#if UNITY_EDITOR
        // Crear directorio si no existe
        if (!Directory.Exists(assetsItemsPath))
        {
            Directory.CreateDirectory(assetsItemsPath);
            AssetDatabase.Refresh();
        }

        LimpiarItemsExistentes();

        // Crear ScriptableObjects desde el JSON
        foreach (var stat in statsList.items)
        {
            if (stat.item_id == 0) continue;

            Debug.Log($"Creando ScriptableObject para item {stat.item_id} con subtype {stat.subtype}");
            
            // Crear nuevo ScriptableObject
            Item nuevoItem = ScriptableObject.CreateInstance<Item>();
            
            // Asignar el ID primero
            nuevoItem.item_id = stat.item_id;
            
            // Aplicar todos los datos del JSON
            nuevoItem.AplicarDatosDesdeJson(stat);
            
            // Generar nombre de archivo único
            string fileName = $"Item_{stat.item_id}_{SanitizeFileName(stat.name)}.asset";
            string fullPath = Path.Combine(assetsItemsPath, fileName);
            
            // Crear el asset
            AssetDatabase.CreateAsset(nuevoItem, fullPath);
            
            Debug.Log($"[ItemStatsLoader] Creado ScriptableObject '{fileName}' para item ID {stat.item_id}");
        }
        
        // Guardar y refrescar la base de datos de assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"[ItemStatsLoader] Proceso completado. Creados {statsList.items.Count} ScriptableObjects.");
#else
        Debug.LogWarning("[ItemStatsLoader] La creación de ScriptableObjects solo funciona en el Editor de Unity.");
#endif
    }

    #if UNITY_EDITOR
    // Método opcional para limpiar items existentes
    private static void LimpiarItemsExistentes()
    {
        if (!Directory.Exists(assetsItemsPath)) return;

        string[] existingAssets = Directory.GetFiles(assetsItemsPath, "*.asset");
        foreach (string assetPath in existingAssets)
        {
            // Convertir ruta del sistema a ruta de Unity
            string unityPath = assetPath.Replace('\\', '/');
            if (unityPath.StartsWith(Application.dataPath))
            {
                unityPath = "Assets" + unityPath.Substring(Application.dataPath.Length);
            }
            
            AssetDatabase.DeleteAsset(unityPath);
        }
        
        Debug.Log("[ItemStatsLoader] Items existentes eliminados.");
    }
    
    // Método para sanitizar nombres de archivo
    private static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return "Unknown";
        
        // Caracteres no válidos para nombres de archivo
        char[] invalidChars = Path.GetInvalidFileNameChars();
        string sanitized = fileName;
        
        foreach (char c in invalidChars)
        {
            sanitized = sanitized.Replace(c, '_');
        }
        
        // Limitar longitud y quitar espacios
        sanitized = sanitized.Trim().Replace(' ', '_');
        if (sanitized.Length > 50) sanitized = sanitized.Substring(0, 50);
        
        return sanitized;
    }
    #endif
}