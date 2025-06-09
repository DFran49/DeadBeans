using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int curHP;
    private StatsComponent stats;
    
    public System.Action OnDeath;

    public void Initialize(StatsComponent stats)
    {
        this.stats = stats;
        curHP = stats.maxHp;
    }

    public void TakeDamage(int amount, string type)
    {
        int damage = 0;
        if (type.Equals("Magic"))
        {
            damage = Mathf.Max(amount - stats.mgcDef, 0);
        } else if (type.Equals("Physical"))
        {
            damage = Mathf.Max(amount - stats.def, 0);
        }
        
        curHP -= damage;
        if (curHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
            
        if (gameObject.tag.Equals("Player"))
            return;
        
        Destroy(gameObject);
    }
    
    public void Heal(int amount)
    {
        curHP = Mathf.Min(curHP + amount, stats.maxHp);
    }

    public int GetHp()
    {
        return curHP;
    }

    public void SetHp(int hp)
    {
        curHP = hp;
    }
}
