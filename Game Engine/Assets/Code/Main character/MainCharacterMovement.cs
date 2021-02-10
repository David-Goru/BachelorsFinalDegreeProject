using System;
using UnityEngine;
using Cinemachine;

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

    [Header("References")]
    [SerializeField] private CinemachineFreeLook characterCameraComponent;
    [SerializeField] private CapsuleCollider characterCollider = null;
    [SerializeField] private MainCharacterAnimations characterAnimations = null;
    [SerializeField] private GameObject characterModel = null;
    [SerializeField] private Transform viewPoint = null;

    [Header("Debug")]
    [SerializeField] private MainCharacterState currentState;

    private void Start()
    {
        speed = walkSpeed;
        setAnimation(MainCharacterState.IDLE);
    }

    private void FixedUpdate()
    {
        if (currentState == MainCharacterState.BLOCKED) return;

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        if (Input.GetButton("Crouch") && currentState != MainCharacterState.CROUCH && currentState != MainCharacterState.WALKINGCROUCHED) crouch();
        else if (!Input.GetButton("Crouch") && (currentState == MainCharacterState.CROUCH || currentState == MainCharacterState.WALKINGCROUCHED)) stopCrouching();

        if (xMovement == 0 && zMovement == 0)
        {
            if (currentState != MainCharacterState.IDLE && currentState != MainCharacterState.CROUCH) stopMoving();
            else if (currentState == MainCharacterState.WALKINGCROUCHED) setAnimation(MainCharacterState.CROUCH);
            return;
        }

        if (Input.GetButton("Run") && currentState != MainCharacterState.RUNNING && currentState != MainCharacterState.WALKINGCROUCHED && currentState != MainCharacterState.CROUCH) run();
        else if (!Input.GetButton("Run") && currentState == MainCharacterState.RUNNING) stopRunning();
        else if (currentState == MainCharacterState.IDLE) walk();
        else if (currentState == MainCharacterState.CROUCH) setAnimation(MainCharacterState.WALKINGCROUCHED);

        transform.eulerAngles = new Vector3(0, characterCameraComponent.m_XAxis.Value, 0);
        transform.Translate(new Vector3(xMovement, 0, zMovement) * speed * Time.deltaTime);
        viewPoint.position = transform.position;
        viewPoint.Translate(new Vector3(xMovement, 0, zMovement) * speed * 10 * Time.deltaTime);
        characterModel.transform.LookAt(viewPoint);
    }

    private void walk()
    {
        speed = walkSpeed;
        setAnimation(MainCharacterState.WALKING);
    }

    private void stopMoving()
    {
        setAnimation(currentState == MainCharacterState.WALKINGCROUCHED ? MainCharacterState.CROUCH : MainCharacterState.IDLE);
    }

    private void crouch()
    {
        speed = crouchSpeed;
        setAnimation(currentState == MainCharacterState.IDLE ? MainCharacterState.CROUCH : MainCharacterState.WALKINGCROUCHED);

        characterCollider.center = new Vector3(0, crouchingCenter, 0);
        characterCollider.height = crouchingHeight;
    }

    private void stopCrouching()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + defaultHeight + 0.05f, transform.position.z), characterCollider.radius)) return;

        speed = walkSpeed;
        setAnimation(currentState == MainCharacterState.CROUCH ? MainCharacterState.WALKING : MainCharacterState.IDLE);

        characterCollider.center = new Vector3(0, defaultCenter, 0);
        characterCollider.height = defaultHeight;
    }

    private void run()
    {
        speed = runSpeed;
        setAnimation(MainCharacterState.RUNNING);
    }

    private void stopRunning()
    {
        speed = walkSpeed;
        setAnimation(MainCharacterState.WALKING);
    }

    private void setAnimation(MainCharacterState newState)
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