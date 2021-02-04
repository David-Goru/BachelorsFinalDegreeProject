using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{
    [Header("Current values")]
    [Range(0, 20)]
    [SerializeField] private float speed = 0f;

    [Header("Speeds")]
    [Range(0, 20)]
    [SerializeField] private float walkSpeed = 0f;
    [Range(0, 20)]
    [SerializeField] private float crouchSpeed = 0f;
    [Range(0, 20)]
    [SerializeField] private float runSpeed = 0f;

    [Header("Collider")]
    [SerializeField] private float defaultCenter = 0f;
    [SerializeField] private float crouchingCenter = 0f;
    [SerializeField] private float defaultHeight = 0f;
    [SerializeField] private float crouchingHeight = 0f;

    [Header("Debug")]
    [SerializeField] private MainCharacterState currentState;
    [SerializeField] private CapsuleCollider characterCollider = null;

    public MainCharacterState CurrentState { get => currentState; set => currentState = value; }

    void Start()
    {
        speed = walkSpeed;
        currentState = MainCharacterState.IDLE;
        characterCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if (currentState == MainCharacterState.BLOCKED) return;

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        if (Input.GetButton("Crouch") && currentState != MainCharacterState.CROUCHING && currentState != MainCharacterState.WALKINGCROUCHED) crouch();
        else if (!Input.GetButton("Crouch") && (currentState == MainCharacterState.CROUCHING || currentState == MainCharacterState.WALKINGCROUCHED)) stopCrouching();

        if (xMovement == 0 && zMovement == 0)
        {
            if (currentState != MainCharacterState.IDLE && currentState != MainCharacterState.CROUCHING) stopMoving();
            return;
        }        

        if (Input.GetButton("Run") && currentState != MainCharacterState.RUNNING && currentState != MainCharacterState.WALKINGCROUCHED && currentState != MainCharacterState.CROUCHING) run();
        else if (!Input.GetButton("Run") && currentState == MainCharacterState.RUNNING) stopRunning();
        else if (currentState == MainCharacterState.IDLE) setAnimation(MainCharacterState.WALKING);
        else if (currentState == MainCharacterState.CROUCHING) setAnimation(MainCharacterState.WALKINGCROUCHED);

        transform.Translate(new Vector3(xMovement, 0, zMovement) * speed * Time.deltaTime);
    }

    void stopMoving()
    {
        setAnimation(currentState == MainCharacterState.WALKINGCROUCHED ? MainCharacterState.CROUCHING : MainCharacterState.IDLE);
    }

    void crouch()
    {
        speed = crouchSpeed;
        setAnimation(currentState == MainCharacterState.IDLE ? MainCharacterState.CROUCHING : MainCharacterState.WALKINGCROUCHED);

        characterCollider.center = new Vector3(0, crouchingCenter, 0);
        characterCollider.height = crouchingHeight;
    }

    void stopCrouching()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + defaultHeight + 0.05f, transform.position.z), characterCollider.radius)) return;

        speed = walkSpeed;
        setAnimation(currentState == MainCharacterState.CROUCHING ? MainCharacterState.WALKING : MainCharacterState.IDLE);

        characterCollider.center = new Vector3(0, defaultCenter, 0);
        characterCollider.height = defaultHeight;
    }

    void run()
    {
        speed = runSpeed;
        setAnimation(MainCharacterState.RUNNING);
    }

    void stopRunning()
    {
        speed = walkSpeed;
        setAnimation(MainCharacterState.WALKING);
    }

    void setAnimation(MainCharacterState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case MainCharacterState.IDLE:
            case MainCharacterState.BLOCKED:
                // Idle animation
                break;
            case MainCharacterState.WALKING:
                // Walking animation
                break;
            case MainCharacterState.RUNNING:
                // Running animation
                break;
            case MainCharacterState.CROUCHING:
                // Crouching animation
                break;
            case MainCharacterState.WALKINGCROUCHED:
                // Walking crouched animation
                break;
            default:
                Debug.Log("Animation not implemented");
                break;
        }
    }
}

public enum MainCharacterState
{
    IDLE,
    WALKING,
    RUNNING,
    CROUCHING,
    WALKINGCROUCHED,
    BLOCKED
}