using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterStateType
{
    Idle,
    Chase,
    Search,
    PuzzleWait
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
    [SerializeField] private float _chaseSpeed = 2.2f;//c추격 시 속도
    private float _defaultSpeed;
    public PuzzleMonster PuzzleMonster { get; private set; }

    public Transform PlayerStransform => _playerTransform;
    public float DetectRange => _detectRange;
    public float SearchDuration => _searchDuration;
    public float ChaseSpeed => _chaseSpeed;
    public float DefaultSpeed => _defaultSpeed;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public float PatrolRadius => _patrolRadius;
    public float PatrolWaitTime => _patrolWaitTime;
    
    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PuzzleMonster = GetComponent<PuzzleMonster>();
        
        _defaultSpeed = NavMeshAgent.speed;
        
        _states = new Dictionary<MonsterStateType, IState>
        {
            { MonsterStateType.Idle, new IdleState(this) },
            { MonsterStateType.Chase, new ChaseState(this) },
            { MonsterStateType.Search, new SearchState( this) },
            { MonsterStateType.PuzzleWait, new PuzzleWaitState(this) }
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

    public void TransitionTo(MonsterStateType newStetType)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = _states[newStetType];
        _currentState.Enter();
    }

    public void OnPlayerHidden() => TransitionTo(MonsterStateType.Search);
    public void OnPlayerFound() => TransitionTo(MonsterStateType.Chase);
    public void OnPuzzleStart() => TransitionTo(MonsterStateType.PuzzleWait);
    public void OnPuzzleComplete() => TransitionTo(MonsterStateType.Chase);
}
