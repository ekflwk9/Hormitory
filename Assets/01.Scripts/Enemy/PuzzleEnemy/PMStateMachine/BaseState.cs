using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseState : IState
{
    protected readonly MonsterStateMachine StateMachine;
    protected Transform MonsterTransform => StateMachine.transform;
    protected Transform PlayerTransform => StateMachine.PlayerTransform;
    protected NavMeshAgent NavMeshAgent => StateMachine.NavMeshAgent;
    protected float DetectRange => StateMachine.DetectRange;
    protected float SearchDuration => StateMachine.SearchDuration;

    protected float PatrolRadius => StateMachine.PatrolRadius;
    protected float PatrolWaitTime => StateMachine.PatrolWaitTime;
    protected float CaptureRange => StateMachine.CaptureRange;
    protected PuzzlePlayerController PuzzlePlayerController => StateMachine.PuzzlePlayerController;
    protected  Camera DeadCam => StateMachine.DeadCam;
    protected bool IsPuzzle => StateMachine.IsPuzzle;
    
    

    
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
        if (IsPuzzle)
        {
            StateMachine.TransitionTo(MonsterStateType.PuzzleWait);
        }
    }

    protected void StartAnimation(int animationHash)
    {
        StateMachine.PuzzleMonster.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        StateMachine.PuzzleMonster.Animator.SetBool(animationHash, false);
    }

    
}
