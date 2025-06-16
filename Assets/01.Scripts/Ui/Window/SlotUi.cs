using _01.Scripts.Component;
using UnityEngine;
using UnityEngine.UI;

public class SlotUi : UiBase
{
    private Image icon;
    private Image select;

    public override void Init()
    {
        select = this.TryGetComponent<Image>();
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
        select.color = _isActive ? new Color(0.278f, 0.764f, 1f, 1f) : Color.white;
    }
}
