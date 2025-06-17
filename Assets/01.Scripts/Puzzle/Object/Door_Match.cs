using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 숫자 맞추기 퍼즐이 있는 문에 달린 스크립트
/// CountMatchSO에서 데이터를 받아옴.
/// </summary>
public class Door_Match : MonoBehaviour, IInteractable
{
    public CountMatchSO MatchData;
    public int MatchCount;

    //private Animator animator;

    private bool isInteracted = false; // 첫 상호작용이 있었는지 여부

    public bool isSolved = false; // 퍼즐이 해결되었는지 여부

    public void Interact()
    {
        MatchCount = MatchData.RequiredNum;
        // 1. 해당 오브젝트의 MatchData를 가져온다 =>할당
        // 2. CountMatch의 StartPuzzle을 시작시킨다.
        if (isSolved == false && isInteracted == false)
        {
            //즉, 첫 상호작용(문이 잠겼음을 확인)
            UiManager.Instance.Get<TalkUi>().Popup("젠장, 좌물쇠가 걸려있잖아!");
            SoundManager.PlaySfx(SoundCategory.Interaction, "LockDoor");
            isInteracted = true; // 첫 상호작용이 있었음을 표시
        }
        else if (!isSolved)
        {
            UiManager.Instance.Show<LockUi>(true);
            PuzzleManager.instance.GetPuzzle<CountMatchController>().SetTargetDoor(this);
            PuzzleManager.instance.GetPuzzle<CountMatchController>().SetRequiredNum(MatchCount);
            //퍼즐이 시작되고 해결되면, 퍼즐이 해결됨.
        }
        else if (isSolved)
        {
            Service.Log("Door_Match: 퍼즐이 해결되었고, 이제부턴 문을 열고 닫을 수 있습니다.");

            //퍼즐 해결 후 상호작용이 있을 경우 로직
            // 문을 열거나 닫을 수 있음
        }
        
    }

}
