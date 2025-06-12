using UnityEngine;

// 이 큐브는 상호작용이 가능하다고 명시 (IInteractable 인터페이스 구현)
public class InteractableCube : MonoBehaviour, IInteractable
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        // 색상을 바꾸기 위해 MeshRenderer 컴포넌트를 미리 가져옴
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // IInteractable 인터페이스의 Interact 메서드를 실제로 구현
    public void Interact()
    {
        // 상호작용 시 랜덤한 색상으로 변경
        meshRenderer.material.color = Random.ColorHSV();
        Debug.Log("큐브와 상호작용했습니다!");
    }
}