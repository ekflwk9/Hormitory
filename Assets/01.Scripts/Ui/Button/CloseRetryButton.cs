using UnityEngine.EventSystems;

public class CloseRetryButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);
        UiManager.Instance.Show<RetryUi>(false);
    }
}
