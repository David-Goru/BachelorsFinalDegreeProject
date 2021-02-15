using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private MainCharacterAnimation currentAnimation;
    [SerializeField] private float idleTime = 0f;

    private void Start()
    {
        currentAnimation = MainCharacterAnimation.IDLE;
    }

    private void Update()
    {
        if (!IsRunningSpells() && currentAnimation == MainCharacterAnimation.IDLE)
        {
            idleTime += Time.deltaTime;
            animator.SetFloat("IdleTime", idleTime);
        }
        else idleTime = 0;
    }

    public void SetAnimation(MainCharacterAnimation animation)
    {
        currentAnimation = animation;
        switch (animation)
        {
            case MainCharacterAnimation.IDLE:
                animator.SetBool("Idle", true);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", false);
                break;
            case MainCharacterAnimation.WALKING:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", false);
                break;
            case MainCharacterAnimation.RUNNING:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", true);
                animator.SetBool("Crouching", false);
                break;
            case MainCharacterAnimation.CROUCH:
                animator.SetBool("Idle", true);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", true);
                break;
            case MainCharacterAnimation.WALKINGCROUCHED:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", true);
                break;
        }
    }

    public bool IsRunningSpells()
    {
        if (animator.GetInteger("SpellAnimID") != 0) return true;
        return false;
    }
}

public enum MainCharacterAnimation
{
    IDLE,
    WALKING,
    RUNNING,
    CROUCH,
    WALKINGCROUCHED
}