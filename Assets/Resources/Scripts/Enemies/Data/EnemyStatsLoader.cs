using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class EnemyStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/enemies.json";
    private const string resourcesEnemiesPath = "Scripts/Enemies/Data/Enemies";
    private const string assetsEnemiesPath = "Assets/Resources/Scripts/Enemies/Data/Enemies"; // Ruta completa en Assets

    public static void CargarYAplicarStatsDesdeJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"[EnemyStatsLoader] No se encontró el archivo JSON en {jsonPath}");
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
            Debug.LogError("[EnemyStatsLoader] Lista de enemigos vacía o mal formateada.");
            return;
        }

#if UNITY_EDITOR
        // Crear directorio si no existe
        if (!Directory.Exists(assetsEnemiesPath))
        {
            Directory.CreateDirectory(assetsEnemiesPath);
            AssetDatabase.Refresh();
        }

        LimpiarEnemigosExistentes();

        // Crear ScriptableObjects desde el JSON
        foreach (var stat in enemiesList.enemies)
        {
            if (stat.enemy_id == 0) continue;

            Debug.Log($"Creando ScriptableObject para enemigo {stat.enemy_id}");
            
            // Crear nuevo ScriptableObject
            ScriptableEnemy nuevoEnemigo = ScriptableObject.CreateInstance<ScriptableEnemy>();
            
            // Asignar el ID primero
            nuevoEnemigo.enemy_id = stat.enemy_id;
            
            // Aplicar todos los datos del JSON
            nuevoEnemigo.AplicarDatosDesdeJson(stat);
            
            // Generar nombre de archivo que incluya el nombre del enemigo para facilitar la búsqueda
            string enemyName = SanitizeFileName(stat.name);
            string fileName = $"{enemyName}_Enemy_{stat.enemy_id}.asset";
            string fullPath = Path.Combine(assetsEnemiesPath, fileName);
            
            // Crear el asset
            AssetDatabase.CreateAsset(nuevoEnemigo, fullPath);
            
            Debug.Log($"[EnemyStatsLoader] Creado ScriptableObject '{fileName}' para enemigo {stat.name} (ID {stat.enemy_id})");
        }
        
        // Guardar y refrescar la base de datos de assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"[EnemyStatsLoader] Proceso completado. Creados {enemiesList.enemies.Count} ScriptableObjects de enemigos.");
#else
        Debug.LogWarning("[EnemyStatsLoader] La creación de ScriptableObjects solo funciona en el Editor de Unity.");
#endif
    }

    #if UNITY_EDITOR
    // Método opcional para limpiar enemigos existentes
    private static void LimpiarEnemigosExistentes()
    {
        if (!Directory.Exists(assetsEnemiesPath)) return;

        string[] existingAssets = Directory.GetFiles(assetsEnemiesPath, "*.asset");
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
        
        Debug.Log("[EnemyStatsLoader] Enemigos existentes eliminados.");
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