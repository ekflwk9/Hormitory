using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUi : UiBase
{
    public override void Init()
    {
        UiManager.Instance.Add<DeadUi>(this);
    }
    private void EndAction()
    {
        //애니메이션 이벤트 호출 전용 메서드
        UiManager.Instance.Get<FadeUi>().OnFade(EndFade);
    }

    private void EndFade()
    {
        var thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);

        this.gameObject.SetActive(false);
        UiManager.Instance.Get<HitUi>().ResetView();
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
