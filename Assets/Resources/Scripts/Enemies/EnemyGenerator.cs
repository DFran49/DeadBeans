using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Enemigos")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    private int currentEnemies = 0;

    [Header("Spawning")]
    public float spawnInterval = 3f;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    private float timer = 0f;
    
    private GameObject player;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;

        var reporter = newEnemy.GetComponent<EnemyController>();
        reporter.Initialize(this);
    }

    public void NotifyEnemyDeath()
    {
        currentEnemies = Mathf.Max(currentEnemies - 1, 0);
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
