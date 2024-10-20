using UnityEngine;
using Unity.Netcode;
using Unity.Cinemachine;


public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Player Camera Settings")]
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Player Animation Settings")]
    [SerializeField] private float animationSmoothTime = 0.1f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputManager inputManager;
    private Transform cameraTransform;
    private AudioListener audioListener;

    private Animator animator;

    int moveXAnimationParameterId;
    int moveYAnimationParameterId;

    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    

    private void Start() 
    {
        if (!IsOwner) return;
        
        controller = gameObject.GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;

        if (IsLocalPlayer)
        {
            cinemachineCamera.Priority = 10;
        } 
        else
        {
            cinemachineCamera.Priority = 0;
        }

        //Animations initialization
        animator = GetComponent<Animator>();

        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveYAnimationParameterId = Animator.StringToHash("MoveY");
    }

    private void Update()
    {
        if (!IsOwner) return;

        Move();
    }

    private void Move()
    {
        groundedPlayer = controller.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Ensure the player stays grounded
        }

        Vector2 input = inputManager.GetPlayerMovement();

        // Calculate the camera forward and right directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Project camera directions onto the horizontal plane (y = 0)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        
        // Calculate the movement direction based on camera directions
        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;

        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveYAnimationParameterId, currentAnimationBlendVector.y);

        moveDirection.Normalize();

        float currentSpeed = inputManager.GetSprint() ? sprintSpeed : playerSpeed;

        // Move the player
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
