using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleReceiver : ItemReceiver
{
    //interaction 등 공통적인 내용은 ItemReceiver를 그대로 사용

    public override void InteractAction()
    {
        Service.Log($"샘플 아이템 사용:(ID: {itemData.ItemID})");
        // 상호작용 성공 시 수행할 액션 기재(예: 문 열림, 애니메이션, SE)
    }

    public override void AfterInteract()
    {
        Service.Log($"상호작용 완료 후, 추가되는 상호작용");
    }

}
