using System.Collections.Generic;
using UnityEngine;

public class HitUi : UiBase
{
    private int hitCount;
    [SerializeField] private Animator[] volume;
    [SerializeField] private Animator[] effect;

    public override void Init()
    {
        var childCount = this.transform.childCount;
        var tempVolume = new List<Animator>();
        var tempEffect = new List<Animator>();

        for (int i = 0; i < childCount; i++)
        {
            var child = this.transform.GetChild(i);
            var component = child.GetComponent<Animator>();

            if (child.name.Contains("Blood")) tempEffect.Add(component);
            else tempVolume.Add(component);
        }

        volume = tempVolume.ToArray();
        effect = tempEffect.ToArray();

        UiManager.Instance.Add<HitUi>(this);
    }

    public void HitView()
    {
        if (hitCount == volume.Length) hitCount = volume.Length - 1;

        for (int i = 0; i < hitCount + 1; i++)
        {
            volume[i].Play(AnimName.Hit, 0, 0);
        }

        hitCount++;
        effect[Random.Range(0, effect.Length)].Play(AnimName.Show, 0, 0);
    }

    public void ResetView()
    {
        hitCount = 0;

        for (int i = 0; i < volume.Length; i++)
        {
            volume[i].Play(AnimName.Idle, 0, 0);
        }

        for(int i  = 0; i < effect.Length; i++)
        {
            effect[i].Play(AnimName.Idle, 0, 0);
        }
    }
}
