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
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.PuzzleWaitParameterHash);
                
    }

    public override void Exit()
    {
        base.Exit();
        NavMeshAgent.isStopped = false;
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.PuzzleWaitParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!StateMachine.IsPuzzle)
        {
            StateMachine.TransitionTo(MonsterStateType.Idle);
        }
    }
}
