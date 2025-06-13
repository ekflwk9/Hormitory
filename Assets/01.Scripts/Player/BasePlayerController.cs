// BasePlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

// CharacterController 컴포넌트가 반드시 필요함을 명시
[RequireComponent(typeof(CharacterController))]
// abstract 키워드는 이 클래스가 직접 게임 오브젝트에 붙일 수 없는 '설계도'임을 의미
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

    #endregion

    #region Unity Lifecycle Methods

    // virtual: 자식 클래스에서 이 함수의 내용을 확장하거나 재정의(override)할 수 있도록 허용
    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected virtual void OnEnable()
    {
        playerActions.Player.Enable();
        // 점프와 상호작용 입력은 각 컨트롤러의 특성을 탈 수 있으므로,
        // 자식 클래스에서 필요에 따라 연결하도록 할 수 있습니다. 여기서는 점프만 공통으로 둡니다.
        playerActions.Player.Jump.performed += OnJump;
    }

    protected virtual void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= OnJump;
    }

    protected virtual void Update()
    {
        // 땅에 닿았는지 체크
        isGrounded = characterController.isGrounded;

        // 핵심 로직 호출
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

    #endregion
}