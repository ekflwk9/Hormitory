using UnityEngine;

public interface IInteractable
{
    // ��ȣ�ۿ� ������ ��ü��� �ݵ�� �� �޼��带 �����ؾ� ��
    void Interact();

    void Interact(GameObject interactor)
    {
        Interact();
    }
}