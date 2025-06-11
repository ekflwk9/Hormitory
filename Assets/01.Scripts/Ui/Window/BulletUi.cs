using UnityEngine;

public class BulletUi : UiBase
{
    private int bulletCount = -1;
    private GameObject[] bulletUi;

    public override void Init()
    {
        var length = this.transform.childCount;
        bulletUi = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            bulletUi[i] = this.transform.GetChild(i).gameObject;
            bulletUi[i].SetActive(false);
        }

        UiManager.Instance.Add<BulletUi>(this);
    }

    /// <summary>
    /// 총알 + 1일 경우 true / - 1일 경우 false
    /// </summary>
    /// <param name="_isUp"></param>
    public void BulletView(bool _isUp)
    {
        bulletCount = _isUp ? +1 : -1;

        if (bulletCount < 0)
        {
            bulletCount = 0;
            Service.Log($"{bulletCount}가 0 이하일 수는 없음");
        }

        bulletUi[bulletCount].SetActive(_isUp);
    }
}
