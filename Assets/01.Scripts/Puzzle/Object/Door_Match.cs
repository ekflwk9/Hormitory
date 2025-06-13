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

    public void Interact()
    {
        // 1. 해당 오브젝트의 MatchData를 가져온다 =>할당
        // 2. CountMatch의 StartPuzzle을 시작시킨다.

        MatchCount = MatchData.RequiredNum;


        PuzzleManager.instance.CountMatch.SetRequiredNum(MatchCount);
    }

}
