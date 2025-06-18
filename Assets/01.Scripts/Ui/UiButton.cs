using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UiButton : UiBase, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameObject touch;

    private void Reset()
    {
        touch = this.TryFindChild(StringMap.Touch).gameObject;
    }

    public override void Init()
    {
        if (touch == null) touch = this.TryFindChild(StringMap.Touch).gameObject;
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UiManager.Instance.Get<FadeUi>().isFade)
        {
            touch.SetActive(true);
            SoundManager.PlaySfx(SoundCategory.Impacts, "UI_Click");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (touch.activeSelf) touch.SetActive(false);
    }
}
