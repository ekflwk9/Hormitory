using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RetryButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Show<MenuUi>(false);
        UiManager.Instance.Show<RetryUi>(false);

        //씬 전환 및 페이드 액션
        //SceneManager.LoadScene(StringMap.Battle);
    }
}
