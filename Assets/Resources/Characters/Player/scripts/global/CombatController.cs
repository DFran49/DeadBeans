using System;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public StatsComponent stats;
    public HealthComponent health;

    public HealthBar healthBar;
    
    private void Awake()
    {
        stats = GetComponent<StatsComponent>();
        health = GetComponent<HealthComponent>();
        
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
    }

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar.SetMaxHealth(stats.maxHp);
            healthBar.SetHealth(health.GetHp());
        }
    }

    private void OnEnable()
    {
        health.Initialize(stats);
    }

    public void ReceiveDamage(int amount, string type)
    {
        health.TakeDamage(amount, type);
    
        if (CompareTag("Player"))
            healthBar.SetHealth(health.GetHp());
    }

    public void ReceiveHeal(int amount)
    {
        health.Heal(amount);
    }

    public void SetHp(int hp)
    {
        health.SetHp(hp);
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
