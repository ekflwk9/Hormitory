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

    public void SetSlotView()
    {
        var itemId = 0;

        if (itemId == 0)
        {
            //var item = ItemManager.GetItem(itemId);
            //icon.sprite = item.sprite;
            icon.color = Color.white;
        }

        else
        {
            icon.color = Color.clear;
        }
    }
}
