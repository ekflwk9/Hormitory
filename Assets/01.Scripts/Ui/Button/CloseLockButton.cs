using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseLockButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);
        UiManager.Instance.Show<LockUi>(false);
        UiManager.Instance.Get<LockUi>().ResetPassWord();
    }
}
