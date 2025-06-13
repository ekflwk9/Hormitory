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
        NavMeshAgent.isStopped = false;
        //애니매이터
    }

    public override void Update()
    {
        NavMeshAgent.SetDestination(PlayerTransform.position);
        float distance = Vector3.Distance(MonsterTransform.position,PlayerTransform.position);

        if (distance > DetectRange) // || hide 상태
        {
            StateMachine.TransitionTo(MonsterStateType.Search);
        }
    }
    public override void Exit()
    {
        NavMeshAgent.isStopped = true;
        //애니매이션 종료
    }

}
