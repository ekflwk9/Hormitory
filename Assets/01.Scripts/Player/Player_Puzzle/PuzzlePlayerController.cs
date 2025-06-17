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

    private MainCamera mainCamera => CameraManager.Instance.MainCamera;

    /// <summary>
    /// 외부에서 플레이어가 숨어있는지 확인할 수 있는 프로퍼티입니다.
    /// </summary>
    public bool IsHiding => isHiding;

    #endregion

    #region Private State Variables

    private bool isInputLocked = false;
    private bool isHiding = false;
    private IInteractable currentInteractable;
    private HidingSpot currentHidingSpot;

    #endregion

    #region Unity Lifecycle Methods

    // 부모의 OnEnable 기능을 확장하여 상호작용 입력을 추가로 연결합니다.
    protected override void OnEnable()
    {
        base.OnEnable(); // 부모 클래스의 OnEnable()을 먼저 실행 (점프 입력 등)
        playerActions.Player.Interact.performed += OnInteract;
    }

    // 부모의 OnDisable 기능을 확장합니다.
    protected override void OnDisable()
    {
        base.OnDisable(); // 부모 클래스의 OnDisable()을 먼저 실행
        playerActions.Player.Interact.performed -= OnInteract;
    }

    
    protected override void Update()
    {
        

        if (isInputLocked) return;

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
            // 부모의 Update를 호출하여 이동, 시점 조작 등 공통 기능 실행
            base.Update();
            // 퍼즐 플레이어 고유의 상호작용 탐색 기능 실행
            HandleInteraction();
        }
    }

    #endregion

    #region Core Overridden & Specific Methods

    /// <summary>
    /// 부모 클래스의 abstract Die 메서드를 반드시 구현(override)해야 합니다.
    /// </summary>
    public override void Die()
    {
        
        if (isDead) return;

        isDead = true;
        playerActions.Player.Disable();

        // 카메라 사망 연출 호출
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerDeath);
        }
        // 사망 시 UI 표시
        
        mainCamera.enabled = false;
        UiManager.Instance.Show<HitUi>(true); 
        UiManager.Instance.Show<DeadUi>(true);
        Debug.Log("퍼즐 플레이어가 사망했습니다.");
    }

    /// <summary>
    /// PuzzlePlayerController의 고유한 피격 처리 기능입니다.
    /// </summary>
    public void TakeDamage()
    {
        if (isDead) return;

        Debug.Log("퍼즐 플레이어가 피격당했습니다!");

        // 카메라 피격 연출 호출
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Play(CameraShakeType.PlayerHit);
        }
    }

    #endregion

    #region Interaction & Hiding Logic

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

    #endregion

    #region Physics Callbacks

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
           //TakeDamage();
            Die(); // 사망 테스트 시 이 줄의 주석을 해제하세요.
        }
    }

    #endregion
}