using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidOneNotScared : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Animator animator;

    public override void Run()
    {
        animator.SetBool("QuestCompleted", true);
        animator.SetTrigger("IDLE");
    }
}