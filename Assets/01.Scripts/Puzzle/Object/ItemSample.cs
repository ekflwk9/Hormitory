using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 샘플의 아이템 데이터를 담고, Interaction을 통해 아이템을 획득할 수 있는 기능
/// </summary>
public class ItemSample : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;
    public ItemSO ItemData
    {
        get { return itemData; }
        set { itemData = value; }
    }

    public void Interact()
    {
        //아이템 획득 및 등록
        if(itemData != null)
        {
            // 차후 인벤토리 UI에 따라, Item 개수를 2개로 제한
            // 아이템 등록과 동시에 ItemManager에서 Inventory에 추가함
            ItemManager.instance.RegisterItem(ItemData, ItemData.ItemID);
            Service.Log($"아이템 획득: {itemData.ItemName} (ID: {itemData.ItemID})");

            // 아이템 획득이 정상적으로 되었다면, Destroy (아이템을 획득하지 못했다면 Destroy하지 않음)
            // ItemManager에서 GetItemNumber를 통해 아이템이 등록되어 있는지 확인할 수 있음
            // 아이템이 없다면 -1, 등록되어있다면 해당 아이템의 번호를 반환함
            if (ItemManager.instance.GetItemNumber(itemData) != -1)
            {
                Destroy(gameObject);
            }
            else
            {
                Service.Log("아이템 획득에 실패했습니다.");
            }
        }
    }
}
