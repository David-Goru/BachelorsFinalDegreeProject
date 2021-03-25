using System.Collections;
using UnityEngine;

public class LethalOrbProjectileBehaviour : MonoBehaviour, IProjectileWithStart
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Damage of the projectile when detonation occurs")] private int projectileDamage = 0;
    [SerializeField] [Tooltip("Damage of the projectile when detonation occurs")] private float projectileRadius = 0.0f;
    [SerializeField] [Tooltip("Force of the projectile when pushing the collided entities")] private float pushForce = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private Material detonatedMaterial;

    public void StartProjectile()
    {
        StartCoroutine(startProjectile());
    }

    private IEnumerator startProjectile()
    {
        yield return new WaitForSeconds(0.25f);
        model.SetActive(true);
    }

    public void Stop()
    {
        Destroy(gameObject);
    }

    public void Detonate()
    {
        StartCoroutine(startDetonation());
    }

    private IEnumerator startDetonation()
    {
        explosionParticles.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        model.GetComponent<MeshRenderer>().material = detonatedMaterial;
        dealDamage();

        yield return new WaitForSeconds(0.15f);

        endDetonation();
    }

    private void dealDamage()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, projectileRadius))
        {
            if (col.gameObject.CompareTag("Player")) continue;
            IEntity e = col.gameObject.GetComponent<IEntity>();
            if (e != null)
            {
                col.GetComponent<Rigidbody>().AddForce((col.transform.position - transform.position).normalized * pushForce);
                e.ReceiveDamage(projectileDamage);
            }
        }
    }

    private void endDetonation()
    {
        Destroy(gameObject);
    }
}