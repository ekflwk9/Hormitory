using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : BaseState
{
   public IdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    private Vector3 _currentDestination;
    private float _waitTimer;

    public override void Enter()
    {
        StateMachine.ResetTalkTimer();
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.WalkParameterHash);
        NavMeshAgent.isStopped = false;
        _waitTimer = 0f;
        SetNextDestination();

    }

    public override void Update()
    {
        base.Update();
        float distanceToPlayer = Vector3.Distance(MonsterTransform.position, PlayerTransform.position);
        if (distanceToPlayer <= DetectRange && !PuzzlePlayerController.IsHiding )
        {
            StateMachine.TransitionTo(MonsterStateType.Chase);
            return;
        }
        float distanceToDestination = Vector3.Distance(MonsterTransform.position,_currentDestination);
        if (distanceToDestination <= 0.5f)
        {
            if (_waitTimer <= 0f)
            {
                _waitTimer = PatrolWaitTime;
            }
            else
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0f)
                {
                    SetNextDestination();
                }
            }
        }
    }

    public override void Exit()
    {
        NavMeshAgent.isStopped = true;
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.WalkParameterHash);
    }

    private void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * PatrolRadius;
        randomDirection += MonsterTransform.position;

        NavMeshHit hitInfo;
        Vector3 targetPosition = MonsterTransform.position;

        if (NavMesh.SamplePosition(randomDirection, out hitInfo, PatrolRadius, NavMesh.AllAreas))
        {
            targetPosition = hitInfo.position;
        }

        _currentDestination = targetPosition;
        NavMeshAgent.SetDestination(_currentDestination);
    }   
}
