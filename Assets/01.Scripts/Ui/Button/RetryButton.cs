using _01.Scripts.Component;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RetryButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Show<MenuUi>(false);
        UiManager.Instance.Show<RetryUi>(false);

        UiManager.Instance.Get<FadeUi>().OnFade(NextScene, 0.5f);
    }

    private void NextScene()
    {
        var retryScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(retryScene);
        PlayerManager.Instance.Player.SetPauseState(false);

        UiManager.Instance.Show<LockUi>(false);
        UiManager.Instance.Show<TimingMatchUi>(false);
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
