// HidingSpot.cs
using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractable
{
    [Tooltip("플레이어가 숨기 위해 이동할 위치")]
    [SerializeField] private Transform hidingTransform;

    [Tooltip("숨는 동작이 끝난 후 플레이어가 서 있을 위치")]
    [SerializeField] private Transform exitTransform;

    // 누가 이 장소를 사용하고 있는지 저장 (bool 대신 GameObject 사용)
    private GameObject occupant = null;

    // IInteractable의 요구사항인 Interact() 함수
    public void Interact()
    {
        // 이 오브젝트는 상호작용 대상 정보가 반드시 필요하므로, 이 함수는 비워둡니다.
    }

    // IInteractable의 기본 구현을 덮어쓰는 Interact(interactor) 함수
    public void Interact(GameObject interactor)
    {
        // 1. 아무도 사용하고 있지 않다면, 들어온 사람이 사용하도록 함
        if (occupant == null)
        {
            // 이 장소를 사용하고 있는 대상으로 interactor를 지정
            occupant = interactor;

            PuzzlePlayerController player = interactor.GetComponent<PuzzlePlayerController>();
            if (player != null)
            {
                player.ToggleHiding(this, hidingTransform, exitTransform);
            }
        }
        // 2. 만약 누군가 사용 중인데, 그 사람이 바로 다시 말을 건 사람(interactor)이라면
        else if (occupant == interactor)
        {
            // 사용을 마친 것으로 간주하고 occupant를 비워줌
            occupant = null;

            PuzzlePlayerController player = interactor.GetComponent<PuzzlePlayerController>();
            if (player != null)
            {
                player.ToggleHiding(this, hidingTransform, exitTransform);
            }
        }
        // 3. 다른 사람이 이미 사용 중이라면 아무것도 하지 않음
    }
}