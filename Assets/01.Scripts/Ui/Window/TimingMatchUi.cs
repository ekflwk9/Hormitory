using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMatchUi : UiBase
{
    public override void Init()
    {
        var timingMatch = this.TryGetComponent<TimingMatch>();
        timingMatch.Init();

        UiManager.Instance.Add<TimingMatchUi>(this);
    }
}