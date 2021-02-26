using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Entity
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Health the main character has when spawning")] private int maxHealth = 0;

    [Header("References")]
    [SerializeField] private Animator animator = null;
    [SerializeField] private MainCharacterNoise noise = null;
    [SerializeField] private MainCharacterMovement movement = null;
    [SerializeField] private MainCharacterAnimations animations = null;

    [Header("Debug")]
    [SerializeField] private int currentHealth =  0;
    [SerializeField] private MainCharacterState currentState;

    // Getters
    public Animator Animator { get => animator; }
    public MainCharacterNoise Noise { get => noise; }
    public MainCharacterMovement Movement { get => movement; }
    public MainCharacterAnimations Animations { get => animations; }

    public MainCharacterState CurrentState { get => currentState; set => currentState = value; }

    private void Start()
    {
        currentHealth = maxHealth;
        currentState = MainCharacterState.IDLE;
    }

    public override void ReceiveDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) StartCoroutine(kill());
        else
        {
            // do something? update ui? hit effect? sound?
        }
    }

    private IEnumerator kill()
    {
        movement.enabled = false;
        animations.SetAnimation(MainCharacterState.DIE);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));

        // End screen?
    }
}

public enum MainCharacterState
{
    IDLE,
    WALK,
    RUN,
    CROUCH,
    WALKCROUCH,
    DIE,
    USINGSPELLS
}