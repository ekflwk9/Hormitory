using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
