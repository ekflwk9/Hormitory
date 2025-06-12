public class InventoryUi : UiBase
{
    private SlotUi firstSlot, secondSlot;

    public override void Init()
    {
        firstSlot = this.TryGetChildComponent<SlotUi>("FirstSlot");
        secondSlot = this.TryGetChildComponent<SlotUi>("SecondSlot");

        UiManager.Instance.Add<InventoryUi>(this);
    }

    public void SetView()
    {
        firstSlot.SetSlotView();
        secondSlot.SetSlotView();
    }
}
