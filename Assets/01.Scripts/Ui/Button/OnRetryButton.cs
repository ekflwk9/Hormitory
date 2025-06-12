using UnityEngine.EventSystems;

public class OnRetryButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Show<RetryUi>(true);
    }
}
