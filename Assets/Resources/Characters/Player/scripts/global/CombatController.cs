using System;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public StatsComponent stats;
    public HealthComponent health;

    public int patata = 1;
    
    private void Awake()
    {
        stats = GetComponent<StatsComponent>();
        health = GetComponent<HealthComponent>();
        
        //stats.Initialize(10,1,2,4,5,5,4);
    }

    private void OnEnable()
    {
        health.Initialize(stats);
    }

    public void ReceiveDamage(int amount, string type)
    {
        health.TakeDamage(amount, type);
    }

    public void ReceiveHeal(int amount)
    {
        health.Heal(amount);
    }

    public int getStr()
    {
        return stats.str;
    }
    
    public int getSpd()
    {
        return stats.spd;
    }
}
