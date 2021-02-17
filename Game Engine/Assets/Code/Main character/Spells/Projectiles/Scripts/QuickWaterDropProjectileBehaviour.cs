using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickWaterDropProjectileBehaviour : IProjectileBehaviour
{
    [Header("Current values")]
    [SerializeField] private float projectileDamage = 0.0f;
    [SerializeField] private float detonationDuration = 0.0f;
    [SerializeField] private float detonationThrust = 0.0f;

    [Header("Debug")]
    [SerializeField] private bool isDetonating = false;
    [SerializeField] private float timeDetonating = 0.0f;

    public override void Stop()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!isDetonating) return;

        timeDetonating += Time.deltaTime;

        if (timeDetonating >= detonationDuration) endDetonation();
    }

    public override void Detonate()
    {
        isDetonating = true;

        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(transform.forward * detonationThrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        Debug.Log("Quick water drop hit something.");

        // If enemy: deal damage using projectileDamage
        Debug.Log("Dealing " + projectileDamage + " damage to a target");

        endDetonation();
    }

    private void endDetonation()
    {
        Destroy(gameObject);
    }
}