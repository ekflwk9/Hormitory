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
    private CountMatch countMatch; // countMatch 불러오기

    // 플레이어 컨트롤러에서 화면 움직임 잠금
    private PuzzlePlayerController playerController;

    private Door_Match targetDoor; // 상호작용 대상 문

    private void Awake()
    {

        countMatch = new CountMatch();
        // CountMatch 생성자에서 자동 등록됨
        PuzzleManager.instance.RegisterPuzzle(this); //퍼즐매니저에 컨트롤러 등록        
    }

    private void Start()
    {
        playerController = FindObjectOfType<PuzzlePlayerController>();
        if (playerController == null)
        {
            Service.Log("CountMatchController: PlayerController를 찾을 수 없습니다.");
        }
        else
        {
            playerController.LockInput(); // 시작 시 커서 잠금
        }
    }

    public void SetRequiredNum(int num)
    {
        countMatch.SetRequiredNum(num);
        ShowUI(true);
    }

    public void SetTargetDoor(Door_Match door)
    {
        targetDoor = door;
        if (targetDoor == null)
        {
            Service.Log("CountMatchController: Target Door is null");
            // 게임은 계속 진행 가능.
        }
    }

    //UI와 Cursor의 OnOff를 bool값으로 한 번에 조절
    private void ShowUI(bool isOn)
    {
        Cursor.lockState = isOn? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOn;
    }

    private void HandleSolved()
    {
        Service.Log("CountMatch: 성공했습니다. 다음 장면 실행.");
        // 성공 시 처리 로직
        // SE: 문이 열리는 소리(달칵)
        if (targetDoor != null)
        {
            targetDoor.isSolved = true;
            // 또는 targetDoor.OpenDoor() 등으로 애니메이션 처리
        }
        else
        {
            //없어도 게임은 계속 진행가능하도록
            Service.Log("CountMatchController: 대상인 문은 없지만, 퍼즐은 풀림.");
        }
        IsPuzzleSolved(true); // 퍼즐이 해결되었음을 알림
        playerController.UnlockInput(); // 플레이어 컨트롤러의 입력 잠금 해제
        UiManager.Instance.Show<LockUi>(false);
        ShowUI(false);
    }

    // 퍼즐이 해결되었는지 여부 반환
    public bool IsPuzzleSolved(bool isCleard)
    {
        return isCleard;
    }

    private void HandleFailed()
    {
        Service.Log("CountMatch: 실패했습니다. 게임오버로직 실행.");
        if (targetDoor != null)
        {
            targetDoor.isSolved = false;
            // 또는 targetDoor.OpenDoor() 등으로 애니메이션 처리
        }
        else
        {
            //없어도 게임은 계속 진행가능하도록
            Service.Log("CountMatchController: 대상의 문이 없고, 퍼즐도 오답임.");
        }
        IsPuzzleSolved(false); // 퍼즐 미해결
        playerController.UnlockInput(); // 플레이어 컨트롤러의 입력 잠금 해제
        UiManager.Instance.Show<LockUi>(false);
        // 실패 시 처리로직 (게임오버)
        ShowUI(false);
    }
    private void OnEnable()
    {
        countMatch.OnSolved += HandleSolved; //성공 이벤트 구독
        countMatch.OnFailed += HandleFailed; //실패 이벤트 구독
    }

    private void OnDisable()
    {
        countMatch.OnSolved -= HandleSolved; //성공 이벤트 구독
        countMatch.OnFailed -= HandleFailed; //실패 이벤트 구독
    }

}
