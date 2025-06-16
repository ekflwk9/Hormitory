using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUi : UiBase
{
    public override void Init()
    {
        UiManager.Instance.Add<MenuUi>(this);
    }

    public override void Show(bool _isActive)
    {
        base.Show(_isActive);
        UiManager.Instance.Get<FadeUi>().OnFade();

        //플레이어 조작 설정
    }    
}
