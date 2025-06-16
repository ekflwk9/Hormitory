using UnityEngine;
using UnityEngine.UI;

public class BulletUi : UiBase
{
    private int bulletCount;

    private GameObject[] bulletUi;
    private Animator anim;

    public override void Init()
    {
        var length = this.transform.childCount;
        bulletUi = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            bulletUi[i] = this.transform.GetChild(i).gameObject;
            if (bulletUi[i].TryGetComponent<Image>(out var component)) component.color = Color.clear;

            bulletCount++;
        }

        bulletCount--;

        anim = this.TryGetChildComponent<Animator>();
        UiManager.Instance.Add<BulletUi>(this);
    }

    public override void Show(bool _isActive)
    {
        anim.Play(_isActive ? AnimName.Show : AnimName.Hide, 0, 0);
    }

    /// <summary>
    /// 총알 + 1일 경우 true / - 1일 경우 false
    /// </summary>
    /// <param name="_isUp"></param>
    public void BulletView(bool _isUp)
    {
        if (!_isUp)
        {
            if (bulletCount < 0) bulletCount = 0;

            bulletUi[bulletCount].SetActive(_isUp);
            bulletCount--;
        }

        else
        {
            var uiCount = bulletUi.Length;

            for (int i = 0; i < uiCount; i++)
            {
                bulletUi[i].SetActive(true);
            }

            bulletCount = uiCount - 1;
        }
    }
}
