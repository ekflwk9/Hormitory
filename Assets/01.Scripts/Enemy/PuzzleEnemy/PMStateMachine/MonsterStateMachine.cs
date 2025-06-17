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
    
    [SerializeField] float minTalkInterval = 4f;
    [SerializeField] float maxTalkInterval = 6f;
    private float talkTimer;
    private float nextTalkTime;
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
        talkTimer += Time.deltaTime;
        if (talkTimer >= nextTalkTime)
        {
            PlayRandomSound();
            ResetTalkTimer();
        }
    }
    
    
    private readonly List<string> _puzzleMonsterTalk = new List<string>()
    {
        "당신은 그것을 얻을 것입니다!", 
        "당신은 맛있어 보인다",
        "나는 이것을 즐길 것 같아요",
        "하하 하하 하하 하하",
        "나는 이제 당신을 얻었습니다 쉘",
        "그러지 마, 에단. 모습을 보여줘",
        "키스해줘",
        "네 마음을 축복해 - 결국 널 찾을 거란 걸 알잖아",
        "지금 뭐하는 거야?",
        "무덤이 진실을 밝힐 거야!",
        "진정해. 진정해.",
        "이번엔 도망 못 가",
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
        talkTimer = 0f;
        nextTalkTime = Random.Range(minTalkInterval, maxTalkInterval);
    }
}
