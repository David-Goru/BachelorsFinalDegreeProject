using System;
using UnityEngine;
using Cinemachine;

public class MainCharacterMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Walking speed in m/s")] [Range(0, 20)] private float walkSpeed = 0f;
    [SerializeField] [Tooltip("Crouching speed in m/s")] [Range(0, 20)] private float crouchSpeed = 0f;
    [SerializeField] [Tooltip("Running speed in m/s")] [Range(0, 20)] private float runSpeed = 0f;
    [SerializeField] [Tooltip("Default collider center position")] private float defaultCenter = 0f;
    [SerializeField] [Tooltip("Collider center position when crouching")] private float crouchingCenter = 0f;
    [SerializeField] [Tooltip("Default collider height")] private float defaultHeight = 0f;
    [SerializeField] [Tooltip("Collider height when crouching")] private float crouchingHeight = 0f;
    [SerializeField] [Tooltip("Layers to check when crouching")] private LayerMask crouchLayer;

    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter = null;
    [SerializeField] private CinemachineFreeLook characterCameraComponent;
    [SerializeField] private BoxCollider characterCollider = null;
    [SerializeField] private GameObject characterModel = null;
    [SerializeField] private Transform viewPoint = null;

    [Header("Debug")]
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float xMovementLastFrame = 0.0f;
    [SerializeField] private float zMovementLastFrame = 0.0f;

    private void Start()
    {
        speed = walkSpeed;
    }

    private void Update()
    {
        GetCurrentState();
    }

    private void FixedUpdate()
    {
        if (xMovementLastFrame != 0 || zMovementLastFrame != 0) UpdatePosition();
    }

    public void GetCurrentState()
    {
        xMovementLastFrame = Input.GetAxis("Horizontal");
        zMovementLastFrame = Input.GetAxis("Vertical");

        if (Input.GetButton("Crouch")) crouch();
        else if (xMovementLastFrame != 0 || zMovementLastFrame != 0)
        {
            if (Input.GetButton("Run")) run();
            else walk();
        }
        else idle();
    }

    public void UpdatePosition()
    {
        transform.eulerAngles = new Vector3(0, characterCameraComponent.m_XAxis.Value, 0);
        transform.Translate(new Vector3(xMovementLastFrame, 0, zMovementLastFrame) * speed * Time.fixedDeltaTime);
        viewPoint.position = transform.position;
        viewPoint.Translate(new Vector3(xMovementLastFrame, 0, zMovementLastFrame) * speed * 10 * Time.fixedDeltaTime);
        characterModel.transform.LookAt(viewPoint);
    }

    private void idle()
    {
        changeState(MainCharacterState.IDLE);
    }

    private void walk()
    {
        speed = walkSpeed;
        changeState(MainCharacterState.WALK);
    }

    private void crouch()
    {
        if (xMovementLastFrame != 0 || zMovementLastFrame != 0)
        {
            speed = crouchSpeed;
            changeState(MainCharacterState.WALKCROUCH);
        }
        else changeState(MainCharacterState.CROUCH);

        characterCollider.center = new Vector3(0, crouchingCenter, 0);
        characterCollider.size = new Vector3(1, crouchingHeight, 1);
    }

    private bool stopCrouching()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, crouchingHeight + 1.0f, transform.position.z), 0.5f, crouchLayer)) return false;

        speed = walkSpeed;

        characterCollider.center = new Vector3(0, defaultCenter, 0);
        characterCollider.size = new Vector3(1, defaultHeight, 1);
        return true;
    }

    private void run()
    {
        speed = runSpeed;
        changeState(MainCharacterState.RUN);
    }

    private void changeState(MainCharacterState newState)
    {
        if (mainCharacter.CurrentState == newState) return;

        if (mainCharacter.CurrentState == MainCharacterState.CROUCH || mainCharacter.CurrentState == MainCharacterState.WALKCROUCH)
        {
            if (!stopCrouching() && newState != MainCharacterState.WALKCROUCH && newState != MainCharacterState.CROUCH)
            {
                crouch();
                return;
            }
        }

        mainCharacter.SetState(newState);
    }
}