using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        //여기서 퍼즐 호출
        //퍼즐에 해당하는 UI 호출함

        //퍼즐이 끝나면 해당 오브젝트를 Destroy
    }

    //일정 범위 내에 접근 시, E 혹은 상호작용 마커를 띄우는 메서드를 추가할 수 있음.
}
