using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterNoise : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Radius of noise when on idle (including crouch)")] private float idleRadius = 0.0f;
    [SerializeField] [Tooltip("Radius of noise when walking")] private float walkingRadius = 0.0f;
    [SerializeField] [Tooltip("Radius of noise when running")] private float runningRadius = 0.0f;
    [SerializeField] [Tooltip("Radius of noise when crouching")] private float crouchingRadius = 0.0f; 
    [SerializeField] [Tooltip("Radius of noise when using spells")] private float spellsRadius = 0.0f;

    [Header("References")]
    [SerializeField] private SphereCollider noiseCollider;

    [Header("Debug")]
    [SerializeField] private float currentNoiseRadius = 0.0f;

    private void Start()
    {
        try
        {
            noiseCollider = gameObject.GetComponent<SphereCollider>();
            SetNoise(MainCharacterState.IDLE);
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterNoise references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().UpdateTarget(gameObject);
        }
    }

    public void SetNoise(MainCharacterState state)
    {
        currentNoiseRadius = 0.0f;

        switch (state)
        {
            case MainCharacterState.IDLE:
            case MainCharacterState.CROUCH:
                currentNoiseRadius = idleRadius;
                break;
            case MainCharacterState.WALK:
                currentNoiseRadius = walkingRadius;
                break;
            case MainCharacterState.WALKCROUCH:
                currentNoiseRadius = crouchingRadius;
                break;
            case MainCharacterState.RUN:
                currentNoiseRadius = runningRadius;
                break;
            case MainCharacterState.USINGSPELLS:
                currentNoiseRadius = spellsRadius;
                break;
        }

        noiseCollider.radius = currentNoiseRadius;
    }
}