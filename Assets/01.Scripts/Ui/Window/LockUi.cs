using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockUi : UiBase
{
    [SerializeField] private TMP_Text[] number;
    [SerializeField] private int[] passWord;

    public override void Init()
    {
        var tempList = new List<TMP_Text>();
        var tempArray = GetComponentsInChildren<TMP_Text>();

        for (int i = 0; i < tempArray.Length; i++)
        {
            if (tempArray[i].name.Contains("Number"))
            {
                tempList.Add(tempArray[i]);
            }
        }

        number = tempList.ToArray();
        passWord = new int[number.Length];

        var buttonComponent = GetComponentsInChildren<SetLockButton>();
        var findCount = 0;
        var count = 0;

        for (int i = 0; i < buttonComponent.Length; i++)
        {
            if (count == 2)
            {
                count = 0;
                findCount++;
            }

            buttonComponent[i].SetButtonIndex(findCount);
            count++;
        }

        UiManager.Instance.Add<LockUi>(this);
    }

    public override void Show(bool _isActive)
    {
        base.Show(_isActive);
        UiManager.Instance.Get<FadeUi>().OnFade();

        if (!_isActive)
        {
            for (int i = 0; i < number.Length; i++)
            {
                passWord[i] = 0;
            }
        }
    }

    public void SetNumber(int _buttonIndex, int _value)
    {
        if (_buttonIndex < 0 || _buttonIndex >= number.Length)
        {
            Service.Log($"{_buttonIndex}가 number.Length보다 높음");
            return;
        }

        passWord[_buttonIndex] += _value;

        if (passWord[_buttonIndex] < 0) passWord[_buttonIndex] = 9;
        else if (passWord[_buttonIndex] > 9) passWord[_buttonIndex] = 0;

        number[_buttonIndex].text = passWord[_buttonIndex].ToString();
    }

    public void SetPassWord()
    {
        //정보 전송

        //PuzzleManager.instance.
    }
}
