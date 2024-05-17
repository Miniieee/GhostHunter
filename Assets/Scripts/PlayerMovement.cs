using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private InputManager inputManager;
    private Transform cameraTransform;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
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

        // Calculate the movement direction based on camera directions
        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection.Normalize();

        float currentSpeed = inputManager.GetSprint() ? sprintSpeed : playerSpeed;

        // Move the player
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
