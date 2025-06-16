using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 기존 스크립트에서 사용할 수 없게된 요소
/// 컨트롤러로 이사(버튼, TMP, SetActive등)
/// </summary>
public class CountMatchController : MonoBehaviour
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

    private CountMatch countMatch; // countMatch 불러오기

    private void Awake()
    {

        countMatch = new CountMatch();
        // CountMatch 생성자에서 자동 등록됨
        PuzzleManager.instance.RegisterPuzzle(this); //퍼즐매니저에 컨트롤러 등록


        countMatch.OnSolved += HandleSolved; //성공 이벤트 구독
        countMatch.OnFailed += HandleFailed; //실패 이벤트 구독
        countMatch.OnNumbersChanged += UpdateNumberTexts;
        // 데이터 상에서 바뀐 숫자를 TMP_text에 넣을 수 있도록 구독
    }

    public void SetRequiredNum(int num)
    {
        countMatch.SetRequiredNum(num);
        ShowUI(true);
        BindButtons();

    }

    //UI와 Cursor의 OnOff를 bool값으로 한 번에 조절
    private void ShowUI(bool isOn)
    {
        if (numberObj != null) numberObj.SetActive(isOn);
        else Service.Log("CountMatch: numberObj가 없으니 할당해주세요");
        if (buttonObj != null) buttonObj.SetActive(isOn);
        else Service.Log("CountMatch: buttonObj가 없으니 할당해주세요");

        Cursor.lockState = isOn? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOn;
    }

    // 배열에 맞춰 버튼과 숫자 입력칸을 매칭, 선택 버튼 할당
    private void BindButtons()
    {
        for (int i = 0; i < numTexts.Length; i++)
        {
            int idx = i; // NumberChange에 연결할 배열 (1번 버튼을 누르면 1번 Text가 변경되도록)
                         // 더 나은 방법이 있을 것 같은데 (체크)
            Service.Log($"CountMatch: for문 진입 현재 {idx} 번");

            // UP Button과 DownButton을 각 배열 순서에 맞춰서 연결지음.
            upButtons[idx].onClick.RemoveAllListeners(); // 중복 방지
            upButtons[idx].onClick.AddListener(() => countMatch.ChangeNumber(idx, 1)); //up 버튼 클릭시 1 상승
            downButtons[idx].onClick.RemoveAllListeners();
            downButtons[idx].onClick.AddListener(() => countMatch.ChangeNumber(idx, -1)); //down 버튼 클릭시 1감소
        }

        if (selectButton != null)
        {
            //selectButton을 연결함
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => countMatch.GuessNum());
        }

        
    }

    private void HandleSolved()
    {
        Service.Log("CountMatch: 성공했습니다. 다음 장면 실행.");
        // 성공 시 처리 로직
        // SE: 문이 열리는 소리(달칵)
        ShowUI(false);
    }

    private void HandleFailed()
    {
        Service.Log("CountMatch: 실패했습니다. 게임오버로직 실행.");

        // 실패 시 처리로직 (게임오버)
        ShowUI(false);
    }

    private void UpdateNumberTexts(int[] nums)
    {
        for (int i = 0; i < numTexts.Length && i < nums.Length; i++)
        {
            if (numTexts[i] != null)
                numTexts[i].text = nums[i].ToString();
        }
    }

}
