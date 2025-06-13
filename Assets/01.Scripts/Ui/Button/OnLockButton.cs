using UnityEngine.EventSystems;

public class OnLockButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);
        UiManager.Instance.Get<LockUi>().SetPassWord();
    }
}
