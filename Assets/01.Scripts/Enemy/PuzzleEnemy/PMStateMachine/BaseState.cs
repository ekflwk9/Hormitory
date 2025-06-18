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
    
    private float _idleHeartBeatInterval = 1.5f;
    private float _chaseHeartBeatInterval = 0.8f;
    private float _heartBeatTimer;


    
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
        PlayHeartBeat("Base");
        
    }

    protected void StartAnimation(int animationHash)
    {
        StateMachine.PuzzleMonster.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        StateMachine.PuzzleMonster.Animator.SetBool(animationHash, false);
    }
    protected void ResetHeartBeat()
    {
        _heartBeatTimer = 0f;
    }
    public void PlayHeartBeat(string _state)
    {
        _heartBeatTimer += Time.deltaTime;
        float _interval;
        if (_state == "Base")
        {
            _interval = _idleHeartBeatInterval;
            if (_heartBeatTimer >= _interval)
            {
                SoundManager.PlaySfx(SoundCategory.Movement, $"HeartBeat{_state}");
                ResetHeartBeat();
            }
        }
        else if (_state == "Chase")
        {
            _interval = _chaseHeartBeatInterval;
            if (_heartBeatTimer >= _interval)
            {
                SoundManager.PlaySfx(SoundCategory.Movement, $"HeartBeat{_state}");
                ResetHeartBeat();
            }
        }
            
    }
}

