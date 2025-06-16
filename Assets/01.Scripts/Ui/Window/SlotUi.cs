using UnityEngine;
using UnityEngine.UI;

public class SlotUi : UiBase
{
    private Image icon;

    public override void Init()
    {
        icon = this.TryGetChildComponent<Image>(StringMap.Icon);
        icon.color = Color.clear;
    }

    public void SetSlotView(int _itemId)
    {
        var itemId = 0;

        if (itemId == 0)
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
}
