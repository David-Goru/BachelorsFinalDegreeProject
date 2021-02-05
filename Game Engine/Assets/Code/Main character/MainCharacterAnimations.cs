using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool onNormalIdle = false;
    float idleTime = 0f;

    void Update()
    {
        if (onNormalIdle)
        {
            idleTime += Time.deltaTime;
            animator.SetFloat("IdleTime", idleTime);
        }
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
                idleTime = 0f;
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