using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// 4자리 숫자를 맞추는 오브젝트
/// </summary>
public class CountMatch : MonoBehaviour, IPuzzle
{
    // UP, Down 버튼 배열

    [Header("활성UI 오브젝트")]
    [SerializeField] private GameObject numberObj;
    [SerializeField] private GameObject buttonObj;

    [Header("UP/DOWN 버튼")]
    [SerializeField] private Button[] upButtons;
    [SerializeField] private Button[] downButtons;

    [Header("선택 버튼")]
    [SerializeField] private Button selectButton;

    [Header("숫자 리스트")]
    [SerializeField] private TMP_Text[] numTexts;

    // Button들과 Text의 배열 순서 맞추기 (0~3)
    
    //상호작용한 오브젝트의 ScriptableObject에서 requiredNum을 가져오기
    private IRequireNumber requireNumberData;

    //맞춰야 할 횟수
    private int maxChance = 3;
    private int failCount = 0;
    private int[] currentNums = new int[4]; //4개의 칸에 설정된 현재 수 = numTexts 받아오기
    private int requireNum; // 외부에서 받아온 RequiredNum을 저장하는 변수

    // 정답 세팅 로직(상호작용한 오브젝트의 Interaction에서 호출
    public void SetRequiredNum(IRequireNumber numData)
    {
        requireNumberData = numData;
        StartPuzzle();
    }

    public void StartPuzzle()
    {
        // 퍼즐 시작시 로직
        // 1. Numbers와 Buttons 게임 오브젝트를 SetActive true로 변경
        // 2. RequireNum이 존재한다면 퍼즐 로직을 진행
        // 3. 할당되지 않았다면 오류.

        //1. 
        if (numberObj != null) numberObj.SetActive(true);
        else Service.Log("CountMatch: numberObj가 없으니 할당해주세요");
        if (buttonObj != null) buttonObj.SetActive(true);
        else Service.Log("CountMatch: buttonObj가 없으니 할당해주세요");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //2.
        if (requireNumberData != null)
        {
            requireNum = requireNumberData.RequiredNum;
            if(requireNum > 9999)
            {
                Service.Log($"CountMatch: RequiredNum이 9999를 초과하여, 값이 9999로 고정됩니다, 입력값: {requireNum}");
                requireNum = 9999;
            }
            else if (requireNum < 0)
            {
                Service.Log($"CountMatch: RequiredNum이 0 미만이므로, 값이 0000으로 고정됩니다. 입력값: {requireNum}");
                requireNum = 0;
            }
                Service.Log($"해당 퍼즐의 RequiredNum(정답): {requireNum}"); //정상적으로 데이터를 받아왔는지 확인(체크 완료)

            ResetNum(); // 빈 칸의 모든 수를 0으로 변경(초기값)
            
            for(int i = 0; i<numTexts.Length; i++)
            {
                int idx = i; // NumberChange에 연결할 배열 (1번 버튼을 누르면 1번 Text가 변경되도록)
                // 더 나은 방법이 있을 것 같은데 (체크)
                Service.Log($"CountMatch: for문 진입 현재 {idx} 번");

                // UP Button과 DownButton을 각 배열 순서에 맞춰서 연결지음.
                upButtons[idx].onClick.RemoveAllListeners(); // 중복 방지
                upButtons[idx].onClick.AddListener(() => ChangeNumber(idx, 1)); //up 버튼 클릭시 1 상승
                downButtons[idx].onClick.RemoveAllListeners();
                downButtons[idx].onClick.AddListener(()=> ChangeNumber(idx, -1)); //down 버튼 클릭시 1감소
            }

            if(selectButton != null)
            {
                //selectButton을 연결함
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(GuessNum);
            }

            //퍼즐 로직 진행=>GuessNum 메서드 먼저 완성할 것
            
        }
        else Service.Log("CountMatch: RequiredNum이 제대로 할당되지 않았습니다.");
    }

    private void ResetNum()
    {
        //숫자를 초기값(0000)으로 바꿔주는 메서드
       for(int i = 0; i < numTexts.Length; i++)
        {
            //현재 설정된 숫자를 모두 0으로 변경하고
            currentNums[i] = 0; 
            // Text에 표기된 숫자들도 전부 0으로 재설정
            if (numTexts != null && numTexts.Length > i && numTexts[i] != null)
                numTexts[i].text = "0";
        }
    }

    private void ChangeNumber(int index, int value)
    {
        //index번의 숫자를 value씩 증감시켜주는 로직
        currentNums[index] = (currentNums[index] + value + 10) % 10;
        // 0~9를 순환하도록 함 = 0번에서 +1 +10 /10 = 11/10 =1, 9번이면 10/10 = 0 이 되도록

        numTexts[index].text = currentNums[index].ToString(); // 현재 지정된 index의 숫자를 currentNums의 숫자로 변경함
    }


    public void IsSolved()
    {
        // 퍼즐이 종료되면, 다음 퍼즐을 위해 requireNumber = null로 변경
        requireNumberData = null;

        Service.Log("CountMatch: 성공했습니다. 다음 장면 실행.");
        // 성공 시 처리 로직
        // SE: 문이 열리는 소리(달칵)

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //UI 화면 off
        numberObj.SetActive(false);
        buttonObj.SetActive(false);
    }

    public void IsFailed()
    {
        // 퍼즐이 종료되면, requireNumber = null로 변경
        requireNumberData = null;
        Service.Log("CountMatch: 실패했습니다. 게임오버로직 실행.");

        // 실패 시 처리로직 (게임오버)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        numberObj.SetActive(false);
        buttonObj.SetActive(false);
    }

    //숫자를 맞추는 로직
    private void GuessNum()
    {
        if (requireNumberData == null) return;
        //requireNum과, TMP_Text 배열에 완성된 숫자들의 배열이 동일하면 성공이다.

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

    // 숫자 선택 로직 필요

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
