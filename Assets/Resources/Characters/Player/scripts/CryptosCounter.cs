using System;
using TMPro;
using UnityEngine;

public class CryptosCounter : MonoBehaviour
{
    private TextMeshProUGUI counter;

    private void Awake()
    {
        counter = GameObject.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    public void SetCcryptos(int amount)
    {
        counter.text = amount.ToString();
    }
}
