using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool onNormalIdle = false;
    private float idleTime = 0f;

    void Update()
    {
        if (!IsRunningSpells() && onNormalIdle)
        {
            idleTime += Time.deltaTime;
            animator.SetFloat("IdleTime", idleTime);
        }
        else idleTime = 0;
    }

    public void SetAnimation(MainCharacterAnimation animation)
    {
        switch (animation)
        {
            case MainCharacterAnimation.IDLE:
                animator.SetBool("Idle", true);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", false);
                onNormalIdle = true;
                break;
            case MainCharacterAnimation.WALKING:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", false);
                onNormalIdle = false;
                break;
            case MainCharacterAnimation.RUNNING:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", true);
                animator.SetBool("Crouching", false);
                onNormalIdle = false;
                break;
            case MainCharacterAnimation.CROUCH:
                animator.SetBool("Idle", true);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", true);
                onNormalIdle = false;
                break;
            case MainCharacterAnimation.WALKINGCROUCHED:
                animator.SetBool("Idle", false);
                animator.SetFloat("IdleTime", 0);
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
                animator.SetBool("Crouching", true);
                onNormalIdle = false;
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
    LONGIDLE,
    WALKING,
    RUNNING,
    CROUCH,
    WALKINGCROUCHED
}