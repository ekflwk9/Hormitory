using _01.Scripts.Component;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BasePlayerController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Look Settings")]
    [SerializeField] protected float mouseSensitivity = 100f;

    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 5.0f;
    [SerializeField] protected float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] protected float gravity = -20f;

    [Header("Player Audio Settings")]
    [SerializeField] protected AudioSource walkAudioSource;
    [SerializeField] protected AudioClip walkSoundClip;
    [SerializeField] protected GameObject UIManager;

    #endregion

    #region Protected Variables
    public bool isControl { get; protected set; } = true;
    protected CharacterController characterController;
    protected PuzzlePlayer playerActions;
    protected Camera mainCamera;
    protected Vector2 moveInput;
    protected float verticalVelocity;
    protected float xRotation = 0f;
    protected float yRotation = 0f;
    protected bool isGrounded;
    protected bool isDead = false;
    #endregion

    #region Unity Lifecycle Methods
    protected virtual void Awake()
    {
        Instantiate(UIManager);
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = GetComponentInChildren<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Player 프리팹의 자식 오브젝트에 Camera 컴포넌트가 없습니다!", this);
        }
        PlayerManager.Instance.SetPlayer(this);
    }

    protected virtual void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    protected virtual void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Jump.performed += OnJump;
        playerActions.Player.Menu.performed += OnMenu;
    }

    protected virtual void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= OnJump;
        playerActions.Player.Menu.performed -= OnMenu;

    }

    protected virtual void Update()
    {
        if (isDead || !isControl) return;
        if (mainCamera == null) return;
        isGrounded = characterController.isGrounded;
        HandleMovementAndGravity();
        HandleLook();
    }
    #endregion

    #region Core Logic Methods
    protected virtual void HandleLook()
    {
        Vector2 lookInput = playerActions.Player.Look.ReadValue<Vector2>();
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);


        if (yRotation > 360f) yRotation -= 360f;
        else if (yRotation < 0f) yRotation += 360f;
     

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    protected virtual void HandleMovementAndGravity()
    {
        if (isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x);
        bool isMoving = moveDirection.magnitude > 0.1f;

        if (isMoving && isGrounded)
        {
            if (walkAudioSource != null && !walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSoundClip;
                walkAudioSource.Play();
            }
        }
        else
        {
            if (walkAudioSource != null && walkAudioSource.isPlaying)
            {
                walkAudioSource.Stop();
            }
        }

        characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    protected virtual void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            SoundManager.PlaySfx(SoundCategory.Movement, "Jump");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        playerActions.Player.Disable();
        // UIManager 호출 방식으로 변경
        if (UiManager.Instance != null)
        {
            UiManager.Instance.Get<HitUi>().Show(true);
            UiManager.Instance.Get<DeadUi>().Show(true);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerDeath);
        }
        Debug.Log("플레이어가 사망했습니다.");
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        if (isControl)
        {
            SetPauseState(true);
            UiManager.Instance.Get<MenuUi>().Show(true);
        }
    }

    public void SetPauseState(bool isPaused)
    {
        isControl = !isPaused;        
        Cursor.lockState = isControl? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isPaused;

    }
    #endregion
}