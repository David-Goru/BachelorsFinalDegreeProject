using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    [SerializeField] private MainCharacterAnimations characterAnimations = null;

    void Start()
    {
        speed = walkSpeed;
        currentState = MainCharacterState.IDLE;
        characterCollider = GetComponent<CapsuleCollider>();
        characterAnimations = GetComponent<MainCharacterAnimations>();
    }

    void FixedUpdate()
    {
        if (currentState == MainCharacterState.BLOCKED) return;

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        if (Input.GetButton("Crouch") && currentState != MainCharacterState.CROUCH && currentState != MainCharacterState.WALKINGCROUCHED) crouch();
        else if (!Input.GetButton("Crouch") && (currentState == MainCharacterState.CROUCH || currentState == MainCharacterState.WALKINGCROUCHED)) stopCrouching();

        if (xMovement == 0 && zMovement == 0)
        {
            if (currentState != MainCharacterState.IDLE && currentState != MainCharacterState.CROUCH) stopMoving();
            return;
        }

        if (Input.GetButton("Run") && currentState != MainCharacterState.RUNNING && currentState != MainCharacterState.WALKINGCROUCHED && currentState != MainCharacterState.CROUCH) run();
        else if (!Input.GetButton("Run") && currentState == MainCharacterState.RUNNING) stopRunning();
        else if (currentState == MainCharacterState.IDLE) walk();
        else if (currentState == MainCharacterState.CROUCH) setAnimation(MainCharacterState.WALKINGCROUCHED);

        transform.Translate(new Vector3(xMovement, 0, zMovement) * speed * Time.deltaTime);
    }

    void walk()
    {
        speed = walkSpeed;
        setAnimation(MainCharacterState.WALKING);
    }

    void stopMoving()
    {
        setAnimation(currentState == MainCharacterState.WALKINGCROUCHED ? MainCharacterState.CROUCH : MainCharacterState.IDLE);
    }

    void crouch()
    {
        speed = crouchSpeed;
        setAnimation(currentState == MainCharacterState.IDLE ? MainCharacterState.CROUCH : MainCharacterState.WALKINGCROUCHED);

        characterCollider.center = new Vector3(0, crouchingCenter, 0);
        characterCollider.height = crouchingHeight;
    }

    void stopCrouching()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + defaultHeight + 0.05f, transform.position.z), characterCollider.radius)) return;

        speed = walkSpeed;
        setAnimation(currentState == MainCharacterState.CROUCH ? MainCharacterState.WALKING : MainCharacterState.IDLE);

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
        characterAnimations.SetAnimation(Enum.TryParse(newState.ToString(), out MainCharacterAnimation anim) ? anim : MainCharacterAnimation.IDLE);
    }
}

public enum MainCharacterState
{
    IDLE,
    WALKING,
    RUNNING,
    CROUCH,
    WALKINGCROUCHED,
    BLOCKED
}