using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private Coroutine _searchCoroutine;
    public SearchState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        NavMeshAgent.isStopped = true;
        
        StartAnimation(StateMachine.PuzzleMonster.AnimationData.SearchParameterHash);
        _searchCoroutine = StateMachine.StartCoroutine(SearchCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        if (_searchCoroutine != null)
        {
            StateMachine.StopCoroutine(_searchCoroutine);
            _searchCoroutine = null;
        }
        NavMeshAgent.isStopped = false;
        
        StopAnimation(StateMachine.PuzzleMonster.AnimationData.SearchParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    private IEnumerator SearchCoroutine()
    {
        float timer = 0f;
        while (timer < SearchDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        float distance = Vector3.Distance(MonsterTransform.position,PlayerTransform.position);

        if (distance <= DetectRange && !PuzzlePlayerController.IsHiding)
        {
            StateMachine.TransitionTo(MonsterStateType.Chase);
        }
        else
        {
            StateMachine.TransitionTo(MonsterStateType.Idle);
        }
    }
}
