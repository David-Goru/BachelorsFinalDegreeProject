using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private EnemySpawn spawner;

    public void StartEnemy(EnemySpawn spawner)
    {
        this.spawner = spawner;
        StartCoroutine(destroyTest());
    }

    private IEnumerator destroyTest()
    {
        yield return new WaitForSeconds(5);
        spawner.RemoveCache(gameObject);
        Destroy(gameObject);
    }
}