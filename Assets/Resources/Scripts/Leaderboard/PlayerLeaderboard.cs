using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPlayerLeaderboard", menuName = "Scripts/Leaderboard/Players/PlayerLeaderboard")]
public class PlayerLeaderboard : ScriptableObject
{
    [Header("Identificación")]
    public int player_id;           // Clave del JSON, debe coincidir exactamente

    [Header("Atributos")]
    [HideInInspector] public string username;
    [HideInInspector] public int level;
    [HideInInspector] public int cryptos;
    [HideInInspector] public DateTime last_connection;
    [HideInInspector] public int runs;
    [HideInInspector] public int experience;

    public void AplicarDatosDesdeJson(PlayerLeaderboardStats stats)
    {
        if (stats == null /*|| stats.name != this.name*/)
        {
            Debug.LogWarning($"PlayerLeaderboardStats no válido o nombre no coincide con el ScriptableObject: {this.name}");
            return;
        }

        // Atributos
        username = stats.username;
        level = stats.level;
        cryptos = stats.cryptos;
        last_connection = stats.last_connection;
        runs = stats.runs;
        experience = stats.experience;
    }
}