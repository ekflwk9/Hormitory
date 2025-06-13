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

    public Transform PlayerStransform => _playerTransform;
    public float DetectRange => _detectRange;
    public float SearchDuration => _searchDuration;
    public NavMeshAgent NavMeshAgent { get; private set; }
    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

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
