using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public enum MonsterStateType
{
    Idle,
    Chase,
    Search,
    PuzzleWait,
    Capture
}
public class MonsterStateMachine : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _detectRange = 8f;
    [SerializeField] private float _searchDuration = 3f;

    private NavMeshAgent _navMeshAgent;
    private IState _currentState;
    private Dictionary<MonsterStateType, IState> _states;

    [SerializeField] private float _patrolRadius = 10f; //순찰 반경
    [SerializeField] private float _patrolWaitTime = 1.2f; // 목적지 도착 후 대기 시간
    
    [Header("SpeedSetting")]
    [SerializeField] private float _chaseSpeed = 2.5f;//추격 시 속도
    
    [SerializeField] private Camera _deadCam;
    private float _defaultSpeed;
    
    [SerializeField] float _minTalkInterval = 4f;
    [SerializeField] float _maxTalkInterval = 6f;
    private float _talkTimer;
    private float _nextTalkTime;
    private float _captureRange = 1.5f;
    [SerializeField]private  bool _isPuzzle = false;
    
    public PuzzleMonster PuzzleMonster { get; private set; }

    public Transform PlayerTransform => _playerTransform;
    public float DetectRange => _detectRange;
    public float SearchDuration => _searchDuration;
    public float ChaseSpeed => _chaseSpeed;
    public float DefaultSpeed => _defaultSpeed;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public float PatrolRadius => _patrolRadius;
    public float PatrolWaitTime => _patrolWaitTime;
    public float CaptureRange => _captureRange;
    public PuzzlePlayerController PuzzlePlayerController { get; private set; }
    public Camera DeadCam => _deadCam;
    public bool IsPuzzle => _isPuzzle;
    public static Action OnPuzzle;
    public static Action OffPuzzle;
    private void Awake()
    {
        OnPuzzle = PuzzleStart;
        OffPuzzle = PuzzleEnd;
        
        
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PuzzleMonster = GetComponent<PuzzleMonster>();

        if (_playerTransform != null)
        {
            PuzzlePlayerController = PlayerTransform.GetComponent<PuzzlePlayerController>();
        }
        
        
        _defaultSpeed = NavMeshAgent.speed;
        _states = new Dictionary<MonsterStateType, IState>
        {
            { MonsterStateType.Idle, new IdleState(this) },
            { MonsterStateType.Chase, new ChaseState(this) },
            { MonsterStateType.Search, new SearchState( this) },
            { MonsterStateType.PuzzleWait, new PuzzleWaitState(this) },
            { MonsterStateType.Capture, new CaptureState(this)}
        };
    }

    private void Start()
    {
        ResetTalkTimer();
        TransitionTo(MonsterStateType.Idle);
    }

    private void Update()
    {
        _currentState.Update();
        HandleRandomSound();
        
    }

    public void TransitionTo(MonsterStateType newStateType)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = _states[newStateType];
        _currentState.Enter();
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
    
    
    private readonly List<string> _puzzleMonsterTalk = new List<string>()
    {
        "너도 알게 될거야!", 
        "너 참 맛있어보인다",
        "내 생각에 이거 즐거운데",
        "하하 하하 하하 하 하하",
        "이제 널 잡았어 셰어",
        "그러지 마, 에단. 모습을 보여줘",
        "키스해줘",
        "네 마음을 축복해 - 결국 널 찾을 거란 걸 알잖아",
        "지금 뭐하는 거야?",
        "진실은 결국 드러난다!",
        "진정해. 진정해.",
        "이번엔 도망 못 가",
        //"죽여버릴 거야, 죽여버릴 거야, 죽여버릴 거야!!"
    };
            
    public void PlayRandomSound()
    {
        if (_puzzleMonsterTalk.Count == 0)
            return;
        
        int index = Random.Range(0, _puzzleMonsterTalk.Count);
        SoundManager.PlaySfx(SoundCategory.Movement, $"PuzzleMonster{index+1}");
        UiManager.Instance.Get<TalkUi>().Popup(_puzzleMonsterTalk[index]);
    }

    public void ResetTalkTimer()
    {
        _talkTimer = 0f;
        _nextTalkTime = Random.Range(_minTalkInterval, _maxTalkInterval);
    }

    public void PuzzleStart()
    {
        _isPuzzle = true;
    }

    public void PuzzleEnd()
    {
        _isPuzzle = false;
    }


}
