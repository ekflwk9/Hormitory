using UnityEngine;
using UnityEngine.EventSystems;

public class SetLockButton : UiButton
{
    [SerializeField] private int index;
    [SerializeField] private int value;

    public void SetButtonIndex(int thisIndex, int _setValue)
    {
        index = thisIndex;
        value = _setValue == 1 ? 1 : -1;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Get<LockUi>().SetNumber(index, value);
    }
}
