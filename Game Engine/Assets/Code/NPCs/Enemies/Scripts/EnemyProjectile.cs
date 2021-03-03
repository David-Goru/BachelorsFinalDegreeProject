using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float projectileDuration = 0.0f;
    [SerializeField] private float projectileSpeed = 0.0f;

    [Header("Debug")]
    [SerializeField] private int projectileDamage = 0;
    [SerializeField] private float timeAlive = 0.0f;
    [SerializeField] private Transform projectileTarget = null;

    public void StartProjectile(int damage, Transform target)
    {
        projectileDamage = damage;
        projectileTarget = target;

        this.enabled = true;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, projectileTarget.position + Vector3.up, Time.deltaTime * projectileSpeed);
        if (timeAlive >= projectileDuration) endDetonation();

        if (Vector3.Distance(transform.position, projectileTarget.position + Vector3.up) < 0.5f)
        {
            Entity e = projectileTarget.GetComponent<Entity>();
            if (e) e.ReceiveDamage(projectileDamage);
            endDetonation();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Building")) endDetonation();
    }

    private void endDetonation()
    {
        Destroy(gameObject);
    }
}