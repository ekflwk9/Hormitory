using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.Instance.Get<FadeUi>().isFade)
        {
            UiManager.Instance.Show<IntroUi>(false);
            UiManager.Instance.Get<FadeUi>().OnFade(NextScene);
        }
    }

    private void NextScene()
    {
        SceneManager.LoadScene(StringMap.Puzzle);
        UiManager.Instance.IntroScene(false);

        UiManager.Instance.Get<IntroUi>().RemoveIntroData();
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
