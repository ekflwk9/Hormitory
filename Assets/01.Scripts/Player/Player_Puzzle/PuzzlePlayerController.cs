using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PuzzlePlayerController : MonoBehaviour
{
    #region Serialized Fields & Public Properties

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -20f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer;

    public bool IsHiding => isHiding;

    #endregion

    #region Private State Variables

    private CharacterController characterController;
    private PuzzlePlayer playerActions;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool isGrounded;
    private bool isInputLocked = false;
    private bool isHiding = false;
    private IInteractable currentInteractable;
    private HidingSpot currentHidingSpot;

    #endregion

    #region Unity Lifecycle Methods

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

    // <<--- 수정된 Update 함수
    private void Update()
    {
        if (isInputLocked) return;

        // 숨어있을 때는 아무 동작도 하지 않음. 나오기 처리는 OnInteract에서 전담.
        if (isHiding)
        {
            return;
        }

        isGrounded = characterController.isGrounded;
        HandleMovementAndGravity();
        HandleLook();
        HandleInteraction();
    }

    #endregion

    #region Input Handling Methods

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    
    private void OnInteract(InputAction.CallbackContext context)
    {
        // 1. 숨어있는 상태가 최우선. 다른 조건 없이 바로 나오기를 시도.
        if (isHiding)
        {
            // 현재 숨어있는 장소(currentHidingSpot)와 다시 상호작용하여 나옴.
            if (currentHidingSpot != null)
            {
                currentHidingSpot.Interact(this.gameObject);
            }
        }
        // 2. 숨어있지 않고, 바라보는 대상이 있을 때만 일반 상호작용
        else if (currentInteractable != null)
        {
            currentInteractable.Interact(this.gameObject);
        }
    }

    #endregion

    #region Core Logic Methods

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
        if (isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleInteraction()
    {
        currentInteractable = null;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                currentInteractable = interactable;
            }
        }
    }

    #endregion

    #region State Control Methods

    public void LockInput()
    {
        isInputLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockInput()
    {
        isInputLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleHiding(HidingSpot spot, Transform hideTransform, Transform exitTransform)
    {
        // 숨으려고 할 때만 currentHidingSpot을 새로 지정
        if (!isHiding)
        {
            currentHidingSpot = spot;
            StartCoroutine(HideCoroutine(hideTransform));
        }
        else
        {
            // 나올 때는 이미 저장된 currentHidingSpot 정보를 사용
            StartCoroutine(UnhideCoroutine(exitTransform));
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator HideCoroutine(Transform hideTransform)
    {
        LockInput();
        isHiding = true;
       
        characterController.enabled = false;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, hideTransform.position, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRot, hideTransform.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = hideTransform.position;
        transform.rotation = hideTransform.rotation;

        Debug.Log("숨기 완료.");
        UnlockInput();
    }

    private IEnumerator UnhideCoroutine(Transform exitTransform)
    {
        LockInput();

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, exitTransform.position, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRot, exitTransform.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = exitTransform.position;
        transform.rotation = exitTransform.rotation;

        characterController.enabled = true;

        // isHiding 상태는 모든 동작이 끝난 후 변경
        isHiding = false;
       
        currentHidingSpot = null;

        Debug.Log("나오기 완료.");
        UnlockInput();
    }

    #endregion

    #region Physics Callbacks

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("몬스터와 충돌했습니다!");
        }
    }

    #endregion
}