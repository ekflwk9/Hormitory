using System;
using UnityEngine;

public class FadeUi : UiBase
{
    public bool isFade { get; private set; }

    private Animator anim;
    private Action func;

    public override void Init()
    {
        anim = this.TryGetComponent<Animator>();
        UiManager.Instance.Add<FadeUi>(this);
    }

    /// <summary>
    /// 페이드 인이 끝나면서 해당 메서드 호출
    /// </summary>
    /// <param name="_fadeFunc"></param>
    public void OnFade(Action _fadeFunc, float _speed = 1f)
    {
        anim.SetFloat(AnimName.Speed, _speed);
        anim.Play(AnimName.FadeIn, 0, 0);

        func = _fadeFunc;
        isFade = true;
    }

    /// <summary>
    /// 페이드 아웃 애니메이션 재생
    /// </summary>
    public void OnFade(float _speed = 1f)
    {
        anim.SetFloat(AnimName.Speed, _speed);
        anim.Play(AnimName.FadeOut, 0, 0);
        isFade = false;
    }

    private void EndFade()
    {
        //애니메이션 이벤트 호출 전용 메서드
        func?.Invoke();
        func = null;
    }
}
