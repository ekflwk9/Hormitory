using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState : BaseState
{
    public CaptureState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    
    
    public override void Enter()
    {
        NavMeshAgent.isStopped = true;
        //연출
       
        //gameOverUI 아니면 놓아주기
    }

    public override void Exit()
    {
        NavMeshAgent.isStopped = false;// 게임오버면 다시 움직임 필요없을지도
    }

    public override void Update()
    {
        
    }
    
    
}
