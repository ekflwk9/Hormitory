using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    public ChaseState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
   
    public override void Enter()
    {
        NavMeshAgent.speed = StateMachine.ChaseSpeed;
        NavMeshAgent.isStopped = false;
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        NavMeshAgent.SetDestination(PlayerTransform.position);
        float distance = Vector3.Distance(MonsterTransform.position,PlayerTransform.position);

        if (distance > DetectRange ) // || hide 상태
        {
            StateMachine.TransitionTo(MonsterStateType.Search);
        }
    }
    public override void Exit()
    {
        NavMeshAgent.speed = StateMachine.DefaultSpeed;
        NavMeshAgent.isStopped = true;
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.ChaseParameterHash);
    }

}
