using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlotUi : UiBase
{
    private Image icon;
    private Image select;

    [SerializeField]private RectTransform iconScale;
    [SerializeField] private RectTransform selectScale;

    private Color seletColor = new Color(0.278f, 0.764f, 1f, 1f);
    private Vector2 selectSize = new Vector2(155f, 155f);
    private Vector2 noneSize;

    public override void Init()
    {
        select = this.TryGetComponent<Image>();
        selectScale = this.TryGetComponent<RectTransform>();
        select.color = Color.clear;

        icon = this.TryGetChildComponent<Image>(StringMap.Icon);
        iconScale = icon.TryGetComponent<RectTransform>();
        icon.color = Color.clear;

        noneSize = this.TryGetComponent<RectTransform>().sizeDelta;
    }

    public void SetSlotView(int _itemId)
    {
        if (_itemId != 0)
        {
            var itemData = ItemManager.instance.Getitem(_itemId);

            icon.sprite = itemData.Icon;
            icon.color = Color.white;
        }

        else
        {
            icon.color = Color.clear;
        }
    }

    public override void Show(bool _isActive)
    {
        icon.DOKill();
        select.DOKill();

        icon.color = Color.white;
        select.color = Color.white;

        if (_isActive)
        {
            select.color = seletColor;

            selectScale.sizeDelta = selectSize;
            iconScale.sizeDelta = selectSize;
        }

        else
        {
            selectScale.sizeDelta = noneSize;
            iconScale.sizeDelta = noneSize;
        }

        icon.DOFade(0, 2f);
        select.DOFade(0, 2f);
    }
}
