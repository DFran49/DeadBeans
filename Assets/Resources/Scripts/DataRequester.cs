using System;
using System.IO;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataRequester : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    private void Awake()
    {
        PlayerStatsLoader.CargarYAplicarStatsDesdeJson();
    }

    public void LoadData()
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        progressBar.value = 0;
        progressText.text = "0%";

        ApiCalls.FetchAll(
            this,
            onProgress: (completed, total) =>
            {
                float progress = (float)completed / total;
                progressBar.value = progress;
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
                Thread.Sleep(200);
            },
            onComplete: () =>
            {
                loadingScreen.SetActive(false);
                Debug.Log("¡Todo cargado!");
                //todo Faltan algunos itemss
                ItemStatsLoader.CargarYAplicarStatsDesdeJson();
                EnemyStatsLoader.CargarYAplicarStatsDesdeJson();
                progressText.text = "Lectura completada!";
                progressText.fontSize = 72;
                Thread.Sleep(1000);

                
                ScriptablePlayer player = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
                
                string scene = "City";
                Debug.Log("Loading scene " + scene);
                if (player != null)
                    scene = player.lastScene;
                Debug.Log(player.lastScene);
                SceneManager.LoadScene(scene);
            }
        );
    }
    
    private const string assetsItemsPath = "Assets/Resources/Scripts/Enemies/Data/Enemies";

    /*[MenuItem("Herramientas/Mostrar enemies")]
    public static void MostrarItemsDesdeRuta()
    {
        if (!Directory.Exists(assetsItemsPath))
        {
            Debug.LogWarning($"La ruta no existe: {assetsItemsPath}");
            return;
        }

        string[] files = Directory.GetFiles(assetsItemsPath, "*.asset", SearchOption.AllDirectories);

        foreach (string filePath in files)
        {
            ScriptableEnemy item = AssetDatabase.LoadAssetAtPath<ScriptableEnemy>(filePath);
            if (item != null)
            {
                Debug.Log($"ID: {item.enemy_id} | Descripción: {item.description} | Drops: {item.dropTables.drops.Count} | Ruta: {filePath}");
            }
        }
    }*/
}

