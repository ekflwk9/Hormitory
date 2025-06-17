using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterAIController : MonoBehaviour
{
    [Header("Nodes")]
    private SelectorNode rootNode = new SelectorNode(); // 루트노드 
    
    private SequenceNode flyingBodySlam = new SequenceNode(); // 날아오른 후 몸통박치기 시퀀스
    private SequenceNode jumpingBodySlam = new SequenceNode(); // 뛰어서 몸통박치기 시퀀스
    
    public ActionNode currentAction = null; // 현재 진행중인 액션 저장용
    
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
    [SerializeField] private float patternCooldown = 5f;
    private float timer;
    private bool shouldLookAtPlayer = true;
    float overshootDistance = 4.0f; // 플레이어보다 얼마나 더 지나쳐서 착지할지
    [SerializeField] private GameObject player;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool flyingRoarEnded = false;
    private bool takeOffFinished = false;
    private bool roarFinished = false;
    
    [Header("Battle")]
    [SerializeField] private int damageAmount = 20;
    [SerializeField] MonsterStatController monsterStatController;
    private bool groggyCoroutineRunned = false;
    private bool isPendingGroggy = false;
    private bool isGroggy = false;

    private float groggyDuration = 6.0f; // 그로기 지속 시간
    private Vector3[] rayPos =
    {
        new Vector3(0f, 0f, 0f),         // 중심
        new Vector3(-1f, 0f, 0f),     // 왼쪽
        new Vector3(1f, 0f, 0f)       // 오른쪽
    };
    private float rayDistance = 0.5f;
    
    [SerializeField] private BarrelSpawner barrelSpawner;
    
    
    [Header("Talk")]
    [SerializeField] private float _talkTimer;
    [SerializeField] private float _nextTalkTime;
    [SerializeField] private float _minTalkInterval = 5f;
    [SerializeField] private float _maxTalkInterval = 7f;
    private readonly List<string> _battleMonsterTalk = new List<string>()
    {
        "완전 쪼그맣군, 장난감처럼!", 
        "으흐흐흐, 예아",
        "난 죽을수가 없어. 하지만 넌 다르지",
        "엿이나 먹어!",
        "여기 오지 말았어야해 ,친구",
    };
    
    // 액션노드 생성 헬퍼
    private ActionNode CreateAction(Func<INode.State> func) => new ActionNode(func);
    
    private void Reset()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        barrelSpawner = GameObject.Find("BarrelSpawner").GetComponent<BarrelSpawner>();
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

    private void Start()
    {
        ResetTalkTimer();
    }

    private void Update()
    {
        if (!monsterStatController.isDead && !isGroggy)
        {
            HandleRandomSound();
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
        
        takeOffFinished = false;
        animator.SetBool("TakeOff", true);

        while (!takeOffFinished)
        {
            if (monsterStatController.isDead)
                yield break;  // 즉시 종료

            yield return null;  // 한 프레임 대기
        }

        animator.SetBool("Fly", true);
        animator.SetBool("TakeOff", false);
        
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 5f, 0);
        float riseDuration = 2.7f;
        float riseElapsed = 0f;

        // 위로 천천히 상승하면서 플레이어를 바라보기
        while (riseElapsed < riseDuration)
        {
            if (monsterStatController.isDead)
            {
                yield break;
            }
            
            transform.position = Vector3.Lerp(startPos, targetPos, riseElapsed / riseDuration);
            riseElapsed += Time.deltaTime;
            
            yield return null;
        }

        transform.position = targetPos;

        animator.SetTrigger("FlyingRoar");

        flyingRoarEnded = false;
        while (!flyingRoarEnded)
        {
            if (monsterStatController.isDead)
                yield break;  // 즉시 종료

            yield return null;  // 한 프레임 대기
        }
        
        groggyCoroutineRunned = false;
        shouldLookAtPlayer = false;
        
        if (player != null)
        {
            // 박치기 방향 고정: 현재 바라보는 방향 기준 플레이어 위치 저장
            Vector3 slamStart = transform.position;
            Vector3 slamTarget = player.transform.position;

            Vector3 dirFromEnemyToPlayer = (player.transform.position - transform.position).normalized;
            Vector3 behindPlayerOffset = dirFromEnemyToPlayer * 4.0f;

            slamTarget += behindPlayerOffset;
            slamTarget.y = 0f;
            
            float slamDuration = 1f;
            float slamElapsed = 0f;

            bool isFlyingRandingRunned = false;

            while (slamElapsed < slamDuration)
            {
                if (monsterStatController.isDead)
                    yield break;
                
                Vector3 curpos = transform.position + transform.up + transform.forward * 1.75f;
                
                foreach (Vector3 rayOrigin in rayPos)
                { 
                    Debug.DrawRay(curpos, transform.forward + rayOrigin, Color.red, 1.0f);

                    if (Physics.Raycast(curpos, transform.forward + rayOrigin, out RaycastHit hit, rayDistance))
                    {
                        if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("ExplosiveBarrel"))
                        {   
                            PlayerManager.Instance.MainCamera.Shake(1f,1.4f);
                            barrelSpawner.SpawnBarrelOnMonsterStun();
                            if (!groggyCoroutineRunned)
                            {
                                isPendingGroggy = true;
                            }
                        }
                    }
                }
                
                if (isPendingGroggy && !groggyCoroutineRunned)
                {
                    isPendingGroggy = false;
                    groggyCoroutineRunned = true;
                    StartCoroutine(GroggyCoroutine());
                    yield break;// 코루틴 중단
                }
                
                if (slamDuration - slamElapsed < 0.3f && isFlyingRandingRunned == false)
                {
                    animator.SetBool("FlyingLanding", true);
                    isFlyingRandingRunned = true;
                }
                
                transform.position = Vector3.Lerp(slamStart, slamTarget, slamElapsed / slamDuration);
                slamElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = slamTarget; // 보정
        }

        animator.SetBool("FlyingLanding", false);
        animator.SetBool("Fly", false);
        timer = 0f;
        currentAction = null;
    }

    
    private IEnumerator JumpingAndBodySlamActionCoroutine()
    {
        shouldLookAtPlayer = true;
        
        roarFinished = false;
        animator.SetBool("Roar", true);
        
        while (!roarFinished)
        {
            if (monsterStatController.isDead)
                yield break;  // 즉시 종료

            yield return null;  // 한 프레임 대기
        }
        
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
            if (monsterStatController.isDead)
                yield break;
            
            Vector3 curpos = transform.position + transform.up + transform.forward * 1.75f;
                
            foreach (Vector3 rayOrigin in rayPos)
            { 
                if (Physics.Raycast(curpos, transform.forward + rayOrigin, out RaycastHit hit, rayDistance))
                {
                    if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("ExplosiveBarrel"))
                    {
                        Vector3 temp = transform.position;
                        temp.y = 0f;
                        transform.position = temp;
                        
                        animator.SetBool("BiteAttack", false);
                        currentAction = null;
                        yield break;
                    }
                }
            }
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        
        PlayerManager.Instance.MainCamera.Shake(0.5f,1f);
        transform.position = targetPosition;
        animator.SetBool("BiteAttack", false);

        currentAction = null;
    }

    private IEnumerator GroggyCoroutine()
    {
        animator.ResetTrigger("StandUp");
        animator.SetBool("FlyingLanding", false);
        animator.SetBool("BiteAttack", false);
        animator.SetBool("TailAttack", false);
        animator.SetBool("CrawlForward", false);
        animator.SetBool("Fly", false);
        animator.SetBool("TakeOff", false);
        animator.SetBool("Roar", false);
        animator.SetBool("Groggy", false);
        
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, 0, startPos.z);
        float duration = 0.25f;
        float elapsed = 0f;
        
        isGroggy = true;
        currentAction = null; // 현재 액션 정지
        animator.SetBool("Groggy", true);
        shouldLookAtPlayer = false; // 시선도 고정

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        
        float waitTime = 0f;
        while (waitTime < groggyDuration)
        {
            if (monsterStatController.isDead)
                yield break; // 즉시 종료
        
            waitTime += Time.deltaTime;
            yield return null;
        };

        animator.SetBool("Groggy", false);
        isGroggy = false;
        timer = 0f; // 패턴 다시 선택하게끔 타이머 초기화
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
    
    private void HandleRandomSound()
    {
        _talkTimer += Time.deltaTime;
        if (_talkTimer >= _nextTalkTime)
        {
            PlayRandomSound();
            ResetTalkTimer();
        }
    }
    
    public void PlayRandomSound()
    {
        if (_battleMonsterTalk.Count == 0)
            return;
        
        int index = Random.Range(0, _battleMonsterTalk.Count);
        SoundManager.PlaySfx(SoundCategory.Movement, $"BattleMonster{index+1}");
        UiManager.Instance.Get<TalkUi>().Popup(_battleMonsterTalk[index]);
    }
    
    public void ResetTalkTimer()
    {
        _talkTimer = 0f;
        _nextTalkTime = Random.Range(_minTalkInterval, _maxTalkInterval);
    }

    public void RoarSoundStart()
    {
        SoundManager.PlaySfx(SoundCategory.Movement, $"BattleMonsterRoar");
    }

    public void LandingSoundStart()
    {
        // 
    }

    public void GroggySoundStart()
    {
        SoundManager.PlaySfx(SoundCategory.Movement, $"BattleMonsterGotGroggy");
    }
}