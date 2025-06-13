using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마커가 좌우로 오가며 핀포인트를 찾는 게임
/// </summary>
public class TimingMatch : MonoBehaviour, IPuzzle
{
    //Dagger 에서 상호작용하면 여기로 온다.
    // StartPuzzle 에서 게임 시작.

    [Header("활성 UI 오브젝트")]
    [SerializeField] private GameObject RailObj; //가로길이  1000
    [SerializeField] private RectTransform marker; //마커 UI 위치 생성 범위 200~800(한 곳에 너무 치우치지 않도록)
    [SerializeField] private RectTransform pinPoint; //핀포인트 UI 


    [Header("Game 카운터")]
    [SerializeField] private int maxCycles = 3; // 왕복 3회 고정, 외부 호출 없을 시 아예 [SerializeField] 제거
    [SerializeField] private int success = 0; // 성공 횟수 = 3회 성공 시 퍼즐 해제
    [SerializeField] private int failed = 0; // 실패 횟수 =3회 실패시 게임오버

    private int direction = 1; // 1은 양수 =>오른쪽방향, -1은 Flip. 음수. <= 왼쪽방향


    public void StartPuzzle()
    {
        throw new System.NotImplementedException();
    }
    public void IsSolved()
    {
        throw new System.NotImplementedException();
    }

    public void IsFailed()
    {
        throw new System.NotImplementedException();
    }




}
