using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dagger : MonoBehaviour, IInteractable
{
    bool isCleared = false; // 퍼즐이 완료되었는지
    [SerializeField] private ItemSO daggerData;
    public ItemSO ItemData
    {
        get { return daggerData; }
        set { daggerData = value; }
    }
    public void Interact()
    {
        //여기서 퍼즐 호출
        //퍼즐에 해당하는 UI 호출함
        if(isCleared == false)
        {
            //TimingPuzzle

            PuzzleManager.instance.GetPuzzle<TimingMatch>().StartPuzzle();
            //실패하면 게임오버로 넘어가므로, 여기서는 성공만 처리
            isCleared = true; // 퍼즐이 완료되었음을 표시
        }
        else
        {
            AfterInteraction();
        }
    }

    //일정 범위 내에 접근 시, E 혹은 상호작용 마커를 띄우는 메서드를 추가할 수 있음.

    private void AfterInteraction()
    {
        //아이템 획득 및 등록
        if (daggerData != null)
        {
            // 차후 인벤토리 UI에 따라, Item 개수를 2개로 제한
            // 아이템 등록과 동시에 ItemManager에서 Inventory에 추가함
            ItemManager.instance.RegisterItem(ItemData, ItemData.ItemID);
            Service.Log($"아이템 획득: {daggerData.ItemName} (ID: {daggerData.ItemID})");

            // 아이템 획득이 정상적으로 되었다면, Destroy (아이템을 획득하지 못했다면 Destroy하지 않음)
            // ItemManager에서 GetItemNumber를 통해 아이템이 등록되어 있는지 확인할 수 있음
            // 아이템이 없다면 -1, 등록되어있다면 해당 아이템의 번호를 반환함
            if (ItemManager.instance.GetItemNumber(daggerData) != -1)
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
