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
    public MainCharacterMovement Movement { get => movement; }

    // Getters and setters
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public MainCharacterState CurrentState { get => currentState; set => currentState = value; }

    private void Start()
    {
        // Get components
        try
        {
            animator = transform.Find("Main character").GetComponent<Animator>();
            noise = transform.Find("Noise area").GetComponent<MainCharacterNoise>();
            movement = gameObject.GetComponent<MainCharacterMovement>();
            animations = gameObject.GetComponent<MainCharacterAnimations>();
        }
        catch (UnityException e) 
        { 
            Debug.Log("MainCharacter references not found. Disabling script. Error: " + e);
            enabled = false;
        }

        // Set base stats
        currentHealth = maxHealth;
        currentState = MainCharacterState.IDLE;
    }

    public void SetState(MainCharacterState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animations.UpdateAnimation();

        // Update noise radius
        noise.SetNoise(newState);
    }

    public override void ReceiveDamage(int damageAmount)
    {
        if (PlayerAndEnemiesPlaytesting.Instance != null)
        {
            PlayerAndEnemiesPlaytesting.Instance.ChangeHealth(-damageAmount);
        }
        else
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0) StartCoroutine(kill());
            else
            {
                // do something? update ui? hit effect? sound?

                //if (onHitParticles) Instantiate(onHitParticles, transform);
            }
        }
    }

    public bool IsInFight()
    {
        return !movement.enabled;
    }

    private IEnumerator kill()
    {
        movement.enabled = false;
        SetState(MainCharacterState.DIE);
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