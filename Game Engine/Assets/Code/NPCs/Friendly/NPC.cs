using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IEntity
{
    [Header("References")]
    [SerializeField] private NPCAnimations animations = null;

    [Header("Debug")]
    [SerializeField] private NPCState currentState;
    [SerializeField] private NPCState lastState;

    public NPCAnimations NpcAnimations { get => animations; }
    public NPCState CurrentState { get => currentState; }

    private void Awake()
    {
        try
        {
            animations = GetComponent<NPCAnimations>();
        }
        catch (UnityException e)
        {
            Debug.Log("NPC references not found. Disabling script. Error: " + e);
            enabled = false;
        }

        // Set base info
        currentState = NPCState.IDLE;
    }

    public void SetState(NPCState newState)
    {
        if (currentState != newState)
        {
            lastState = currentState;
            currentState = newState;
        }

        animations.UpdateAnimation();
    }

    public void ResumeState()
    {
        currentState = lastState;
        animations.UpdateAnimation();
    }

    public void ReceiveDamage(int damageAmount, bool finalBattle = false)
    {
        // Reduce HP?
    }
}

public enum NPCState
{
    IDLE,
    WALK,
    TALK,
    ATTACK
}