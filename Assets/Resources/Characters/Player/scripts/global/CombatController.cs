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
        
        //rb = GetComponent<Rigidbody2D>();
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
    }

    public void EnemyReceiveDamage(int amount, Vector2 origen, string type)
    {
        ReceiveDamage(amount, type);
        Vector2 direccion = ((Vector2)transform.position - (Vector2)origen).normalized;
        GetComponent<EnemyMovement>().ApplyKnockback(direccion, 7f, 0.2f);
    }

    public void PlayerReceiveDamage(int amount, Vector2 origen, string type)
    {
        if ((Time.time - GetLastHurt()) > 1f)
        {
            ReceiveDamage(amount, type);
            healthBar.SetHealth(health.GetHp());
            Debug.Log("Daño con espera " + (Time.time - GetLastHurt()));
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
    
    public int getSpd()
    {
        return stats.spd;
    }

    
}
