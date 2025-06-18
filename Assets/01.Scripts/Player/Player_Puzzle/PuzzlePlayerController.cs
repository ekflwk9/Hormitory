using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using _01.Scripts.Component;

public class PuzzlePlayerController : BasePlayerController
{
    #region Serialized Fields & Public Properties
    [Header("Puzzle Player Settings")]
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer;

    public bool IsHiding => isHiding;
    #endregion

    #region Private State Variables
    private bool isInputLocked = false;
    private bool isHiding = false;
    private IInteractable currentInteractable;
    private HidingSpot currentHidingSpot;
    #endregion

    #region Unity Lifecycle Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        playerActions.Player.Interact.performed += OnInteract;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerActions.Player.Interact.performed -= OnInteract;
    }

    protected override void Update()
    {
        if (isInputLocked) return;

        if (isHiding)
        {
            HandleLook();
            HandleInteraction();
            return;
        }

        base.Update();
        HandleInteraction();
    }
    #endregion

    #region Core Overridden & Specific Methods
    
   public override void Die()
    {
        base.Die(); 
    }


    public void TakeDamage()
    {
        if (isDead) return;
        Debug.Log("퍼즐 플레이어가 피격당했습니다!");
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerHit);
        }
    }
    #endregion

    #region Interaction & Hiding Logic


    private void HandleInteraction()
    {
        // mainCamera 변수는 부모로부터 물려받아 안전하게 사용
        if (mainCamera == null) return;

        currentInteractable = null;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                currentInteractable = interactable;
            }
        }
    }


    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isHiding && currentHidingSpot != null)
        {
            currentHidingSpot.Interact(this.gameObject);
        }
        else if (!isHiding && currentInteractable != null)
        {
            currentInteractable.Interact(this.gameObject);
        }
    }

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
        if (!isHiding)
        {
            currentHidingSpot = spot;
            StartCoroutine(HideCoroutine(hideTransform));
        }
        else
        {
            StartCoroutine(UnhideCoroutine(exitTransform));
        }
    }

    private IEnumerator HideCoroutine(Transform hideTransform)
    {
        LockInput();
        isHiding = true;
        characterController.enabled = false;

        Vector3 startPos = transform.position;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // 위치만 부드럽게 이동
            transform.position = Vector3.Lerp(startPos, hideTransform.position, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 설정
        transform.position = hideTransform.position;

        UnlockInput();
    }

    private IEnumerator UnhideCoroutine(Transform exitTransform)
    {
        LockInput();

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        // 목표 회전값을 '똑바로 선 자세'로 설정
        Quaternion targetRot = Quaternion.identity;

        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, exitTransform.position, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = exitTransform.position;
        transform.rotation = targetRot;

        // HandleLook이 사용하는 회전 변수들을 현재 상태에 맞게 초기화 (핵심)
        xRotation = 0f;
        yRotation = transform.eulerAngles.y;

        characterController.enabled = true;
        isHiding = false;
        currentHidingSpot = null;

        UnlockInput();
    }
    #endregion

    #region Physics Callbacks
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            // 이제 Die를 호출해도 mainCamera가 null일 걱정이 없습니다.
            Die();
        }
    }
    #endregion
}