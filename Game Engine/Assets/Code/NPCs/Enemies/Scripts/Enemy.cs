using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour, IEntity
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Partciles that will be spawned when receiving a hit. Null for no particles.")] private GameObject onHitParticles = null;

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
    
    private Action currentBehaviour = null;

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

    public void UpdateState()
    {
        if (currentBehaviour != null) currentBehaviour();
    }

    public void UpdateTarget(GameObject target)
    {
        if (!agent.isOnNavMesh || currentTarget != null || Physics.CheckSphere(transform.position, 1.25f, 1 << LayerMask.NameToLayer("EnemyLimit"))) return;

        if (target.transform.parent.GetComponent<MainCharacter>() != null) target.transform.parent.GetComponent<MainCharacter>().StartFighting();

        currentTarget = target;
        chase();
    }

    public void ReceiveDamage(int damageAmount, bool finalBattle = false)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0) StartCoroutine(kill());
        else if (onHitParticles) Instantiate(onHitParticles, transform);
    }

    public bool IsOnFight()
    {
        return currentTarget != null;
    }

    private void idle()
    {
        resetAllTriggers();
        animator.SetTrigger("Idle");
        animator.SetFloat("IdleTime", 0);
        agent.isStopped = true;
        currentBehaviour = updateIdle;
        nextBehaviour = UnityEngine.Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
    }

    private void updateIdle()
    {
        nextBehaviour -= Time.deltaTime;
        if (nextBehaviour <= 0.0f) selector(getRandomPoint, walk, idle)();
        else animator.SetFloat("IdleTime", animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    private void walk()
    {
        resetAllTriggers();
        animator.SetTrigger("Walk");
        agent.isStopped = false;
        agent.speed = enemyInfo.WalkingSpeed;
        currentBehaviour = updateWalk;
    }

    private void updateWalk()
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
        currentBehaviour = updateChase;
        
        nextBehaviour = 60 / enemyInfo.AttackRate;
    }

    private void updateChase()
    {
        Vector3 ctpos = currentTarget.transform.position;
        agent.SetDestination(ctpos);
        transform.LookAt(currentTarget.transform.position);

        if (Vector3.Distance(transform.position, ctpos) < enemyInfo.AttackRange) attack();
        else if (Physics.CheckSphere(transform.position, 1.25f, 1 << LayerMask.NameToLayer("EnemyLimit")))
        {
            if (currentTarget.transform.parent.GetComponent<MainCharacter>() != null) currentTarget.transform.parent.GetComponent<MainCharacter>().StopFighting();
            currentTarget = null;
            walk();
        }
        else if (Vector3.Distance(transform.position, ctpos) > enemyInfo.ForgetRange)
        {
            if (currentTarget.transform.parent.GetComponent<MainCharacter>() != null) currentTarget.transform.parent.GetComponent<MainCharacter>().StopFighting();
            currentTarget = null;
            idle();
        }
    }

    private void attack()
    {
        resetAllTriggers();
        animator.SetFloat("IdleTime", 0);
        animator.SetTrigger("Attack");
        agent.isStopped = true;
        currentBehaviour = updateAttack;

        if (nextAttack <= 0.0f)
        {
            nextAttack = 60 / enemyInfo.AttackRate;

            if (enemyInfo.Projectile != null) Instantiate(enemyInfo.Projectile, projectileSpawner.position, Quaternion.identity).GetComponent<EnemyProjectile>().StartProjectile(enemyInfo.Damage, currentTarget.transform);
            else
            {
                IEntity e = currentTarget.transform.parent.GetComponent<IEntity>();
                if (e != null) e.ReceiveDamage(enemyInfo.Damage);
            }
        }
    }

    private void updateAttack()
    {
        nextAttack -= Time.deltaTime;
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > (enemyInfo.AttackRange + 0.25f)) chase();
        else
        {
            transform.LookAt(currentTarget.transform.position); 
            if (nextAttack <= 0) attack();
        }
    }

    private bool getRandomPoint()
    {
        nextPoint = new Vector3(transform.position.x + UnityEngine.Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + UnityEngine.Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));

        int checks = 0;
        while (checks < 100 && Physics.CheckSphere(nextPoint, 0.5f, spawner.CheckLayers) || Physics.Linecast(transform.position, nextPoint, spawner.CheckLayers))
        {
            nextPoint = new Vector3(transform.position.x + UnityEngine.Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + UnityEngine.Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));
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
        if (PlayerAndEnemiesPlaytesting.Instance != null) PlayerAndEnemiesPlaytesting.Instance.UpdateStat(enemyInfo.name, 1);

        if (currentTarget != null && currentTarget.transform.parent.GetComponent<MainCharacter>() != null) currentTarget.transform.parent.GetComponent<MainCharacter>().StopFighting();
        spawner.RemoveCache(this);
        enemyInfo.AmountKilled++;
        Destroy(gameObject);
    }

    private void spawnLoot()
    {
        float spawnProb = 0.8f;
        foreach (ItemPool lootPool in enemyInfo.LootPools)
        {
            foreach (Item loot in lootPool.Items)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) < spawnProb)
                {
                    Instantiate(loot.ItemModel, transform.position + Vector3.up, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0)).GetComponent<ItemOnWorld>().Spawn(loot);
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

    private Action selector(Func<bool> condition, Action ifTrue, Action ifFalse) { return condition() ? ifTrue : ifFalse; }
}