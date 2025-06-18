public class IntroUi : UiBase
{
    public override void Init()
    {
    }

    private void Start()
    {
        var uiBase = this.transform.GetComponentsInChildren<UiBase>(true);

        for (int i = 0; i < uiBase.Length; i++)
        {
            uiBase[i].Init();
        }

        SoundManager.PlayBgm("IntroBGM");
        UiManager.Instance.Add<IntroUi>(this);
    }

    public override void Show(bool _isActive)
    {
        base.Show(_isActive);
        UiManager.Instance.Get<FadeUi>().OnFade();
    }

    public void RemoveIntroData()
    {
        UiManager.Instance.Remove<IntroUi>(this);
    }
}
