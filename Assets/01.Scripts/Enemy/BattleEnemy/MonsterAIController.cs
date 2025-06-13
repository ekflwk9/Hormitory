using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAIController : MonoBehaviour
{
    [Header("Nodes")]
    private SelectorNode rootNode = new SelectorNode(); // 루트노드 
    
    private SequenceNode flyingBodySlam = new SequenceNode(); // 날아오른 후 몸통박치기 시퀀스
    private SequenceNode jumpingBodySlam = new SequenceNode(); // 뛰어서 몸통박치기 시퀀스
    private SequenceNode tailAttack = new SequenceNode(); // 꼬리 공격 시퀀스
    private SequenceNode chasingPlayer = new SequenceNode(); // 플레이어 쫓기 시퀀스

    private ActionNode currentAction; // 현재 진행중인 액션 저장용
    
    private ActionNode currentActionNull; // 
    private ActionNode currentActionNullOrChasingPlayer;
    
    private ActionNode timerPassed;
    private ActionNode flyBodySlam;

    private ActionNode playerOutOfDetectDistance;
    private ActionNode jumpBodySlam;

    private ActionNode playerInAttackDistance;
    private ActionNode tailAttacking;
    
    private ActionNode chasePlayer;

    private ActionNode currentActionStillRunning;

    private void Awake()
    {
        currentActionNull = new ActionNode(IsCurrentActionNull);
        currentActionNullOrChasingPlayer = new ActionNode(IsCurrentActionNullOrChasingPlayerAction);
        timerPassed = new ActionNode(IsTimerPassed);
        flyBodySlam = new ActionNode(FlyingAndBodySlam);
        playerOutOfDetectDistance = new ActionNode(IsPlayerOutOfDetectDistance);
        jumpBodySlam = new ActionNode(JumpingAndBodySlam);
        playerInAttackDistance = new ActionNode(IsPlayerInAttackDistance);
        tailAttacking = new ActionNode(TailAttack);
        chasePlayer = new ActionNode(ChasingPlayer);
        currentActionStillRunning = new ActionNode(CurrentActionStillRunning);
        
        flyingBodySlam.Add(timerPassed);
        flyingBodySlam.Add(currentActionNull);
        flyingBodySlam.Add(flyBodySlam);
        
        jumpingBodySlam.Add(playerOutOfDetectDistance);
        jumpingBodySlam.Add(currentActionNull);
        jumpingBodySlam.Add(jumpBodySlam);
        
        tailAttack.Add(playerInAttackDistance);
        tailAttack.Add(currentActionNullOrChasingPlayer);
        tailAttack.Add(tailAttacking);
        
        chasingPlayer.Add(currentActionNull);
        chasingPlayer.Add(chasePlayer);
        
        rootNode.Add(flyingBodySlam);
        rootNode.Add(jumpingBodySlam);
        rootNode.Add(tailAttack);
        rootNode.Add(chasingPlayer);
        rootNode.Add(currentActionStillRunning);
    }

    private void Update()
    {
        rootNode.Evaluate();
    }

    private INode.State IsTimerPassed()
    {
        return INode.State.SUCCESS;
    }

    private INode.State IsCurrentActionNull()
    {
        return INode.State.SUCCESS;
    }

    private INode.State FlyingAndBodySlam()
    {
        return INode.State.RUN;
    }

    private INode.State IsPlayerOutOfDetectDistance()
    {
        return INode.State.SUCCESS;
    }

    private INode.State JumpingAndBodySlam()
    {
        return INode.State.RUN;
    }

    private INode.State IsPlayerInAttackDistance()
    {
        return INode.State.SUCCESS;
    }

    private INode.State IsCurrentActionNullOrChasingPlayerAction()
    {
        return INode.State.SUCCESS;
    }

    private INode.State TailAttack()
    {
        return INode.State.RUN;
    }

    private INode.State ChasingPlayer()
    {
        return INode.State.RUN;
    }

    private INode.State CurrentActionStillRunning()
    {
        return INode.State.RUN;
    }
}
