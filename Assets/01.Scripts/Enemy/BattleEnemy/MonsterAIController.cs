using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAIController : MonoBehaviour
{
    [Header("Nodes")]
    private SelectorNode rootNode = new SelectorNode(); // 루트노드 
    
    private SequenceNode flyingBodySlam = new SequenceNode(); // 날아오른 후 몸통박치기 시퀀스
    private SequenceNode jumpingBodySlam = new SequenceNode(); // 뛰어서 몸통박치기 시퀀스
    private SequenceNode tailAttack = new SequenceNode(); // 꼬리 공격 시퀀스
    private SequenceNode chasingPlayer = new SequenceNode(); // 플레이어 쫓기 시퀀스

    private ActionNode currentAction = null; // 현재 진행중인 액션 저장용
    
    // 액션 여부 체크 노드
    private ActionNode currentActionNull; // 현재 액션 null 
    private ActionNode currentActionNullOrChasingPlayer; // 현재 액션 null 또는 플레이어 추적
    
    // 날기 패턴
    private ActionNode timerPassed; // 시간 체크
    private ActionNode flyBodySlam; // 날기 패턴 실행노드

    // 점프 돌진 패턴
    private ActionNode playerOutOfDetectDistance; // 탐지거리 밖인지 체크
    private ActionNode jumpBodySlam; // 점프돌진 패턴 실행노드

    // 꼬리공격 패턴
    private ActionNode playerInAttackDistance; // 공격사거리 내인지 체크
    private ActionNode tailAttacking; // 꼬리공격 패턴 실행노드
    
    // 쫓아가기 패턴
    private ActionNode chasePlayer;

    // 예외처리
    private ActionNode currentActionStillRunning;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float detectedDistance = 10f;
    [SerializeField] private float attackDistance = 8f;
    [SerializeField] private float jumpDistance = 9f;
    [SerializeField] private float patternCooldown = 25f;
    private float timer = 0f;
    private bool shouldLookAtPlayer = true;
    
    [SerializeField] private GameObject player;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool flyingRoarEnded = false;
    private bool takeOffFinished = false;
    private bool biteAttackFinished = false;
    private bool tailAttackFinished = false;
    private bool isLanding = false;
    
    [SerializeField] private int damageAmount = 20;
    
    // 액션노드 생성 헬퍼
    private ActionNode CreateAction(Func<INode.State> func) => new ActionNode(func);
    
    private void Reset()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        currentActionNull = CreateAction(IsCurrentActionNullAction);
        currentActionNullOrChasingPlayer = CreateAction(IsCurrentActionNullOrChasingPlayerAction);
    
        timerPassed = CreateAction(IsTimerPassedAction);
        flyBodySlam = CreateAction(FlyingAndBodySlamAction);
    
        playerOutOfDetectDistance = CreateAction(IsPlayerOutOfDetectDistanceAction);
        jumpBodySlam = CreateAction(JumpingAndBodySlamAction);
    
        playerInAttackDistance = CreateAction(IsPlayerInAttackDistanceAction);
        tailAttacking = CreateAction(TailAttackAction);
    
        chasePlayer = CreateAction(ChasingPlayerAction);
        currentActionStillRunning = CreateAction(CurrentActionStillRunning);
        
        flyingBodySlam.Add(timerPassed);
        flyingBodySlam.Add(currentActionNull);
        flyingBodySlam.Add(flyBodySlam);
        
        jumpingBodySlam.Add(playerOutOfDetectDistance);
        jumpingBodySlam.Add(currentActionNullOrChasingPlayer);
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
        timer += Time.deltaTime;
        
        // 상태 체크: 추적 중이거나 대기 상태일 때만 회전
        if (shouldLookAtPlayer)
        {
            LookAtPlayer();
        }

        rootNode.Evaluate();
    }


    private INode.State IsTimerPassedAction()
    {
        if (timer > patternCooldown)
        {
            return INode.State.SUCCESS;
        }
        else
        {
            return INode.State.FAILED;
        }
    }

    private INode.State IsCurrentActionNullAction()
    {
        if (currentAction == null)
        {
            return INode.State.SUCCESS;
        }
        else
        {
            return INode.State.FAILED;
        }
    }

    private INode.State FlyingAndBodySlamAction()
    {
        currentAction = flyBodySlam;
        StartCoroutine(FlyingAndBodySlamActionCoroutine());
            
        return INode.State.RUN;
    }

    private INode.State IsPlayerOutOfDetectDistanceAction()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > detectedDistance)
        {
            return INode.State.SUCCESS;
        }
        else
        {
            return INode.State.FAILED;
        }
    }

    private INode.State JumpingAndBodySlamAction()
    {
        currentAction = jumpBodySlam;
        StartCoroutine(JumpingAndBodySlamActionCoroutine());
        
        return INode.State.RUN;
    }

    private INode.State IsPlayerInAttackDistanceAction()
    {      
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            return INode.State.SUCCESS;
        }
        else
        {
            return INode.State.FAILED;
        }
    }

    private INode.State IsCurrentActionNullOrChasingPlayerAction()
    {
        if (currentAction == null || currentAction == chasePlayer)
        {
            return INode.State.SUCCESS;
        }
        else
        {
            return INode.State.FAILED;
        }
    }

    private INode.State TailAttackAction()
    {
        currentAction = tailAttacking;
        StartCoroutine(TailAttackActionCoroutine());
        
        return INode.State.RUN;
    }

    private INode.State ChasingPlayerAction()
    {
        currentAction = chasePlayer;
        StartCoroutine(ChasingPlayerActionCoroutine());
        
        return INode.State.RUN;
    }

    private INode.State CurrentActionStillRunning()
    {
        return INode.State.RUN;
    }

    private IEnumerator FlyingAndBodySlamActionCoroutine()
    {
        animator.SetBool("BiteAttack", false);
        
        shouldLookAtPlayer = true;
        agent.enabled = false;
        
        takeOffFinished = false;
        animator.SetBool("TakeOff", true);

        yield return new WaitUntil(() => takeOffFinished);

        animator.SetBool("Fly", true);
        animator.SetBool("TakeOff", false);
        
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 15f, 0);
        float riseDuration = 2.7f;
        float riseElapsed = 0f;

        // 위로 천천히 상승하면서 플레이어를 바라보기
        while (riseElapsed < riseDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, riseElapsed / riseDuration);
            riseElapsed += Time.deltaTime;
            
            yield return null;
        }

        transform.position = targetPos;

        animator.SetTrigger("FlyingRoar");

        flyingRoarEnded = false;
        yield return new WaitUntil(() => flyingRoarEnded);

        shouldLookAtPlayer = false;
        
        if (player != null)
        {
            // 박치기 방향 고정: 현재 바라보는 방향 기준 플레이어 위치 저장
            Vector3 slamStart = transform.position;
            Vector3 slamTarget = player.transform.position;

            Vector3 dirFromEnemyToPlayer = (player.transform.position - transform.position).normalized;
            Vector3 behindPlayerOffset = dirFromEnemyToPlayer * 8.0f;

            slamTarget += behindPlayerOffset;
            
            float slamDuration = 1f;
            float slamElapsed = 0f;

            // slamTarget을 바라보도록 회전 한번 맞춰놓기
            Vector3 slamDir = (slamTarget - slamStart).normalized;
            slamDir.y = 0;
            if (slamDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(slamDir);

            bool isFlyingRandingRunned = false;

            shouldLookAtPlayer = true;
            while (slamElapsed < slamDuration)
            {
                if (slamDuration - slamElapsed < 0.3f && isFlyingRandingRunned == false)
                {
                    animator.SetTrigger("FlyingRanding");
                    isFlyingRandingRunned = true;
                }
                transform.position = Vector3.Lerp(slamStart, slamTarget, slamElapsed / slamDuration);
                slamElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = slamTarget; // 보정
        }

        animator.SetBool("Fly", false);
        timer = 0f;
        currentAction = null;
    }

    
    private IEnumerator JumpingAndBodySlamActionCoroutine()
    {
        biteAttackFinished = false;
        animator.SetBool("BiteAttack", true);
        isLanding = false;
        animator.SetBool("CrawlForward", false);
        
        shouldLookAtPlayer = true;
        agent.enabled = true;
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize(); // 방향만 유지, 길이는 1로 만듬

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * jumpDistance;
        
        float duration = 0.4f; // 점프 시간
        float elapsed = 0f;        
        
        while (elapsed < duration)
        {
            if (biteAttackFinished && !isLanding)
            {
                Debug.Log("애니메이션 중단");
                animator.SetBool("BiteAttack", false);
                isLanding = true;
            }
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        
        shouldLookAtPlayer = true;
        currentAction = null;
    }

    private IEnumerator TailAttackActionCoroutine()
    {
        animator.SetBool("BiteAttack", false);
        animator.SetBool("CrawlForward", false);
        
        yield return new WaitForSeconds(0.4f);
        shouldLookAtPlayer = true;
        agent.enabled = true;
        agent.isStopped = true;
        
        agent.SetDestination(transform.position);
        
        tailAttackFinished = false;
        animator.SetBool("TailAttack", true);
        
        yield return new WaitUntil(() => tailAttackFinished);

        animator.SetBool("TailAttack", false);
        
        currentAction = null;
    }

    private IEnumerator ChasingPlayerActionCoroutine()
    {
        animator.SetBool("BiteAttack", false);
        shouldLookAtPlayer = true;
        agent.enabled = true;
        agent.isStopped = false;
        
        animator.SetBool("CrawlForward", true);

        agent.speed = 2f;
        agent.SetDestination(player.transform.position);
        
        yield return new WaitUntil(() => agent.remainingDistance < 0.3f);
        
        animator.SetBool("CrawlForward", false);
        
        currentAction = null;
    }
    
    public void OnTakeOffEnd()
    {
        takeOffFinished = true;
    }
    
    public void OnFlyingRoarEnd()
    {
        flyingRoarEnded = true;
    }

    public void BiteAttackEnd()
    {
        biteAttackFinished = true;
    }

    public void TailAttackEnd()
    {
        tailAttackFinished = true;
    }
    
    private void LookAtPlayer()
    {
        if (player == null) return;
    
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // 수평 회전만

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentAction == flyBodySlam || currentAction == jumpBodySlam || currentAction == tailAttacking)
        {
            if (other.CompareTag("Player"))
            {
                IDamagable damageable = other.gameObject.GetComponent<IDamagable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageAmount);
                    Service.Log("데미지입음");
                }
            }
        }
    }
}
