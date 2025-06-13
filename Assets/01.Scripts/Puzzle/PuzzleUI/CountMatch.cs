using System;
using UnityEngine;

/// <summary>
/// 4자리 숫자를 맞추는 오브젝트
/// </summary>
public class CountMatch : IPuzzle
{
  

    // Button들과 Text의 배열 순서 맞추기 (0~3)
    

    //맞춰야 할 횟수
    private int maxChance = 3;
    private int failCount = 0;
    private int[] currentNums = new int[4]; //4개의 칸에 설정된 현재 수 = numTexts 받아오기
    private int requireNum; // 외부에서 받아온 RequiredNum(비밀번호)을 저장하는 변수


    public event System.Action OnSolved; // 해결 이벤트
    public event System.Action OnFailed; // 실패 이벤트
    public event System.Action<int[]> OnNumbersChanged; // 화면의 숫자(기존 curNum) 변경 시 이벤트


    // 퍼즐 시작시 로직
    // 1. Numbers와 Buttons 게임 오브젝트를 SetActive true로 변경
    // 2. RequireNum이 존재한다면 퍼즐 로직을 진행
    // 3. 할당되지 않았다면 오류.


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
        //실패횟수 초기화, User가 조작한(할) 숫자배열 초기화
        failCount = 0;
        ResetNum();
        // 빈 칸의 모든 수를 0으로 변경(초기값)
    }

    private void ResetNum()
    {
        //숫자를 초기값(0000)으로 바꿔주는 메서드
       for(int i = 0; i < currentNums.Length; i++)
        {
            //현재 설정된 숫자를 모두 0으로 변경하고
            currentNums[i] = 0;
            // Text에 표기된 숫자들도 전부 0으로 재설정
            OnNumbersChanged?.Invoke(currentNums);
        }
    }

    public void ChangeNumber(int index, int value)
    {
        //index번의 숫자를 value씩 증감시켜주는 로직
        if (index < 0 || index >= currentNums.Length) return; 
        //SetRequiredNum에서 이미 0과 9999 사이로 지정했지만, 만약을 위한 방어

        currentNums[index] = (currentNums[index] + value + 10) % 10;
        // 0~9를 순환하도록 함 = 0번에서 +1 +10 /10 = 11/10 =1, 9번이면 10/10 = 0 이 되도록
        OnNumbersChanged?.Invoke(currentNums); 
        // 숫자가 바뀌었으므로,숫자 변경 이벤트 호출

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
    public void GuessNum()
    {
        //requireNum과, TMP_Text 배열에 완성될 숫자들의 배열이 동일하면 성공이다.

        // 1. 일단 버튼을 통해 TMP_Text의 숫자를 조정한다.=currentNums의 배열과 동일하다.

        int userNum = 0;
        for(int i = 0; i<currentNums.Length; i++)
        {
            userNum = userNum * 10 + currentNums[i];
        }

        // 1624의 경우, userNum = 0+1 = 1, userNum = 10+6 = 16, userNum = 160+2=162, userNum = 1620+4 = 1624
        // 즉, 배열의 가장 첫번째 수를 userNum에 넣고, 이후 그것을 *10하면서 뒷자리를 더함 = 최종으로 4자리 수가 나옴


        // 2. 조정된 숫자와 맞춰야 할 숫자를 확인한다.
        // 3. 동일하면 IsSolved로, 틀리면 Incorrect로

        if(userNum == requireNum)
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
