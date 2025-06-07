using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiCalls
{
    private static readonly Dictionary<string, string> endpoints = new Dictionary<string, string>
    {
        { "https://apis.dfran49.com/API/crud/items/leer.php", "items.json" },
        { "https://apis.dfran49.com/API/crud/players/leer.php", "player_leaderboard.json" },
        { "https://apis.dfran49.com/API/crud/enemies/leer.php", "enemies.json" },
        //todo Juntar todos los items en un solo endpoint
        { "https://apis.dfran49.com/API/crud/weapons/leer.php", "enemies.json" },
        { "https://apis.dfran49.com/API/crud/armors /leer.php", "enemies.json" },
        { "https://apis.dfran49.com/API/crud/consumables/leer.php", "enemies.json" },
        { "https://apis.dfran49.com/API/crud/materials/leer.php", "enemies.json" },
        //todo Todo lo de sells
        { "https://apis.dfran49.com/API/crud/sells/leer.php", "sells.json" }
    };

    public static void FetchAll(MonoBehaviour caller, Action<int, int> onProgress, Action onComplete)
    {
        caller.StartCoroutine(FetchAllEndpoints(onProgress, onComplete));
    }

    private static IEnumerator FetchAllEndpoints(Action<int, int> onProgress, Action onComplete)
    {
        int total = endpoints.Count;
        int completed = 0;

        foreach (var entry in endpoints)
        {
            yield return FetchAndSave(entry.Key, entry.Value);
            completed++;

            onProgress?.Invoke(completed, total);
        }

        onComplete?.Invoke();
    }

    private static IEnumerator FetchAndSave(string url, string filename)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError("Error fetching " + url + ": " + request.error);
        }
        else
        {
            string path = Path.Combine(Application.persistentDataPath, filename);
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(path, request.downloadHandler.text);
            Debug.Log("Guardado: " + path);
        }
    }
}