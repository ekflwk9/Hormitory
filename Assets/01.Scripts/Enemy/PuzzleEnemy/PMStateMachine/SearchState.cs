using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    public SearchState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //애니메이션
    }

    public override void Exit()
    {
        //애니메이션 리셋
    }

    private IEnumerator SerchCoroutine()
    {
        float timer = 0f;
        while (timer < SearchDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        float distance = Vector3.Distance(MonsterTransform.position,PlayerTransform.position);

        if (distance <= DetectRange)
        {
            StateMachine.TransitionTo(MonsterStateType.Chase);
        }
        else
        {
            StateMachine.TransitionTo(MonsterStateType.Idle);
        }
    }
}
