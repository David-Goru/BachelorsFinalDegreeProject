using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloralFlameProjectileBehaviour : IProjectileBehaviour
{
    [Header("Current values")]
    [SerializeField] private int projectileDamage = 0;
    [SerializeField] private float projectileRadius = 0.0f;

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private int currentState = 0;

    public override void Stop()
    {
        if (currentState == 1) StartCoroutine(unExtend());
        else Destroy(gameObject);
    }

    private IEnumerator unExtend()
    {
        animator.SetTrigger("Unextend");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Extend") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Unextend") == false);
        Destroy(gameObject);
    }

    public override void NextState()
    {
        currentState++;

        if (currentState == 1) animator.SetTrigger("Extend");
    }

    public override void Detonate()
    {
        StartCoroutine(startDetonation());
    }

    private IEnumerator startDetonation()
    {
        animator.SetTrigger("Raise");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("End"));
        endDetonation();
    }

    private void endDetonation()
    {
        foreach(Collider col in Physics.OverlapSphere(transform.position, projectileRadius, 1 << LayerMask.NameToLayer("Enemy")))
        {
            col.gameObject.GetComponent<Enemy>().GetDamage(projectileDamage);
        }

        Destroy(gameObject);
    }
}