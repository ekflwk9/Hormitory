using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureState : BaseState
{
    public CaptureState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    
    
    public override void Enter()
    {
        base.Enter();
        StateMachine.ResetTalkTimer();
        SoundManager.PlaySfx(SoundCategory.Movement,"PuzzleMonster4");
        NavMeshAgent.isStopped = true;
        PuzzlePlayerController.Die();
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.CaptureParameterHash);
        DeadCam.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();
        NavMeshAgent.isStopped = false;
        PuzzlePlayerController.UnlockInput();
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.CaptureParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }
}
