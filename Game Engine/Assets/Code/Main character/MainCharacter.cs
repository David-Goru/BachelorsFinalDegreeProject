
using System.Collections;
using UnityEngine;

public class MainCharacter : MonoBehaviour, IEntity
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Health the main character has when spawning")] private int maxHealth = 0;

    [Header("References")]
    [SerializeField] private Transform model = null;
    [SerializeField] private MainCharacterNoise noise = null;
    [SerializeField] private MainCharacterMovement movement = null;
    [SerializeField] private MainCharacterAnimations animations = null;
    [SerializeField] private MainCharacterCamera characterCamera = null;
    [SerializeField] private MainCharacterGatherer gatherer = null;

    [Header("Debug")]
    [SerializeField] private int currentHealth =  0;
    [SerializeField] private MainCharacterState currentState;
    [SerializeField] private int enemiesFighting = 0;

    // Getters
    public Transform Model { get => model; }
    public MainCharacterMovement Movement { get => movement; }
    public MainCharacterAnimations Animations { get => animations; }
    public bool IsFighting { get => enemiesFighting > 0; }

    // Getters and setters
    public int CurrentHealth { get => currentHealth; }
    public MainCharacterState CurrentState { get => currentState; }

    private void Start()
    {
        // Get components
        try
        {
            model = transform.Find("Model");
            noise = transform.Find("Noise area").GetComponent<MainCharacterNoise>();
            movement = transform.GetComponent<MainCharacterMovement>();
            animations = transform.GetComponent<MainCharacterAnimations>();
            characterCamera = transform.GetComponent<MainCharacterCamera>();
            gatherer = transform.Find("Item gather area").GetComponent<MainCharacterGatherer>();
        }
        catch (UnityException e) 
        { 
            Debug.Log("MainCharacter references not found. Disabling script. Error: " + e);
            enabled = false;
        }

        if (maxHealth == 0) Debug.Log("Main character max health has not been defined.");

        // Set base info
        currentState = MainCharacterState.IDLE;

        // Set stats if new game
        if (!Menu.LoadingGame) currentHealth = maxHealth;
    }

    public void Load(int currentHealth)
    {
        this.currentHealth = currentHealth;
    }

    public void SetState(MainCharacterState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animations.UpdateAnimation();

        // Update noise radius
        noise.SetNoise(newState);
    }

    public void ReceiveDamage(int damageAmount)
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

    public void StartFighting()
    {
        if (enemiesFighting == 0)
        {
            Music.Instance.StartSong("battle");
        }

        enemiesFighting++;
    }

    public void StopFighting()
    {
        enemiesFighting--;

        if (enemiesFighting == 0)
        {
            Music.Instance.StartSong("default");
        }
    }

    private IEnumerator kill()
    {
        movement.enabled = false;
        SetState(MainCharacterState.DIE);
        yield return new WaitUntil(() => animations.DieAnimFinished());

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