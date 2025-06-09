using System.IO;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DataRequester : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    public ScriptableEnemy item1;
    public ScriptableEnemy item2;

    void Start()
    {
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
            },
            onComplete: () =>
            {
                Thread.Sleep(3000);
                loadingScreen.SetActive(false);
                Debug.Log("¡Todo cargado!");
                //todo Faltan algunos itemss
                ItemStatsLoader.CargarYAplicarStatsDesdeJson();
                EnemyStatsLoader.CargarYAplicarStatsDesdeJson();
                //todo Sacar la lista o los items del json
                //PlayerLeaderboardStatsLoader.CargarYAplicarStatsDesdeJson();
                
                MostrarItemsDesdeRuta();
            }
        );
    }
    
    private const string assetsItemsPath = "Assets/Resources/Scripts/Enemies/Data/Enemies";

    [MenuItem("Herramientas/Mostrar enemies")]
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
    }
}

