using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private List<EnemyInfo> enemies;
    [SerializeField] private int maxEnemies = 0;
    [Tooltip("Enemies per minute")]
    [SerializeField] private float spawnRatio = 0.0f;
    [SerializeField] private float spawnRadius = 0.0f;
    [SerializeField] private LayerMask checkLayers;

    [Header("Debug")]
    [SerializeField] private float nextSpawn = 0.0f;
    [SerializeField] private List<GameObject> enemiesCache;

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
        if (enemiesCache.Count >= maxEnemies) return;
        if (nextSpawn > 0) nextSpawn -= Time.deltaTime;
        else spawnEnemy();
    }

    private void spawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));

        int checks = 0;
        while (checks < 100 && Physics.CheckSphere(spawnPosition, 0.5f, checkLayers))
        {
            spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
            checks++;
        }

        if (checks == 100)
        {
            Debug.Log("Spawn position not found");
            enabled = false;
        }
        else
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Count)].Model, spawnPosition, transform.rotation);
            enemiesCache.Add(enemy);
            enemy.GetComponent<EnemyBehaviour>().StartEnemy(this);
            nextSpawn = 60 / spawnRatio;
        }
    }

    public void RemoveCache(GameObject enemy)
    {
        enemiesCache.Remove(enemy);
    }
}