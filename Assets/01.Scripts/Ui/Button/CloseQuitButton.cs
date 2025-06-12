using UnityEngine.EventSystems;

public class CloseQuitButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);

        UiManager.Instance.Get<QuitUi>().Show(false);
        if (UiManager.Instance.introScene) UiManager.Instance.Show<IntroUi>(true);
    }
}
