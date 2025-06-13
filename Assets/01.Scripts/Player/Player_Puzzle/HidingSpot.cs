// HidingSpot.cs
using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractable
{
    [Tooltip("�÷��̾ ���� ���� �̵��� ��ġ")]
    [SerializeField] private Transform hidingTransform;

    [Tooltip("���� ������ ���� �� �÷��̾ �� ���� ��ġ")]
    [SerializeField] private Transform exitTransform;

    // ���� �� ��Ҹ� ����ϰ� �ִ��� ���� (bool ��� GameObject ���)
    private GameObject occupant = null;

    // IInteractable�� �䱸������ Interact() �Լ�
    public void Interact()
    {
        // �� ������Ʈ�� ��ȣ�ۿ� ��� ������ �ݵ�� �ʿ��ϹǷ�, �� �Լ��� ����Ӵϴ�.
    }

    // IInteractable�� �⺻ ������ ����� Interact(interactor) �Լ�
    public void Interact(GameObject interactor)
    {
        // 1. �ƹ��� ����ϰ� ���� �ʴٸ�, ���� ����� ����ϵ��� ��
        if (occupant == null)
        {
            // �� ��Ҹ� ����ϰ� �ִ� ������� interactor�� ����
            occupant = interactor;

            PuzzlePlayerController player = interactor.GetComponent<PuzzlePlayerController>();
            if (player != null)
            {
                player.ToggleHiding(this, hidingTransform, exitTransform);
            }
        }
        // 2. ���� ������ ��� ���ε�, �� ����� �ٷ� �ٽ� ���� �� ���(interactor)�̶��
        else if (occupant == interactor)
        {
            // ����� ��ģ ������ �����ϰ� occupant�� �����
            occupant = null;

            PuzzlePlayerController player = interactor.GetComponent<PuzzlePlayerController>();
            if (player != null)
            {
                player.ToggleHiding(this, hidingTransform, exitTransform);
            }
        }
        // 3. �ٸ� ����� �̹� ��� ���̶�� �ƹ��͵� ���� ����
    }
}