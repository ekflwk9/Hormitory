using _01.Scripts.Component;
using UnityEngine.EventSystems;

public class PlayButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        touch.SetActive(false);                
        UiManager.Instance.Show<MenuUi>(false);
        PlayerManager.Instance.Player.SetPauseState(false);
    }
}
