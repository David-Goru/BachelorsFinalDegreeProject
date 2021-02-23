using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private List<EnemyInfo> enemies;
    [SerializeField] private int maxEnemies = 0;
    [Tooltip("Enemies per minute")] [SerializeField] private float spawnRatio = 0.0f;
    [SerializeField] private float spawnRadius = 0.0f;
    [SerializeField] private LayerMask checkLayers;

    [Header("Debug")]
    [SerializeField] private float nextSpawn = 0.0f;
    [SerializeField] private List<Enemy> enemiesCache;

    public LayerMask CheckLayers { get => checkLayers; set => checkLayers = value; }

    private void Start()
    {
        if (enemies.Count == 0 || maxEnemies == 0 || spawnRatio == 0 || spawnRadius == 0)
        {
            Debug.Log("Attributes not set. Disabling spawn");
            enabled = false;
        }
        else nextSpawn = Random.Range(0, 60 / spawnRatio);
    }

    private void Update()
    {
        foreach (Enemy enemy in enemiesCache)
        {
            enemy.UpdateState(Time.deltaTime);
        }

        if (enemiesCache.Count >= maxEnemies) return;
        if (nextSpawn > 0) nextSpawn -= Time.deltaTime;
        else spawnEnemy();
    }

    private void spawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));

        int checks = 0;
        while (checks < 100 && Physics.CheckSphere(spawnPosition, 1f, checkLayers))
        {
            spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
            checks++;
        }

        if (checks == 100)
        {
            Debug.Log("Spawn position not found");
            nextSpawn = 60.0f;
        }
        else
        {
            EnemyInfo enemyInfo = enemies[Random.Range(0, enemies.Count)];
            Enemy enemy = Instantiate(enemyInfo.Model, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<Enemy>();
            enemiesCache.Add(enemy);
            enemy.StartEnemy(this, enemyInfo);
            nextSpawn = 60 / spawnRatio;
        }
    }

    public void RemoveCache(Enemy enemy)
    {
        enemiesCache.Remove(enemy);
    }
}