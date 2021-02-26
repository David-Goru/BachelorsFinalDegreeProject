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
    [SerializeField] [Tooltip("Collider center position when crouching")]  private float crouchingCenter = 0f;
    [SerializeField] [Tooltip("Default collider height")]  private float defaultHeight = 0f;
    [SerializeField] [Tooltip("Collider height when crouching")]  private float crouchingHeight = 0f;

    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter = null;
    [SerializeField] private CinemachineFreeLook characterCameraComponent;
    [SerializeField] private CapsuleCollider characterCollider = null;
    [SerializeField] private GameObject characterModel = null;
    [SerializeField] private Transform viewPoint = null;

    [Header("Debug")]
    [SerializeField] private float speed = 0f;

    private void Start()
    {
        speed = walkSpeed;
    }

    private void FixedUpdate()
    {
        CheckCurrentState();
    }

    public void CheckCurrentState()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        if (Input.GetButton("Crouch") && mainCharacter.CurrentState != MainCharacterState.CROUCH && mainCharacter.CurrentState != MainCharacterState.WALKCROUCH) crouch();
        else if (!Input.GetButton("Crouch") && (mainCharacter.CurrentState == MainCharacterState.CROUCH || mainCharacter.CurrentState == MainCharacterState.WALKCROUCH)) stopCrouching();

        if (xMovement == 0 && zMovement == 0)
        {
            if (mainCharacter.CurrentState != MainCharacterState.IDLE && mainCharacter.CurrentState != MainCharacterState.CROUCH) stopMoving();
            else if (mainCharacter.CurrentState == MainCharacterState.WALKCROUCH) mainCharacter.Animations.SetAnimation(MainCharacterState.CROUCH);
            return;
        }

        if (Input.GetButton("Run") && mainCharacter.CurrentState != MainCharacterState.RUN && mainCharacter.CurrentState != MainCharacterState.WALKCROUCH && mainCharacter.CurrentState != MainCharacterState.CROUCH) run();
        else if (!Input.GetButton("Run") && mainCharacter.CurrentState == MainCharacterState.RUN) stopRunning();
        else if (mainCharacter.CurrentState == MainCharacterState.IDLE || mainCharacter.CurrentState == MainCharacterState.USINGSPELLS) walk();
        else if (mainCharacter.CurrentState == MainCharacterState.CROUCH) mainCharacter.Animations.SetAnimation(MainCharacterState.WALKCROUCH);

        transform.eulerAngles = new Vector3(0, characterCameraComponent.m_XAxis.Value, 0);
        transform.Translate(new Vector3(xMovement, 0, zMovement) * speed * Time.deltaTime);
        viewPoint.position = transform.position;
        viewPoint.Translate(new Vector3(xMovement, 0, zMovement) * speed * 10 * Time.deltaTime);
        characterModel.transform.LookAt(viewPoint);
    }

    private void walk()
    {
        speed = walkSpeed;
        mainCharacter.Animations.SetAnimation(MainCharacterState.WALK);
    }

    private void stopMoving()
    {
        mainCharacter.Animations.SetAnimation(mainCharacter.CurrentState == MainCharacterState.WALKCROUCH ? MainCharacterState.CROUCH : MainCharacterState.IDLE);
    }

    private void crouch()
    {
        speed = crouchSpeed;
        mainCharacter.Animations.SetAnimation(mainCharacter.CurrentState == MainCharacterState.IDLE ? MainCharacterState.CROUCH : MainCharacterState.WALKCROUCH);

        characterCollider.center = new Vector3(0, crouchingCenter, 0);
        characterCollider.height = crouchingHeight;
    }

    private void stopCrouching()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, crouchingCenter + crouchingHeight / 2 + characterCollider.radius + 0.05f, transform.position.z), characterCollider.radius)) return;

        speed = walkSpeed;
        mainCharacter.Animations.SetAnimation(mainCharacter.CurrentState == MainCharacterState.CROUCH ? MainCharacterState.WALK : MainCharacterState.IDLE);

        characterCollider.center = new Vector3(0, defaultCenter, 0);
        characterCollider.height = defaultHeight;
    }

    private void run()
    {
        speed = runSpeed;
        mainCharacter.Animations.SetAnimation(MainCharacterState.RUN);
    }

    private void stopRunning()
    {
        speed = walkSpeed;
        mainCharacter.Animations.SetAnimation(MainCharacterState.WALK);
    }
}