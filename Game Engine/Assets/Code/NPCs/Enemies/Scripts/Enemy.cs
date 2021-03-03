using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("References")]
    [SerializeField] private Transform projectileSpawner = null;

    [Header("Debug")]
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private float nextBehaviour = 0.0f;
    [SerializeField] private float nextAttack = 0.0f;
    [SerializeField] private EnemyInfo enemyInfo;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemySpawn spawner;
    [SerializeField] private GameObject currentTarget = null;
    [SerializeField] private Vector3 nextPoint;
    [SerializeField] private NavMeshAgent agent;

    private System.Action<float> currentBehaviour = null;

    public void StartEnemy(EnemySpawn spawner, EnemyInfo enemyInfo)
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        this.spawner = spawner;
        this.enemyInfo = enemyInfo;

        // Initialize stats
        currentHealth = enemyInfo.MaxHealth;

        idle();
    }

    public void UpdateState(float timeDifference)
    {
        if (currentBehaviour != null) currentBehaviour(timeDifference);
    }

    private void idle()
    {
        resetAllTriggers();
        animator.SetTrigger("Idle");
        animator.SetFloat("IdleTime", 0);
        agent.isStopped = true;
        currentBehaviour = td => updateIdle(td);
        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
    }

    private void updateIdle(float timeDifference)
    {
        nextBehaviour -= timeDifference;
        if (nextBehaviour <= 0.0f)
        {
            if (getRandomPoint()) walk();
            else idle();
        }
        else animator.SetFloat("IdleTime", animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    private void walk()
    {
        resetAllTriggers();
        animator.SetTrigger("Walk");
        agent.isStopped = false;
        agent.speed = enemyInfo.WalkingSpeed;
        currentBehaviour = td => updateWalk(td);
    }

    private void updateWalk(float timeDifference)
    {
        agent.SetDestination(nextPoint);
        if (Vector3.Distance(transform.position, nextPoint) < 0.1f) idle();
        else transform.LookAt(nextPoint);
    }

    private void chase()
    {
        resetAllTriggers();
        animator.SetTrigger("Run");
        agent.isStopped = false;
        agent.speed = enemyInfo.RunningSpeed;
        currentBehaviour = td => updateChase(td);
        
        nextBehaviour = 60 / enemyInfo.AttackRate;
    }

    private void updateChase(float timeDifference)
    {
        Vector3 ctpos = currentTarget.transform.position;
        agent.SetDestination(ctpos);
        transform.LookAt(currentTarget.transform.position);

        if (Vector3.Distance(transform.position, ctpos) < enemyInfo.AttackRange) attack();
        else if (Vector3.Distance(transform.position, ctpos) > enemyInfo.ForgetRange) idle();
    }

    private void attack()
    {
        resetAllTriggers();
        animator.SetFloat("IdleTime", 0);
        animator.SetTrigger("Attack");
        agent.isStopped = true;
        currentBehaviour = td => updateAttack(td);

        if (nextAttack <= 0.0f)
        {
            nextAttack = 60 / enemyInfo.AttackRate;

            if (enemyInfo.Projectile != null) Instantiate(enemyInfo.Projectile, projectileSpawner.position, Quaternion.identity).GetComponent<EnemyProjectile>().StartProjectile(enemyInfo.Damage, currentTarget.transform);
            else
            {
                Entity e = currentTarget.GetComponent<Entity>();
                if (e) e.ReceiveDamage(enemyInfo.Damage);
            }
        }
    }

    private void updateAttack(float timeDifference)
    {
        nextAttack -= timeDifference;
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > (enemyInfo.AttackRange + 1)) chase();
        else
        {
            transform.LookAt(currentTarget.transform.position); 
            if (nextAttack <= 0) attack();
        }
    }

    public void UpdateTarget(GameObject target)
    {
        currentTarget = target;
        chase();        
    }

    public override void ReceiveDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) StartCoroutine(kill());
        else
        {
            // do something? update ui? hit effect? sound?
        }
    }

    private bool getRandomPoint()
    {
        nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));

        int checks = 0;
        while (checks < 100 && Physics.CheckSphere(nextPoint, 0.5f, spawner.CheckLayers) || Physics.Linecast(transform.position, nextPoint, spawner.CheckLayers))
        {
            nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));
            checks++;
        }

        if (checks == 100) return false;
        return true;
    }

    private IEnumerator kill()
    {
        resetAllTriggers();
        animator.SetTrigger("Die");
        agent.enabled = false;
        currentBehaviour = null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));

        spawnLoot();

        spawner.RemoveCache(this);
        Destroy(gameObject);
    }

    private void spawnLoot()
    {
        float spawnProb = 0.8f;
        foreach (ItemPool lootPool in enemyInfo.LootPools)
        {
            foreach (Item loot in lootPool.Items)
            {
                if (Random.Range(0.0f, 1.0f) < spawnProb)
                {
                    Instantiate(loot.ItemModel, transform.position + Vector3.right * Random.Range(-1.5f, 1.5f) + Vector3.forward * Random.Range(-1.5f, 1.5f), Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<ItemOnWorld>().Initialize(loot);
                    spawnProb = spawnProb * 2.0f / 3.0f; // Reduce a bit the prob for next items
                }
            }
        }
    }

    private void resetAllTriggers()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
    }
}