using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
   public IdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public void Enter()
    {
        //애니메이션 전환
        
    }

    public override void Update()
    {
        float distance = Vector3.Distance(MonsterTransform.position, PlayerTransform.position);
        if (distance <= DetectRange ) //&& player가 !hide 한 상태
        {
            StateMachine.TransitionTo(MonsterStateType.Chase);
        }
    }

    public void Exit()
    {
        //idle애니애이션 해재
    }
}
