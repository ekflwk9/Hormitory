using UnityEngine.EventSystems;

public class CloseOptionButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);
        UiManager.Instance.Show<OptionUi>(false);

        if (UiManager.Instance.introScene) UiManager.Instance.Show<IntroUi>(true);
        else UiManager.Instance.Show<MenuUi>(true);
    }
}
