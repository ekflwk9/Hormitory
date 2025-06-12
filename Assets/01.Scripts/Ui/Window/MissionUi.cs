using TMPro;
using UnityEngine;

public class MissionUi : UiBase
{
    private Animator anim;
    private TMP_Text talk;

    public override void Init()
    {
        anim = this.TryGetComponent<Animator>();
        talk = this.TryGetChildComponent<TMP_Text>();

        UiManager.Instance.Add<MissionUi>(this);
    }

    public void Popup(string _text)
    {
        talk.text = _text;

        if (this.gameObject.activeSelf) anim.Play(AnimName.Idle, 0, 0);
        else this.gameObject.SetActive(true);
    }
}
