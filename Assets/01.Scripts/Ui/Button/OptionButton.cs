using UnityEngine.EventSystems;

public class OptionButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);

        if (!UiManager.Instance.Get<FadeUi>().isFade)
        {
            UiManager.Instance.Show<OptionUi>(true);
            UiManager.Instance.Show<MenuUi>(false);

            if (UiManager.Instance.introScene) UiManager.Instance.Show<IntroUi>(false);
        }
    }
}
