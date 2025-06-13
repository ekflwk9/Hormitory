using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PuzzlePlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Transform cameraTransform;
    private float xRotation = 0f;
    private float yRotation = 0f;

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
    private bool isInputLocked = false;

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

    // --- 추가: 입력을 잠그는 공개 함수 ---
    public void LockInput()
    {
        isInputLocked = true;
        Cursor.lockState = CursorLockMode.None; // 커서를 보이게 하고
        Cursor.visible = true;                  // 자유롭게 움직이도록 설정
    }

    // --- 추가: 입력을 해제하는 공개 함수 ---
    public void UnlockInput()
    {
        isInputLocked = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서를 다시 잠그고
        Cursor.visible = false;                   // 보이지 않게 설정
    }


    private void Update()
    {
        // --- 수정: 입력이 잠겨있으면 아래 로직을 실행하지 않음 ---
        if (isInputLocked)
        {
            return;
        }

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
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
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