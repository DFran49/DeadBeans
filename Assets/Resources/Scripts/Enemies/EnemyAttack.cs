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
            
            //Hacerlo a playerController
            if (cPlayer != null)
                cPlayer.PlayerReceiveDamage(combat.getStr(), transform.position,"Physical");
        }
    }

    public void Initialize(CombatController combat)
    {
        this.combat = combat;
    }
}
