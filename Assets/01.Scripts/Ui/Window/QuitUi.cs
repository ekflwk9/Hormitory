public class QuitUi : UiBase
{
    public override void Init()
    {
        UiManager.Instance.Add<QuitUi>(this);
    }

    public override void Show(bool _isActive)
    {
        base.Show(_isActive);
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
