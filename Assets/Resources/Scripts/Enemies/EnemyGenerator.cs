using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Enemigos")]
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 6;
    private int actualMaxEnemies;
    public int minEnemies = 3;
    private int[] currentEnemies = new int[4];

    [Header("Spawning")]
    public GameObject sala;
    public GameObject[] spawnPoints;
    private List<EnemyController> enemies;

    private float timer = 0f;
    
    private GameObject player;

    private void Awake()
    {
        
    }

    private void Start()
    {
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            currentEnemies[i] = -1;
        }
        actualMaxEnemies = Random.Range(minEnemies, maxEnemies+1);
        int enemiesAssing = 0;
        while (enemiesAssing < actualMaxEnemies)
        {
            int randIndex = Random.Range(0, 3);
            currentEnemies[randIndex] += 1;
            enemiesAssing++;
        }

        enemies = new List<EnemyController>();
        
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            while (currentEnemies[i] >= 0)
                SpawnEnemy(i);
        }
    }

    void SpawnEnemy(int index)
    {
        if (enemies.Count >= actualMaxEnemies)
            return;
        var spawnPoint = spawnPoints[index].transform.position;
        spawnPoint.x = Random.Range(spawnPoint.x-0.5f, spawnPoint.x+0.5f);
        spawnPoint.y = Random.Range(spawnPoint.y-0.5f, spawnPoint.y+0.5f);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length-1);
        
        GameObject newEnemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint, Quaternion.identity, sala.transform);
        var enemyController = newEnemy.GetComponent<EnemyController>();
        enemies.Add(enemyController);
        currentEnemies[index] -= 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null)
                player = other.gameObject;
            foreach (EnemyController enemy in enemies)
            {
                if (!enemy.PlayerExists())
                    enemy.Initialize(player);
                enemy.StartEnemies();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnemyController enemy in enemies)
            {
                enemy.StopEnemies();
            }
        }
    }

    public void AssignPlayer(GameObject player)
    {
        this.player = player;
    }

    public GameObject GetPlayer()
    {
        
        return this.player;
    }
}
