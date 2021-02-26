using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter;

    private void Update()
    {
        if (mainCharacter.Movement.enabled && mainCharacter.CurrentState == MainCharacterState.IDLE) mainCharacter.Animator.SetFloat("IdleTime", mainCharacter.Animator.GetFloat("IdleTime") + Time.deltaTime);
    }

    public void SetAnimation(MainCharacterState state)
    {
        mainCharacter.CurrentState = state;

        if (state == MainCharacterState.IDLE) mainCharacter.Animator.SetFloat("IdleTime", 0);
        mainCharacter.Animator.SetTrigger(state.ToString());

        // Update noise radius
        mainCharacter.Noise.SetNoise(state);
    }
}