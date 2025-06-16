using UnityEngine;
using UnityEngine.EventSystems;

public class SetLockButton : UiButton
{
    [SerializeField] private int index;
    [SerializeField] private int value;

    public void SetButtonIndex(int thisIndex)
    {
        if (this.name.Contains("Up")) value = 1;
        else value = -1;

        index = thisIndex;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Get<LockUi>().SetNumber(index, value);
    }
}
