using UnityEngine.EventSystems;

public class CloseOptionButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);

        UiManager.Instance.Show<OptionUi>(false);
        UiManager.Instance.Show<MenuUi>(true);
    }
}
