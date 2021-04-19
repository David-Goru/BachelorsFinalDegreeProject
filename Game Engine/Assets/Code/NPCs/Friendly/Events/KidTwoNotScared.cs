using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidTwoNotScared : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Animator animator;

    public override void Run()
    {
        animator.SetBool("NoEnemies", true);
    }
}