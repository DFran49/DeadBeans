using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerLeaderboardStatsLoader
{
    private readonly static string jsonPath = Application.persistentDataPath + "/player_leaderboard.json";
    private const string resourcesLeaderboardPath = "Scripts/Leaderboard/Players";

    public static void CargarYAplicarStatsDesdeJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"[PlayerLeaderboardStatsLoader] No se encontró el archivo JSON en {jsonPath}");
            return;
        }

        string jsonText = File.ReadAllText(jsonPath);

        PlayerLBStatsList playersList;
        try
        {
            playersList = JsonUtility.FromJson<PlayerLBStatsList>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PlayerLeaderboardStatsLoader] Error al parsear JSON: {e.Message}");
            return;
        }

        if (playersList == null || playersList.playersStats == null)
        {
            Debug.LogError("[PlayerLeaderboardStatsLoader] Lista de items vacía o mal formateada.");
            return;
        }

        // Diccionario int->PlayerLeaderBoardStats
        Dictionary<int, PlayerLeaderboardStats> playersDict = new Dictionary<int, PlayerLeaderboardStats>();
        foreach (var player in playersList.playersStats)
        {
            Debug.Log($"ItemStats id leído: '{player.player_id}'");
            if (player.player_id != 0)
                playersDict[player.player_id] = player;
        }

        PlayerLeaderboard[] items = Resources.LoadAll<PlayerLeaderboard>(resourcesLeaderboardPath);
        foreach (var player in items)
        {
            if (player.player_id == 0) return;

            if (playersDict.TryGetValue(player.player_id, out var stat))
            {
                Debug.Log($"Cargando jugador {stat.player_id}");
                player.AplicarDatosDesdeJson(stat);
                Debug.Log($"[PlayerLeaderboardStatsLoader] Aplicados datos a '{player.player_id}'");
            }
            else
            {
                Debug.LogWarning($"[PlayerLeaderboardStatsLoader] No se encontraron datos JSON para el jugador '{player.player_id}'");
            }
        }
    }
}