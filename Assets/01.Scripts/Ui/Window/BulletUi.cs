using UnityEngine;

public class BulletUi : UiBase
{
    private int bulletCount;
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
        if (bulletCount < 0) bulletCount = 0;
        else if (bulletCount == bulletUi.Length) bulletCount -= 1;

        bulletUi[bulletCount].SetActive(_isUp);
        bulletCount = _isUp ? bulletCount + 1 : bulletCount - 1;
    }
}
