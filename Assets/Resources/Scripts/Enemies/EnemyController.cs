using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyGenerator generator;
    private EnemyMovement  movement;
    private CombatController combat;
    private EnemyAttack attack;
    
    private void Awake()
    {
        combat = GetComponent<CombatController>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponentInChildren<EnemyAttack>();
    }

    private void Start()
    {
        attack.Initialize(combat);
        movement.SetSpeed(combat.getSpd());
    }


    public void Initialize(EnemyGenerator generator)
    {
        this.generator = generator;
        movement.SetPlayer(generator.GetPlayer());
    }

    private void OnDestroy()
    {
        if (generator != null)
        {
            generator.GetPlayer().GetComponent<PlayerController>().ApplyMoney(combat.stats.value);
            generator.NotifyEnemyDeath();
        }
    }
}
