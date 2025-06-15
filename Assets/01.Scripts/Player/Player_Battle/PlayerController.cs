using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;

public class PlayerController : BasePlayerController, IDamagable
{
    [Header("Input KeyCodes")]
    [SerializeField]private KeyCode keyCodeReload = KeyCode.R;
    private Status status;
    private WeaponBase weapon;


    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float delay = 1f;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float camFOV;
    [SerializeField] private float bonusFOV = 15f;
    
    private bool isRolling;
    
    private void Awake()
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

    private void Start()
    {
        mainCamera = CameraManager.Instance.MainCamera;
        camFOV = mainCamera.fieldOfView;
    }
    private void Update()
    {
        base.Update();
        
        UpdateWeaponAction();
        
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!isRolling)
                StartCoroutine(Roll());
        }
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if (Input.GetMouseButton(1))
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
        bool isDie = status.DecreasHP(damage);
        
        if (isDie == true)
        {
            //GameOver
        }
    }


    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
    
    private IEnumerator Roll()
    {
        isRolling = true;
        float elapsed = 0f;
        
        mainCamera.fieldOfView = camFOV - bonusFOV;
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 direction = (moveInput.y * transform.forward) + (moveInput.x * transform.right).normalized;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            characterController.Move(direction * rollSpeed * Time.deltaTime);
            yield return null;
        }
        
        mainCamera.fieldOfView = camFOV;
        yield return new WaitForSeconds(delay);
        isRolling = false;
    }
}
