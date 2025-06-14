// PuzzlePlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// MonoBehaviour 대신 BasePlayerController를 상속받도록 변경
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

    // 부모의 OnEnable 기능을 확장하여 상호작용 입력을 추가로 연결
    protected override void OnEnable()
    {
        base.OnEnable(); // 부모 클래스의 OnEnable()을 먼저 실행 (점프 입력 연결)
        playerActions.Player.Interact.performed += OnInteract;
    }

    // 부모의 OnDisable 기능을 확장
    protected override void OnDisable()
    {
        base.OnDisable(); // 부모 클래스의 OnDisable()을 먼저 실행
        playerActions.Player.Interact.performed -= OnInteract;
    }

    // 부모의 Update 기능을 확장
    protected override void Update()
    {
        if (isInputLocked) return;

        // 숨어있을 때는 나오기 위한 상호작용만 확인
        if (isHiding)
        {
            HandleInteraction();
            return;
        }

        // 부모 클래스의 Update()를 실행하여 이동, 시점 조작, 중력 처리
        base.Update();

        // PuzzlePlayerController 고유의 기능인 상호작용 처리
        HandleInteraction();
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