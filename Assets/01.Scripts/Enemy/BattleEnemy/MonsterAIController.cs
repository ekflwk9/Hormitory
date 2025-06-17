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
    
    // 날기 패턴
    private ActionNode timerPassed; // 시간 체크
    private ActionNode flyBodySlam; // 날기 패턴 실행노드

    // 점프 돌진 패턴
    private ActionNode jumpBodySlam; // 점프돌진 패턴 실행노드

    // 예외처리
    private ActionNode currentActionStillRunning;
    
    [Header("AI")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float patternCooldown = 5f;
    private float timer;
    private bool shouldLookAtPlayer = true;
    float overshootDistance = 10.0f; // 플레이어보다 얼마나 더 지나쳐서 착지할지
    [SerializeField] private GameObject player;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool flyingRoarEnded = false;
    private bool takeOffFinished = false;
    private bool biteAttackFinished = false;
    private bool tailAttackFinished = false;
    private bool roarFinished = false;
    
    [Header("Battle")]
    [SerializeField] private int damageAmount = 20;
    [SerializeField] MonsterStatController monsterStatController;
    
    // 액션노드 생성 헬퍼
    private ActionNode CreateAction(Func<INode.State> func) => new ActionNode(func);
    
    private void Reset()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        monsterStatController = GetComponent<MonsterStatController>();
    }

    private void Awake()
    {
        currentActionNull = CreateAction(IsCurrentActionNullAction);
    
        timerPassed = CreateAction(IsTimerPassedAction);
        flyBodySlam = CreateAction(FlyingAndBodySlamAction);
    
        jumpBodySlam = CreateAction(JumpingAndBodySlamAction);
        currentActionStillRunning = CreateAction(CurrentActionStillRunning);
        
        flyingBodySlam.Add(timerPassed);
        flyingBodySlam.Add(currentActionNull);
        flyingBodySlam.Add(flyBodySlam);
        
        jumpingBodySlam.Add(currentActionNull);
        jumpingBodySlam.Add(jumpBodySlam);
        
        rootNode.Add(flyingBodySlam);
        rootNode.Add(jumpingBodySlam);
        rootNode.Add(currentActionStillRunning);
    }

    private void Update()
    {
        if (!monsterStatController.isDead)
        {
            timer += Time.deltaTime;
        
            // 상태 체크: 추적 중이거나 대기 상태일 때만 회전
            if (shouldLookAtPlayer)
            {
                LookAtPlayer();
            }

            rootNode.Evaluate();
        }
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

    private INode.State JumpingAndBodySlamAction()
    {
        currentAction = jumpBodySlam;
        StartCoroutine(JumpingAndBodySlamActionCoroutine());
        
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
        shouldLookAtPlayer = true;
        agent.enabled = true;
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        
        roarFinished = false;
        animator.SetBool("Roar", true);
        
        yield return new WaitUntil(() => roarFinished);
        
        animator.SetBool("BiteAttack", true);
        animator.SetBool("Roar", false);
        animator.SetBool("CrawlForward", false);

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.transform.position + direction * overshootDistance;
        targetPosition.y = 0;

        // 잠깐 기다려서 애니메이션 전이되고 나서 길이 가져오게끔
        yield return null;

        float duration = 0.65f;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("BiteAttack"))
        {
            duration = stateInfo.length;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        animator.SetBool("BiteAttack", false);

        shouldLookAtPlayer = true;
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

    public void RoarEnd()
    {
        roarFinished = true;
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
        if (currentAction == flyBodySlam || currentAction == jumpBodySlam)
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
