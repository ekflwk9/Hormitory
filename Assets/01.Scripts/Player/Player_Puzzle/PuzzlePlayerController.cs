// PuzzlePlayerController.cs

using UnityEngine;
using UnityEngine.InputSystem;

// UI 관련 변수를 모두 제거했으므로 using UnityEngine.UI; 는 필요 없습니다.

[RequireComponent(typeof(CharacterController))]
public class PuzzlePlayerController : MonoBehaviour
{
    // --- 체력 및 UI 관련 변수 모두 제거 ---

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer;
    private Camera mainCamera;
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
        mainCamera = Camera.main;

     
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
    }

    private void HandleMovementAndGravity()
    {
        isGrounded = characterController.isGrounded;
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 horizontalMovement = new Vector3(moveInput.x, 0, moveInput.y);

        if (isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement = (horizontalMovement * moveSpeed) + new Vector3(0, verticalVelocity, 0);
        characterController.Move(finalMovement * Time.deltaTime);
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
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayer))
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

    // --- TakeDamage, Die, ShowHitEffect 함수 모두 제거 ---

    // --- 충돌 감지 함수를 아래와 같이 단순화 ---
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 충돌한 오브젝트의 태그가 "Enemy"인지 확인
        if (hit.collider.CompareTag("Enemy"))
        {
            // 디버그 로그만 출력
            Debug.Log("몬스터와 충돌했습니다!");
        }
    }
}