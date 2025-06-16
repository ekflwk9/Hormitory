
using UnityEngine;
using UnityEngine.InputSystem;

// CharacterController 컴포넌트가 반드시 필요함을 명시
[RequireComponent(typeof(CharacterController))]

public abstract class BasePlayerController : MonoBehaviour
{
    #region Serialized Fields

    [Header("Look Settings")]
    [SerializeField] protected float mouseSensitivity = 100f;
    [SerializeField] protected Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 5.0f;
    [SerializeField] protected float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] protected float gravity = -20f;

    #endregion

    #region Protected Variables

    // protected: 자식 클래스에서 접근할 수 있도록 설정
    protected CharacterController characterController;
    protected PuzzlePlayer playerActions; // Input Actions 클래스

    protected Vector2 moveInput;
    protected float verticalVelocity;
    protected float xRotation = 0f;
    protected float yRotation = 0f;
    protected bool isGrounded;
    protected bool isDead = false; // <<-- 사망 상태 플래그 추가
    #endregion

    #region Unity Lifecycle Methods

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // <<-- deathUIPanel.SetActive(false) 로직 제거
    }

    protected virtual void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Jump.performed += OnJump;
    }

    protected virtual void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= OnJump;
    }

    protected virtual void Update()
    {
        // 사망 상태이면 모든 로직을 중단
        if (isDead) return;

        isGrounded = characterController.isGrounded;
        HandleMovementAndGravity();
        HandleLook();
    }

    #endregion

    #region Core Logic Methods (Moved from PuzzlePlayerController)

    protected virtual void HandleLook()
    {
        Vector2 lookInput = playerActions.Player.Look.ReadValue<Vector2>();
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    protected virtual void HandleMovementAndGravity()
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

    protected virtual void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
 
    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        // 모든 입력을 비활성화
       // playerActions.Player.Disable();
                
        Debug.Log("플레이어가 사망했습니다.");
    }

    #endregion
}