using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalOrbProjectileBehaviour : IProjectileBehaviour
{
    [Header("Current values")]
    [SerializeField] private float projectileDamage = 0.0f;

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
        yield return new WaitForSeconds(0.25f);
        endDetonation();
    }

    private void endDetonation()
    {
        // For each enemy in sphere range deal damage using projectileDamage
        Debug.Log("Dealing " + projectileDamage + " damage in area");

        Destroy(gameObject);
    }
}