using UnityEngine.EventSystems;

public class OptionButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);

        UiManager.Instance.Show<OptionUi>(true);
        UiManager.Instance.Show<MenuUi>(false);
    }
}
