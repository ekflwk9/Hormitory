using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState : BaseState
{
    //private Coroutine _coroutine;
    public CaptureState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    
    
    public override void Enter()
    {
        NavMeshAgent.isStopped = true;
        PuzzlePlayerController.LockInput();
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.CaptureParameterHash);
        MainCam.enabled = false;
        DeadCam.enabled = true;
        PuzzlePlayerController.Die();
        //UI띄워야되나
        
    }

    public override void Exit()
    {
        NavMeshAgent.isStopped = false;
        PuzzlePlayerController.UnlockInput();
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.CaptureParameterHash);
        MainCam.enabled = true;
        DeadCam.enabled = false;
    }

    public override void Update()
    {
        
    }

    
    

}
