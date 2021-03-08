using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter = null;

    private void Start()
    {
        mainCharacter = gameObject.GetComponent<MainCharacter>();
    }

    private void Update()
    {
        if (mainCharacter.Movement.enabled && mainCharacter.CurrentState == MainCharacterState.IDLE) mainCharacter.Animator.SetFloat("IdleTime", mainCharacter.Animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    public void UpdateAnimation()
    {
        ResetAllTriggers();

        if (mainCharacter.CurrentState == MainCharacterState.USINGSPELLS) return;

        if (mainCharacter.CurrentState == MainCharacterState.IDLE) mainCharacter.Animator.SetFloat("IdleTime", 0);
        mainCharacter.Animator.SetTrigger(mainCharacter.CurrentState.ToString());
    }

    public void ResetAllTriggers()
    {
        foreach (MainCharacterState state in (MainCharacterState[])Enum.GetValues(typeof(MainCharacterState)))
        {
            if (state == MainCharacterState.USINGSPELLS) continue;
            mainCharacter.Animator.ResetTrigger(state.ToString());
        }
    }
}