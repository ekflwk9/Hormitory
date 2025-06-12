using UnityEngine.EventSystems;

public class OnQuitButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);
        UiManager.Instance.Show<QuitUi>(true);
    }
}
