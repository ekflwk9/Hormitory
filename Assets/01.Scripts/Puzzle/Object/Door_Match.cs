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

    private bool isSolved = false; // 퍼즐이 해결되었는지 여부

    public void Interact()
    {
        // 1. 해당 오브젝트의 MatchData를 가져온다 =>할당
        // 2. CountMatch의 StartPuzzle을 시작시킨다.
        if (!isSolved)
        {
            MatchCount = MatchData.RequiredNum;
            PuzzleManager.instance.GetPuzzle<CountMatchController>().SetRequiredNum(MatchCount);
            //퍼즐이 시작되고 해결되면, 퍼즐이 해결됨.

            isSolved = true; // 퍼즐이 해결되었음을 표시

        }
        else if (isSolved)
        {
            //퍼즐 해결 후 상호작용이 있을 경우 로직
            // 문을 열거나 닫을 수 있음
        }
        
    }

}
