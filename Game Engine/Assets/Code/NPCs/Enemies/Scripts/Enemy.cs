using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Debug")]
    [SerializeField] private EnemyInfo enemyInfo;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemySpawn spawner;
    [SerializeField] private string currentBehaviour = "Idle";
    [SerializeField] private float nextBehaviour = 0.0f;
    [SerializeField] private Vector3 nextPoint;
    [SerializeField] private GameObject currentTarget = null;

    public void StartEnemy(EnemySpawn spawner, EnemyInfo enemyInfo)
    {
        animator = GetComponent<Animator>();
        this.spawner = spawner;
        this.enemyInfo = enemyInfo;

        // Initialize stats
        currentHealth = enemyInfo.MaxHealth;
        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
    }

    public void UpdateState(float timeDifference)
    {
        if (currentHealth <= 0) return;
        nextBehaviour -= timeDifference;

        if (currentTarget != null) transform.LookAt(currentTarget.transform.position);

        if (currentBehaviour == "Run")
        {
            Vector3 ctpos = currentTarget.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, ctpos, enemyInfo.RunningSpeed * timeDifference);

            if (Vector3.Distance(transform.position, ctpos) < enemyInfo.AttackRange)
            {
                currentBehaviour = "Attack";
                animator.SetTrigger("Idle");
            }
            else if (Vector3.Distance(transform.position, ctpos) > enemyInfo.ForgetRange)
            {
                currentTarget = null;
                setBehaviour("Idle");
            }
        }
        else if (currentBehaviour == "Attack" && Vector3.Distance(transform.position, currentTarget.transform.position) > enemyInfo.AttackRange) setBehaviour("Run");
        else if (nextBehaviour <= 0) changeBehaviour();
        else if (currentBehaviour == "Walk")
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, enemyInfo.WalkingSpeed * timeDifference);
            if (Vector3.Distance(transform.position, nextPoint) < 0.1f) changeBehaviour();
            else transform.LookAt(nextPoint);
        }
        else if (currentBehaviour == "Idle") animator.SetFloat("IdleTime", animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    public void UpdateTarget(GameObject target)
    {
        currentTarget = target; 
        setBehaviour("Run");
        nextBehaviour = 60 / enemyInfo.AttackRate;
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

    private void changeBehaviour()
    {
        if (currentBehaviour == "Attack")
        {
            attack();
            nextBehaviour = 60 / enemyInfo.AttackRate;
            return;
        }

        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
        if (currentBehaviour == "Idle")
        {
            nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));

            int checks = 0;
            while (checks < 100 && Physics.CheckSphere(nextPoint, 0.5f, spawner.CheckLayers) || Physics.Linecast(transform.position, nextPoint, spawner.CheckLayers))
            {
                nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));
                checks++;
            }
            if (checks == 100) setBehaviour("Idle");
            else setBehaviour("Walk");
        }
        else setBehaviour("Idle");
    }

    private void setBehaviour(string nextBehaviour)
    {
        currentBehaviour = nextBehaviour;
        animator.SetTrigger(nextBehaviour);
    }

    private void attack()
    {
        setBehaviour("Attack");
        Entity e = currentTarget.GetComponent<Entity>();
        if (e) e.ReceiveDamage(enemyInfo.Damage);
    }

    private IEnumerator kill()
    {
        animator.SetTrigger("Die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));

        // spawn loot

        spawner.RemoveCache(this);
        Destroy(gameObject);
    }
}