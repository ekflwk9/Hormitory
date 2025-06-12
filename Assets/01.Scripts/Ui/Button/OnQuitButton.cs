using UnityEngine.EventSystems;

public class OnQuitButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.Instance.Get<FadeUi>().isFade)
        {
            touch.SetActive(false);
            UiManager.Instance.Show<QuitUi>(true);

            if (UiManager.Instance.introScene) UiManager.Instance.Show<IntroUi>(false);
        }
    }
}
