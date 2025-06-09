using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    [SerializeField] private HitboxesData hitboxesData;

    public float attackRange = 1.5f;
    public int strength;
    public LayerMask enemyLayer;
    
    private float damageInterval = 1f; // Intervalo de daño (1 segundo)
    
    private Coroutine damageCoroutine;
    private HashSet<Collider2D> damagedEnemies = new HashSet<Collider2D>(); // Enemigos ya dañados
    
    public void SetHitboxFrame(int direction)
    {
        switch (direction)
        {
            case 0:
                Debug.Log("Abajo");
                transform.localPosition = new Vector3(0f, -1f, transform.position.z);
                break;
            case 1:
                Debug.Log("Izq");
                transform.localPosition = new Vector3(-0.8f, -0.2f, transform.position.z);
                break;
            case 2:
                Debug.Log("Arr");
                transform.localPosition = new Vector3(0f, 0.3f, transform.position.z);
                break;
            case 3:
                Debug.Log("Der");
                transform.localPosition = new Vector3(0.8f, -0.2f, transform.position.z);
                break;
        }
        
        // Iniciar el daño continuo
        StartDamageLoop();
    }
    
    private void StartDamageLoop()
    {
        // Detener cualquier corrutina anterior
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        
        // Limpiar la lista de enemigos dañados
        damagedEnemies.Clear();
        
        // Iniciar nueva corrutina de daño
        damageCoroutine = StartCoroutine(DamageLoop());
    }

    private IEnumerator DamageLoop()
    {
        while (true)
        {
            DealDamage();
            yield return new WaitForSeconds(damageInterval);
        }
    }
    
    private void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            // Solo dañar si no ha sido dañado antes
            if (!damagedEnemies.Contains(enemy))
            {
                var enemyCombat = enemy.GetComponent<CombatController>();
                if (enemyCombat != null)
                {
                    enemyCombat.EnemyReceiveDamage(strength, transform.position, "Physical");
                    damagedEnemies.Add(enemy); // Marcar como dañado
                }
            }
        }
    }
    
    public void DisableHitboxes()
    {
        // Detener la corrutina de daño
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
        
        // Limpiar la lista de enemigos dañados
        damagedEnemies.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetStats(int str)
    {
        strength = str;
    }
}
