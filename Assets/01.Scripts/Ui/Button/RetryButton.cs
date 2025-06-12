using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RetryButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Show<MenuUi>(false);
        UiManager.Instance.Show<RetryUi>(false);

        UiManager.Instance.Get<FadeUi>().OnFade(NextScene);
    }

    private void NextScene()
    {
        var retryScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(retryScene);

        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
