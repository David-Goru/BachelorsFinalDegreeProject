using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [Tooltip("In seconds")] [SerializeField] private EnemyInfo enemyInfo;

    [Header("Debug")]
    [SerializeField] private int health = 0;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemySpawn spawner;
    [SerializeField] private string currentBehaviour = "Idle";
    [SerializeField] private float nextBehaviour = 0.0f;
    [SerializeField] private Vector3 nextPoint;

    public void StartEnemy(EnemySpawn spawner, EnemyInfo enemyInfo)
    {
        animator = GetComponent<Animator>();
        this.spawner = spawner;
        this.enemyInfo = enemyInfo;

        // Initialize stats
        health = enemyInfo.Health;
        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
    }

    public void UpdateState(float timeDifference)
    {
        nextBehaviour -= timeDifference;
        if (nextBehaviour <= 0) changeBehaviour();
        else
        {
            if (currentBehaviour == "Walking")
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPoint, enemyInfo.MovementSpeed * timeDifference);
                if (Vector3.Distance(transform.position, nextPoint) < 0.1f) changeBehaviour();
                else transform.LookAt(nextPoint);
            }
            else animator.SetFloat("IdleTime", animator.GetFloat("IdleTime") + Time.deltaTime);
        }
    }

    public void GetDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0) StartCoroutine(kill());
        else
        {
            // update do something? update ui? hit effect? sound?
        }
    }

    public void Attack(GameObject test)
    {
        Debug.Log("Attacking " + test.name);
    }

    private void changeBehaviour()
    {
        nextBehaviour = Random.Range(enemyInfo.MinTimeBetweenBehaviours, enemyInfo.MaxTimeBetweenBehaviours);
        animator.SetBool(currentBehaviour, false);
        if (currentBehaviour == "Idle")
        {
            nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));

            int checks = 0;
            while (checks < 100 && Physics.CheckSphere(nextPoint, 0.5f, spawner.CheckLayers) || Physics.Linecast(transform.position, nextPoint, spawner.CheckLayers))
            {
                nextPoint = new Vector3(transform.position.x + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea), transform.position.y, transform.position.z + Random.Range(-enemyInfo.WanderingArea, enemyInfo.WanderingArea));
                checks++;
            }
            if (checks == 100) currentBehaviour = "Idle";
            else currentBehaviour = "Walking";
        }
        else currentBehaviour = "Idle";
        animator.SetBool(currentBehaviour, true);
    }

    private IEnumerator kill()
    {
        // death animation?
        // spawn loot

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        spawner.RemoveCache(this);
        Destroy(gameObject); 
    }
}