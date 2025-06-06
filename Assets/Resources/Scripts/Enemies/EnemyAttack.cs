using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private CombatController combat;
    
    private CombatController cPlayer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name.Equals("Attack_Hitbox"))
            return;
        
        Transform parent = other.transform.parent;
        if (parent != null && parent.CompareTag("Player"))
        {
            cPlayer = parent.GetComponent<CombatController>();
            if (cPlayer != null && (Time.time - cPlayer.GetLastHurt()) >= 0.5f)
                Hurt();
        }
    }

    void Hurt()
    {
        if (combat == null)
            return;
        
        cPlayer.GetComponent<PlayerController>().OnHurt();
        cPlayer.ReceiveDamage(combat.getStr(),"Physical");
        cPlayer.SetLastHurt(Time.time);
        Time.timeScale = 0;
        Thread.Sleep(50);
        Time.timeScale = 1;
    }

    public void Initialize(CombatController combat)
    {
        this.combat = combat;
    }
}
