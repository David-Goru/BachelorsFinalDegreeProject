using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalOrbProjectileBehaviour : IProjectileBehaviour
{
    [Header("Current values")]
    [SerializeField] private float projectileDamage = 0.0f;

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
        yield return new WaitForSeconds(0.5f);
        endDetonation();
    }

    private void endDetonation()
    {
        // For each enemy in sphere range deal damage using projectileDamage
        Debug.Log("Dealing " + projectileDamage + " damage in area");

        Destroy(gameObject);
    }
}