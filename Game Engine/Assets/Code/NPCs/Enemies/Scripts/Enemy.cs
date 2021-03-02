using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("Debug")]
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private float nextBehaviour = 0.0f;
    [SerializeField] private EnemyInfo enemyInfo;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemySpawn spawner;
    [SerializeField] private GameObject currentTarget = null;
    [SerializeField] private Vector3 nextPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshObstacle obstacle;

    private System.Action<float> currentBehaviour = null;

    public void StartEnemy(EnemySpawn spawner, EnemyInfo enemyInfo)
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        this.spawner = spawner;
        this.enemyInfo = enemyInfo;

        // Initialize stats
        currentHealth = enemyInfo.MaxHealth;

        idle();
    }

    public void UpdateState(float timeDifference)
    {
        nextBehaviour -= timeDifference;
        if (currentBehaviour != null) currentBehaviour(timeDifference);
    }

    private void idle()
    {
        resetAllTriggers();
        animator.SetTrigger("Idle");
        animator.SetFloat("IdleTime", 0);
        agent.enabled = false;
        obstacle.enabled = true;
        currentBehaviour = td => updateIdle(td);
        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
    }

    private void updateIdle(float timeDifference)
    {
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
        obstacle.enabled = false;
        agent.enabled = true;
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
        obstacle.enabled = false;
        agent.enabled = true;
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
        agent.enabled = false;
        obstacle.enabled = true;
        currentBehaviour = td => updateAttack(td);
        Entity e = currentTarget.GetComponent<Entity>();
        if (e) e.ReceiveDamage(enemyInfo.Damage);
    }

    private void updateAttack(float timeDifference)
    {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > (enemyInfo.AttackRange + 1)) chase();
        else
        {
            transform.LookAt(currentTarget.transform.position); 
            if (nextBehaviour <= 0)
            {
                nextBehaviour = 60 / enemyInfo.AttackRate;
                attack();
            }
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
        obstacle.enabled = true;
        currentBehaviour = null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));

        // spawn loot

        spawner.RemoveCache(this);
        Destroy(gameObject);
    }

    private void resetAllTriggers()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
    }
}