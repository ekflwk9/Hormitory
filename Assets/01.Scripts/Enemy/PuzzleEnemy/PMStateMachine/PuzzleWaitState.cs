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
        //정지
    }

    public override void Exit()
    {
        NavMeshAgent.isStopped = false;
        //다시 움직임
    }
}
