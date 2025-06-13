using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BaseState : IState
{
    protected readonly MonsterStateMachine StateMachine;
    protected Transform MonsterTransform => StateMachine.transform;
    protected Transform PlayerTransform => StateMachine.PlayerStransform;
    protected NavMeshAgent NavMeshAgent => StateMachine.NavMeshAgent;
    protected float DetectRange => StateMachine.DetectRange;
    protected float SearchDuration => StateMachine.SearchDuration;

    protected BaseState(MonsterStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        
    }


    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }

}
