using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UiButton : UiBase, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected GameObject touch;

    public override void Init()
    {
        touch = this.TryFindChild(StringMap.Touch).gameObject;
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        touch.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (touch.activeSelf) touch.SetActive(false);
    }
}
