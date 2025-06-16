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
        PuzzlePlayerController.Die();
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.CaptureParameterHash);
        MainCam.enabled = false;
        DeadCam.enabled = true;
        UiManager.Instance.Get<HitUi>().HitView();
        
        UiManager.Instance.Show<DeadUi>(true);
        
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
