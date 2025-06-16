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
        NavMeshAgent.isStopped = true;
        //애니메이션
    }

    public override void Exit()
    {
        NavMeshAgent.isStopped = false;
        //애니메이션
    }
}
