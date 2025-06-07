using System.Threading;
using TMPro;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class DataRequester : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressText;

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
            }
        );
    }
}

