public class RetryUi : UiBase
{
    public override void Init()
    {
        UiManager.Instance.Add<RetryUi>(this);
    }

    public override void Show(bool _isActive)
    {
        base.Show(_isActive);
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
