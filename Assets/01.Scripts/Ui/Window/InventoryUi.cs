public enum SlotType
{
    None,
    FirstSlot,
    SecondSlot,
}

public class InventoryUi : UiBase
{
    private SlotUi firstSlot, secondSlot;

    public override void Init()
    {
        firstSlot = this.TryGetChildComponent<SlotUi>("FirstSlot");
        secondSlot = this.TryGetChildComponent<SlotUi>("SecondSlot");

        UiManager.Instance.Add<InventoryUi>(this);
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    public void SetView(SlotType _slot, int _itemId)
    {
        if (_slot == SlotType.FirstSlot) firstSlot.SetSlotView(_itemId);
        else secondSlot.SetSlotView(_itemId);
    }

    public void SlotSelection(SlotType _slot)
    {
        if (_slot == SlotType.FirstSlot)
        {
            firstSlot.Show(true);
            secondSlot.Show(false);
        }

        else if (_slot == SlotType.SecondSlot)
        {
            firstSlot.Show(false);
            secondSlot.Show(true);
        }

        else
        {
            firstSlot.Show(false);
            secondSlot.Show(false);
        }
    }
}
