using UnityEngine;

public class CombatController : MonoBehaviour
{
    public StatsComponent stats;
    public HealthComponent health;
    
    private void Awake()
    {
        stats = GetComponent<StatsComponent>();
        health = GetComponent<HealthComponent>();
        
        health.Initialize(stats);
        //stats.Initialize(10,1,2,4,5,5,4);
    }
    
    public void ReceiveDamage(int amount)
    {
        health.TakeDamage(amount, "a");
    }

    public void ReceiveHeal(int amount)
    {
        health.Heal(amount);
    }

    public int getStr()
    {
        return stats.str;
    }
}
