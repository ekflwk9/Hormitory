using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  퍼즐 전반을 묶어주는 곳.
///  다른 객체에서 퍼즐을 호출하기 쉽도록 함.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    // 모든 퍼즐을 쉽게 접근 할 수 있도록 Instance하는 역할. 
    // 모든 퍼즐은 Dictionary로 관리하며, 모든 퍼즐은 IPuzzle 인터페이스를 가진다. (List로 하게 될지도 모릅니다)

    public CountMatch CountMatch;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else Destroy(this.gameObject);
    }
}
