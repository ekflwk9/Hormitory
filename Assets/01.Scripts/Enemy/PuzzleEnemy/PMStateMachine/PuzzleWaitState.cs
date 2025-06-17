using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWaitState: BaseState
{
    public PuzzleWaitState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        NavMeshAgent.isStopped = true;
        //정지
    }

    public override void Exit()
    {
        //다시 움직임
        base.Exit();
        NavMeshAgent.isStopped = false;
        StateMachine.TransitionTo(MonsterStateType.Idle);
    }
}
