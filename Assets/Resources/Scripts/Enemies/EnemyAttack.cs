using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private CombatController combat;
        
    private Coroutine damageCoroutine;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name.Equals("Attack_Hitbox"))
        {
            return;
        }
        
        Transform parent = other.transform.parent;
        if (parent != null && parent.CompareTag("Player"))
        {
            CombatController cPlayer = parent.GetComponent<CombatController>();
            if (cPlayer != null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(cPlayer));
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Transform parent = other.transform.parent;
        if (parent != null && parent.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    
    private IEnumerator ApplyDamageOverTime(CombatController player)
    {
        while (true)
        {
            if (combat != null)
            {
                player.ReceiveDamage(combat.getStr(),"Physical");
                player.GetComponent<PlayerController>().OnHurt();
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    public void Initialize(CombatController combat)
    {
        
        this.combat = combat;
        //Debug.Log("Initializing enemy " + this.combat.getStr());
    }
}
