using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetTest : MonoBehaviour
{
    [SerializeField] private GameObject projectileTest = null;
    [SerializeField] private float timeBetweenProjectiles = 0.0f;
    [SerializeField] private float nextProjectile = 0.0f;

    private void Update()
    {
        nextProjectile -= Time.deltaTime;
        if (nextProjectile <= 0)
        {
            nextProjectile = timeBetweenProjectiles;
            IProjectile pb = Instantiate(projectileTest, transform).GetComponent<IProjectile>();
            //pb.NextState();
            pb.Detonate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().UpdateTarget(gameObject);
        }
    }
}