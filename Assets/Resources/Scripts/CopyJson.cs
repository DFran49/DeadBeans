using System.IO;
using UnityEngine;

public class CopyJson : MonoBehaviour
{
    /*public Item item;
    public Item item2;*/
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string persistentPath = Application.persistentDataPath + "/items.json";
        string streamingPath = Path.Combine(Application.streamingAssetsPath, "items.json");

        if (!File.Exists(persistentPath))
        {
            Debug.Log("No existe");
            File.Copy(streamingPath, persistentPath);
        }
        Debug.Log("Copiado JSON y cargando");
        ItemStatsLoader.CargarYAplicarStatsDesdeJson();
        Debug.Log("cargado");
// Ahora puedes usar persistentPath para lectura/escritura

        /*Debug.Log($"Item ID: {item.id}");
        Debug.Log($"Nombre: {item.name}");
        Debug.Log($"Descripción: {item.description}");
        Debug.Log($"Subtipo: {item.subtype}");
        
        Debug.Log($"Item ID: {item2.id}");
        Debug.Log($"Nombre: {item2.name}");
        Debug.Log($"Descripción: {item2.description}");
        Debug.Log($"Subtipo: {item2.subtype}");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
