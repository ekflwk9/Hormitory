using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PuzzlePlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Transform cameraTransform;
    private float xRotation = 0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -20f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer;

   

    private IInteractable currentInteractable;

    private CharacterController characterController;
    private PuzzlePlayer playerActions;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Jump.performed += OnJump;
        playerActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= OnJump;
        playerActions.Player.Interact.performed -= OnInteract;
    }

    private void Update()
    {
        HandleMovementAndGravity();
        HandleInteraction();
        HandleLook();
    }

    private void HandleLook()
    {
        Vector2 lookInput = playerActions.Player.Look.ReadValue<Vector2>();
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 50f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

   
    private void HandleMovementAndGravity()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        moveInput = playerActions.Player.Move.ReadValue<Vector2>();

        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;

        // 회전 로직은 제거하고 이동만 처리합니다.
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                currentInteractable = interactable;
            }
            else
            {
                currentInteractable = null;
            }
        }
        else
        {
            currentInteractable = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("몬스터와 충돌했습니다!");
        }
    }

     
}