using System;
using UnityEngine;

public class CryptosComponent : MonoBehaviour
{
    private int cryptos;
    public CryptosCounter counter;

    private void Awake()
    {
        counter = GameObject.FindFirstObjectByType<CryptosCounter>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetCryptos();
    }

    public void AddCryptos(int amount)
    {
        cryptos += amount;
        counter.SetCcryptos(cryptos);
    }
    
    public void RemoveCryptos(int amount)
    {
        cryptos += amount;
        counter.SetCcryptos(cryptos);
    }

    public int GetCryptos()
    {
        return cryptos;
    }

    public void ResetCryptos()
    {
        cryptos = 0;
        counter.SetCcryptos(cryptos);
    }
}
