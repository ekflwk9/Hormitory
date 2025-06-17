using UnityEngine;
using UnityEngine.InputSystem;
using _01.Scripts.Component;

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
    #endregion

    #region Protected Variables
    protected CharacterController characterController;
    protected PuzzlePlayer playerActions;
    protected Camera mainCamera; // 실제 Camera 컴포넌트를 저장할 변수

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
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Awake에서 직접 자식 카메라를 찾아 할당합니다.
        // 이렇게 하면 다른 스크립트가 실행되기 전에 카메라 참조가 보장됩니다.
        mainCamera = GetComponentInChildren<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Player 프리팹의 자식 오브젝트에 Camera 컴포넌트가 없습니다! Inspector에서 확인해주세요.", this);
        }
    }
    protected virtual void Start()
    {
        //// CameraManager로부터 MainCamera 스크립트를 가져와서
        //// 실제 Camera 컴포넌트를 찾아 할당합니다.
        //if (CameraManager.Instance != null && CameraManager.Instance.MainCamera != null)
        //{
        //    mainCamera = CameraManager.Instance.MainCamera.GetComponent<Camera>();
        //}
        //else
        //{
        //    Debug.LogError("CameraManager 또는 CameraManager의 MainCamera가 할당되지 않았습니다!");
        //}
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
        if (isDead) return;
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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
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
        // SoundManager가 static 클래스이므로 Instance 없이 호출
        if (isGrounded)
        {
            SoundManager.PlaySfx(SoundCategory.Movement, "Jump");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Die 메서드는 자식 클래스에서 재정의(override) 할 수 있도록 virtual로 유지
    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        playerActions.Player.Disable();
        UiManager.Instance.Get<HitUi>().Show(true);
        UiManager.Instance.Get<DeadUi>().Show(true);

        // CameraShake는 싱글턴이므로 Instance로 호출
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerDeath);
        }
        Debug.Log("플레이어가 사망했습니다.");
    }
    #endregion
}