// BasePlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Player Audio Settings")]
    [SerializeField] protected AudioSource walkAudioSource;
    [SerializeField] protected AudioClip walkSoundClip;
    #endregion

    #region Protected Variables
    protected CharacterController characterController;
    protected PuzzlePlayer playerActions;
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
        // isDead 플래그는 부모가 계속 관리하여, 어떤 자식이든 죽으면 움직임이 멈추도록 보장
        if (isDead) return;

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
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x);

        bool isMoving = moveDirection.magnitude > 0.1f;

        if (isMoving && isGrounded)
        {
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSoundClip;
                walkAudioSource.Play();
            }
        }
        else
        {
            if (walkAudioSource.isPlaying)
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

    // <<--- TakeDamage() 메서드 제거

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        playerActions.Player.Disable();

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerDeath);
        }
        Debug.Log("플레이어가 사망했습니다.");
    }

    #endregion
}