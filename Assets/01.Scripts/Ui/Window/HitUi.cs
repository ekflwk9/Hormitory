using UnityEngine;

public class HitUi : UiBase
{
    private int hitCount;
    private Animator[] anim;

    public override void Init()
    {
        var childCount = this.transform.childCount;
        anim = new Animator[childCount];

        for (int i = 0; i < childCount; i++)
        {
            var child = this.transform.GetChild(i);
            anim[i] = child.GetComponent<Animator>();
        }

        UiManager.Instance.Add<HitUi>(this);
    }

    public void HitView()
    {
        if (hitCount == anim.Length) hitCount = anim.Length - 1;

        for (int i = 0; i < hitCount + 1; i++)
        {
            anim[i].Play(AnimName.Hit, 0, 0);
        }

        hitCount++;
    }

    public void ResetView()
    {
        hitCount = 0;
    }
}
