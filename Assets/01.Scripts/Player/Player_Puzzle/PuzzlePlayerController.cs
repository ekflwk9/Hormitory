using UnityEngine;
using UnityEngine.InputSystem;

// 이 스크립트가 붙은 오브젝트에는 반드시 CharacterController가 있도록 강제합니다.
[RequireComponent(typeof(CharacterController))]
public class PuzzlePlayerController : MonoBehaviour
{
    // 이동 및 점프 관련 변수들 (인스펙터에서 조절 가능)
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;

    // private 필드는 camelCase 규칙을 따릅니다.
    private CharacterController characterController;
    private PuzzlePlayer playerActions; // 클래스 이름을 PuzzlePlayer로 수정하신 것을 반영했습니다.
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isGrounded;

    // 스크립트가 처음 활성화될 때 한 번 호출됩니다.
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerActions = new PuzzlePlayer();
    }

    // 오브젝트가 활성화될 때마다 호출됩니다.
    private void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Jump.performed += OnJump;
    }

    // 오브젝트가 비활성화될 때마다 호출됩니다.
    private void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= OnJump;
    }

    // 매 프레임마다 호출됩니다.
    private void Update()
    {
        // 1. 땅에 닿았는지 확인합니다.
        isGrounded = characterController.isGrounded;

        // 2. 수평 이동 방향을 계산합니다.
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 horizontalMovement = new Vector3(moveInput.x, 0, moveInput.y);

        // 3. 수직 속도(중력)를 계산합니다.
        // 땅에 있고, 떨어지는 중이 아니면 수직 속도를 초기화합니다.
        if (isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -2.0f;
        }

        // 중력을 계속해서 수직 속도에 적용합니다.
        verticalVelocity += gravity * Time.deltaTime;

        // 4. 모든 이동을 합쳐서 최종 이동 벡터를 만듭니다.
        Vector3 finalMovement = (horizontalMovement * moveSpeed) + new Vector3(0, verticalVelocity, 0);

        // 5. 최종적으로 Move 함수를 '한 번만' 호출합니다.
        characterController.Move(finalMovement * Time.deltaTime);
    }

    // 점프 입력 처리를 담당하는 이벤트 핸들러 메서드입니다.
    private void OnJump(InputAction.CallbackContext context)
    {
        // 땅에 있을 때만 점프가 가능하도록 합니다.
        if (isGrounded)
        {
            // 물리 공식(v = sqrt(h * -2 * g))을 이용해 점프에 필요한 초기 수직 속도를 계산합니다.
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}