using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : BasePlayerController, IDamagable
{
    [Header("Input KeyCodes")]
    [SerializeField]private KeyCode keyCodeReload = KeyCode.R;
    private Status status;
    private WeaponBase weapon;


    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float delay = 5f;
    
    [FormerlySerializedAs("mainCamera")] [SerializeField] private Camera playerCamera;
    [SerializeField] private float camFOV;
    [SerializeField] private float bonusFOV = 15f;
    
    private bool isInvincibility;
    private bool isRolling; // 구르기 시 공격 제어
    private bool canRoll = true; //구르기 제어
    
    private float _talkTimer;
    private float _nextTalkTime;
    
    [SerializeField] float _minTalkInterval = 4f;
    [SerializeField] float _maxTalkInterval = 6f;
    
    private Coroutine rollCoroutine;
    protected override void Awake()
    {
        base.Awake();
        //마우스 커서를 보이지 않게 설정하고, 현재위치에 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        status = GetComponent<Status>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        playerCamera = Camera.main;
        camFOV = playerCamera.fieldOfView;
        
        ResetTalkTimer();
    }
    protected override void Update()
    {
        base.Update();
        
        UpdateWeaponAction();
        HandleRandomSound();
        
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(canRoll)
                rollCoroutine = StartCoroutine(Roll());
        }
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0) && !isRolling && isControl)
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if (Input.GetMouseButton(1) && !isRolling && isControl)
        {
            weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }

        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincibility) return;
        
        status.DecreasHP(damage);

        if (status.CurrentHP == 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
            rollCoroutine = null;
        }
        base.Die();
        //StartCoroutine("DeathEffect");
        PlayerManager.Instance.MainCamera.Fall();
        UiManager.Instance.Show<DeadUi>(true);
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
    
    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;
        float elapsed = 0f;
        isInvincibility = true;
        weapon.Animator.SetTrigger("isRoll");
        playerCamera.fieldOfView = camFOV - bonusFOV;
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 direction = (moveInput.y * transform.forward) + (moveInput.x * transform.right).normalized;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            characterController.Move(direction * rollSpeed * Time.deltaTime);
            yield return null;
        }
        isRolling = false;
        isInvincibility = false;
        playerCamera.fieldOfView = camFOV;
        yield return new WaitForSeconds(delay);
        canRoll = true;
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
    
    
    private readonly List<string> playerTalk = new List<string>()
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
        if (UiManager.Instance.Get<TalkUi>().onTalk) return;
        if (playerTalk.Count == 0)
            return;
        
        int index = Random.Range(0, playerTalk.Count);
        UiManager.Instance.Get<TalkUi>().Popup(playerTalk[index]);
    }
    public void ResetTalkTimer()
    {
        _talkTimer = 0f;
        _nextTalkTime = Random.Range(_minTalkInterval, _maxTalkInterval);
    }
}
