using UnityEngine;

public class QuickWaterDropProjectileBehaviourNPC : MonoBehaviour, IProjectile
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Damage of the projectile when detonation occurs")] private int projectileDamage = 0;
    [SerializeField] [Tooltip("Duration of the projectile detonating")] private float detonationDuration = 0.0f;
    [SerializeField] [Tooltip("Initial thrust that the projectile has when detonating")] private float detonationThrust = 0.0f;

    [Header("Debug")]
    [SerializeField] private bool isDetonating = false;
    [SerializeField] private float timeDetonating = 0.0f;

    public void Stop()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!isDetonating) return;

        timeDetonating += Time.deltaTime;

        if (timeDetonating >= detonationDuration) endDetonation();
    }

    public void Detonate()
    {
        isDetonating = true;

        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(transform.forward * detonationThrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("PlayerItemGatherArea") || collision.gameObject.CompareTag("Item")) return;
        
        IEntity e = collision.gameObject.GetComponent<IEntity>();
        if (e != null) e.ReceiveDamage(projectileDamage);

        endDetonation();
    }

    private void endDetonation()
    {
        Destroy(gameObject);
    }
}