
using System.Collections;
using UnityEngine;

public class MainCharacter : MonoBehaviour, IEntity
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Health the main character has when spawning")] private int maxHealth = 0;
    [SerializeField] [Tooltip("Player gold to be used on the merchant")] private int gold = 0;

    [Header("References")]
    [SerializeField] private Transform model = null;
    [SerializeField] private MainCharacterNoise noise = null;
    [SerializeField] private MainCharacterMovement movement = null;
    [SerializeField] private MainCharacterAnimations animations = null;
    [SerializeField] private MainCharacterCamera characterCamera = null;
    [SerializeField] private MainCharacterGatherer gatherer = null;
    [SerializeField] private MainCharacterMessages messages = null;

    [Header("Debug")]
    [SerializeField] private int currentHealth =  0;
    [SerializeField] private MainCharacterState currentState;
    [SerializeField] private int enemiesFighting = 0;

    // Getters
    public Transform Model { get => model; }
    public MainCharacterMovement Movement { get => movement; }
    public MainCharacterAnimations Animations { get => animations; }
    public MainCharacterCamera CharacterCamera { get => characterCamera; }
    public MainCharacterMessages Messages { get => messages; }
    public bool IsFighting { get => enemiesFighting > 0; }

    // Getters and setters
    public int Gold { get => gold; set => gold = value; }
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
            messages = transform.GetComponent<MainCharacterMessages>();
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

    public void Load(int currentHealth, int gold)
    {
        this.currentHealth = currentHealth;
        this.gold = gold;
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
            if (currentHealth <= 0)
            {
                if (currentState != MainCharacterState.DIE) StartCoroutine(kill());
            }
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

        messages.ShowMessage(MessageType.DEAD);

        yield return new WaitForSeconds(2.0f);

        // Reset info
        SetState(MainCharacterState.IDLE);
        currentHealth = maxHealth;
        transform.position = new Vector3(0, 0.651f, 0);
        movement.enabled = true;
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