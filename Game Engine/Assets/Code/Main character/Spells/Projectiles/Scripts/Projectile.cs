using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Spells/Projectile", order = 0)]
public class Projectile : ScriptableObject
{
    [Header("Current values")]
    [SerializeField] private float forwardOffset = 0.0f;
    [SerializeField] private float rightOffset = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject projectilePrefab = null;

    [Header("Debug")]
    [SerializeField] private IProjectileBehaviour currentProjectile = null;

    public void Spawn(Transform spawnPoint) 
    {
        if (currentProjectile != null) return;

        currentProjectile = MonoBehaviour.Instantiate<GameObject>(projectilePrefab, spawnPoint.position + spawnPoint.forward * forwardOffset + spawnPoint.right * rightOffset, spawnPoint.rotation).GetComponent<IProjectileBehaviour>();
    }

    public void Detonate()
    {
        if (currentProjectile == null)
        {
            Debug.Log("Projectile not spawned yet");
            return;
        }

        currentProjectile.Detonate();
        currentProjectile = null;
    }

    public void Stop()
    {
        if (currentProjectile == null) return;

        currentProjectile.Stop();
        currentProjectile = null;
    }
}