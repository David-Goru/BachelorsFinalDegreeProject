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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().UpdateTarget(gameObject);
        }
    }

    public void SetNoise(MainCharacterState state)
    {
        float newNoiseRadius = 0.0f;

        switch (state)
        {
            case MainCharacterState.IDLE:
            case MainCharacterState.CROUCH:
                newNoiseRadius = idleRadius;
                break;
            case MainCharacterState.WALK:
                newNoiseRadius = walkingRadius;
                break;
            case MainCharacterState.WALKCROUCH:
                newNoiseRadius = crouchingRadius;
                break;
            case MainCharacterState.RUN:
                newNoiseRadius = runningRadius;
                break;
            case MainCharacterState.USINGSPELLS:
                newNoiseRadius = spellsRadius;
                break;
        }

        noiseCollider.radius = newNoiseRadius;
    }
}