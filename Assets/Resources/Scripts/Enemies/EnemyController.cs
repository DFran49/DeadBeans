using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyGenerator generator;
    private EnemyMovement  movement;
    private CombatController combat;
    private EnemyAttack attack;
    private GameObject player;
    
    private void Awake()
    {
        combat = GetComponent<CombatController>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponentInChildren<EnemyAttack>();
    }

    private void Start()
    {
        attack.Initialize(combat);
    }


    public void Initialize(GameObject player)
    {
        this.player = player;
        movement.SetPlayer(player);
    }

    public bool PlayerExists()
    {
        return player != null;
    }

    public void StopEnemies()
    {
        movement.SetSpeed(0);
    }

    public void StartEnemies()
    {
        movement.SetSpeed(combat.getSpd());
    }

    private void OnDestroy()
    {
        if (generator != null)
        {
            //FIXEAR DINERO O ELIMINAR CUANDO HAYA DROPS
            generator.GetPlayer().GetComponent<PlayerController>().ApplyMoney(combat.stats.value);
            //generator.NotifyEnemyDeath();
        }
    }
}
