using UnityEngine;
using UnityEngine.UI;

public class SlotUi : UiBase
{
    private Image icon;
    private Image select;
    private RectTransform selectScale;

    private Color seletColor = new Color(0.278f, 0.764f, 1f, 1f);
    private Vector2 selectSize = new Vector2(155f, 155f);
    private Vector2 noneSize = new Vector2(125f, 125f);

    public override void Init()
    {
        select = this.TryGetComponent<Image>();
        selectScale = this.TryGetComponent<RectTransform>();
        select.color = Color.white;

        icon = this.TryGetChildComponent<Image>(StringMap.Icon);
        icon.color = Color.clear;
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
        if(_isActive)
        {
            select.color = seletColor;
            selectScale.sizeDelta = selectSize;
        }

        else
        {
            select.color = Color.white;
            selectScale.sizeDelta = noneSize;
        }
    }
}
