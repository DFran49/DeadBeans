using System;
using System.Threading;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public StatsComponent stats;
    public HealthComponent health;
    
    private float lastHurt = 0f;

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
        Time.timeScale = 0;
        Thread.Sleep(30);
        Time.timeScale = 1;
    }

    public void EnemyReceiveDamage(int amount, Vector2 origen, string type)
    {
        GetComponent<Animator>().SetFloat("State", 1);
        ReceiveDamage(amount, type);
        Vector2 direccion = ((Vector2)transform.position - (Vector2)origen).normalized;
        GetComponent<EnemyMovement>().ApplyKnockback(direccion, 7f, 0.2f);
    }

    public void PlayerReceiveDamage(int amount, Vector2 origen, string type)
    {
        if ((Time.time - GetLastHurt()) > 1f)
        {
            GetComponent<PlayerController>().OnHurt();
            ReceiveDamage(amount, type);
            healthBar.SetHealth(health.GetHp());
            Vector2 direccion = ((Vector2)transform.position - (Vector2)origen).normalized;
            GetComponent<PlayerMovement>().ApplyKnockback(direccion, 7f, 0.2f);
            SetLastHurt(Time.time);
        }
    }

    public void SetLastHurt(float time)
    {
        lastHurt = time;
    }

    public float GetLastHurt()
    {
        return lastHurt;
    }

    public void ReceiveHeal(int amount)
    {
        health.Heal(amount);
        if (CompareTag("Player"))
            healthBar.SetHealth(health.GetHp());
    }

    public void SetHp(int hp)
    {
        health.SetHp(hp);
    }

    public int getStr()
    {
        return stats.str;
    }
    
    public float getSpd()
    {
        return stats.spd;
    }
}
