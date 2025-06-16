using System;
using UnityEngine;

/// <summary>
/// 4자리 숫자를 맞추는 오브젝트
/// </summary>
public class CountMatch : IPuzzle
{
     //맞춰야 할 횟수
    private int maxChance = 3;
    private int failCount = 0;
    private int requireNum; // 외부에서 받아온 RequiredNum(비밀번호)을 저장하는 변수


    public event System.Action OnSolved; // 해결 이벤트
    public event System.Action OnFailed; // 실패 이벤트

  
    // 퍼즐 시작시 로직
    // 1. Numbers와 Buttons 게임 오브젝트를 SetActive true로 변경
    // 2. RequireNum이 존재한다면 퍼즐 로직을 진행
    // 3. 할당되지 않았다면 오류.


    public CountMatch()
    {
        PuzzleManager.instance.RegisterPuzzle(this);
    }


    // 정답 세팅 로직(상호작용한 오브젝트의 Interaction에서 호출
    public void SetRequiredNum(int numData)
    {
        //여기서 numData를 반드시 0과 9999 사이로 결정
        requireNum = Mathf.Clamp(numData, 0, 9999);
        Service.Log($"해당 퍼즐의 RequiredNum(정답): {requireNum}"); //정상적으로 데이터를 받아왔는지 확인(체크 완료)

        StartPuzzle();
    }

    public void StartPuzzle()
    {
        // 퍼즐 시작, 초기화
        failCount = 0;
    }

    public void IsSolved()
    {
        OnSolved?.Invoke();
        //해결 이벤트 호출
    }

    public void IsFailed()
    {
        OnFailed?.Invoke();
        //실패 이벤트 호출
    }

    //숫자를 맞추는 로직
    public void GuessNum(int userNum)
    {
        //LockUi에서 보낸 4자리 수를 받아서 RequireNum과 비교하는 로직

        // 1. userNum을 받아온다.
        // 2. 조정된 숫자와 맞춰야 할 숫자를 확인한다.
        // 3. 동일하면 IsSolved로, 틀리면 Incorrect로
        Service.Log($"입력한 정답{userNum}, 요구되는 정답{requireNum}");

        if (userNum == requireNum)
        {
            IsSolved();
        }
        else
        {
            Incorrect();
        }
    }

    public void Incorrect()
    {
        failCount++;
        // 3회의 기회를 모두 소모하면.
        if(failCount >= maxChance)
        {
            //게임 실패 연출
            IsFailed();
            return;
        }

        //여기에 SE: 심장소리/괴물 소리
        Service.Log($"CountMatch: Incorrect : {failCount}");

        //숫자맞추기에 실패했을 때(게임오버는 아직 아닐 때)의 로직
        //오류 처리 후 다시 숫자 선택로직으로 변경함
        //3회 오류 시 IsFailed로
    }


}
