using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Net.NetworkInformation;

public class PuzzlePlayerController : BasePlayerController
{
    #region Serialized Fields & Public Properties

    [Header("Interaction Settings")]
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

    // <<--- 수정된 Update 함수
    protected override void Update()
    {
        if (isInputLocked) return;

        // 땅에 닿았는지 여부는 항상 체크
        isGrounded = characterController.isGrounded;

        // 숨어있는 상태일 때의 로직
        if (isHiding)
        {
            // 시점 조작(엿보기)은 허용
            HandleLook();
            // 나오기 위한 상호작용 탐색도 허용
            HandleInteraction();
        }
        // 숨어있지 않은 일반 상태일 때의 로직
        else
        {
            // 부모 클래스의 기능을 직접 호출
            HandleMovementAndGravity();
            HandleLook();
            HandleInteraction();
        }
    }

    #endregion

    #region Puzzle-Specific Logic

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
        isHiding = false;
        currentHidingSpot = null;

        UnlockInput();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        LockInput();
        characterController.enabled = false;
        // 사망 UI 패널 활성화
        // deathUIPanel.SetActive(true); // deathUIPanel은 BasePlayerController에서 관리
        Debug.Log("플레이어가 사망했습니다.");
    }
    public void CameraShake()
    {
        // 카메라 흔들림 로직을 여기에 구현
        Debug.Log("카메라 흔들림 효과 발생");
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