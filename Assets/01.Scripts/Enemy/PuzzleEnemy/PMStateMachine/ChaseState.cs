using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    public ChaseState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
   
   
    public override void Enter()
    {
        base.Enter();
        NavMeshAgent.speed = StateMachine.ChaseSpeed;
        NavMeshAgent.isStopped = false;
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        base.Update();
        NavMeshAgent.SetDestination(PlayerTransform.position);
        float distance = Vector3.Distance(MonsterTransform.position,PlayerTransform.position);

        if (distance > DetectRange  || PuzzlePlayerController.IsHiding) 
        {
            
            StateMachine.TransitionTo(MonsterStateType.Search);
        }

        if (distance <= CaptureRange)
        {
            StateMachine.TransitionTo(MonsterStateType.Capture);
        }
            
        
    }
    public override void Exit()
    {
        base.Exit();
        NavMeshAgent.speed = StateMachine.DefaultSpeed;
        NavMeshAgent.isStopped = true;
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.ChaseParameterHash);
    }
    
    

}
