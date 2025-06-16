using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private float _detectRange = 5f;
    [SerializeField] private float _searchDuration = 3f;

    private NavMeshAgent _navMeshAgent;
    private IState _currentState;
    private Dictionary<MonsterStateType, IState> _states;

    [SerializeField] private float _patrolRadius = 10f; //순찰 반경
    [SerializeField] private float _patrolWaitTime = 2f; // 목적지 도착 후 대기 시간
    
    [Header("SpeedSetting")]
    [SerializeField] private float _chaseSpeed = 2.2f;//추격 시 속도
    
    [SerializeField] private Camera _deadCam;
    private float _defaultSpeed;

    
    private float _captureRange = 1.5f;
    
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
    
    
    private void Awake()
    {
        
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
            {MonsterStateType.Capture, new CaptureState(this)}
        };
    }

    private void Start()
    {
        TransitionTo(MonsterStateType.Idle);
    }

    private void Update()
    {
        _currentState.Update();
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
}
