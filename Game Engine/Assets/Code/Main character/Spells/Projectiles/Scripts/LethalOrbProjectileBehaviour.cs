using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalOrbProjectileBehaviour : IProjectileBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Damage of the projectile when detonation occurs")] private int projectileDamage = 0;
    [SerializeField] [Tooltip("Damage of the projectile when detonation occurs")] private float projectileRadius = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosionParticles;

    public override void StartProjectile()
    {
        StartCoroutine(startProjectile());
    }
    private IEnumerator startProjectile()
    {
        yield return new WaitForSeconds(0.25f);
        model.SetActive(true);
    }

    public override void Stop()
    {
        Destroy(gameObject);
    }

    public override void Detonate()
    {
        StartCoroutine(startDetonation());
    }

    private IEnumerator startDetonation()
    {
        explosionParticles.SetActive(true);
        dealDamage();
        yield return new WaitForSeconds(0.25f);
        endDetonation();
    }

    private void dealDamage()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, projectileRadius))
        {
            if (col.gameObject.CompareTag("Player")) continue;
            Entity e = col.gameObject.GetComponent<Entity>();
            if (e) e.ReceiveDamage(projectileDamage);
        }
    }

    private void endDetonation()
    {
        Destroy(gameObject);
    }
}