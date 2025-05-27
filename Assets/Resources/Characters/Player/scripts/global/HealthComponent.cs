using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int curHP;
    private StatsComponent stats;
    //public HealthBar healthBar;

    public void Initialize(StatsComponent stats)
    {
        this.stats = stats;
        curHP = stats.maxHp;
        
        /*healthBar.SetMaxHealth(stats.maxHp);
        healthBar.SetHealth(GetHp());*/
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
        //healthBar.SetHealth(GetHp());
        Debug.Log($"{gameObject.name} recibe {damage} daño. HP: {curHP}");

        if (curHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
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
}
