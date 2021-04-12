using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter = null;
    [SerializeField] private Animator animator = null;

    private void Start()
    {
        try
        {
            mainCharacter = transform.GetComponent<MainCharacter>();
            animator = transform.Find("Model").GetComponent<Animator>();
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterAnimations references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    private void Update()
    {
        if (mainCharacter.Movement.enabled && mainCharacter.CurrentState == MainCharacterState.IDLE) animator.SetFloat("IdleTime", animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    public void SpellAnimation(int spellAnimId, bool firstSpell = false)
    {
        if (spellAnimId == 0)
        {
            animator.SetInteger("SpellAnimID", 0);
            animator.SetTrigger("StopSpells");
            return;
        }

        if (firstSpell) animator.SetTrigger("StartSpells");
        animator.SetInteger("SpellAnimID", spellAnimId);
    }

    public void UpdateAnimation()
    {
        ResetAllTriggers();

        if (mainCharacter.CurrentState == MainCharacterState.USINGSPELLS) return;

        if (mainCharacter.CurrentState == MainCharacterState.IDLE) animator.SetFloat("IdleTime", 0);
        animator.SetTrigger(mainCharacter.CurrentState.ToString());
    }

    public bool SpellsFinished()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Idle 1");
    }

    public bool DieAnimFinished()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }

    public void ResetAllTriggers()
    {
        foreach (MainCharacterState state in (MainCharacterState[])Enum.GetValues(typeof(MainCharacterState)))
        {
            if (state == MainCharacterState.USINGSPELLS) continue;
            animator.ResetTrigger(state.ToString());
        }
    }
}