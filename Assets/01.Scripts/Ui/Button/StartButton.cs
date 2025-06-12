using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.Get<FadeUi>().OnFade(NextScene);
    }

    private void NextScene()
    {
        SceneManager.LoadScene(StringMap.Puzzle);
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
