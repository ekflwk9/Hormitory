using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]private KeyCode keyCodeReload = KeyCode.R;
    private RotateToMouse rotateToMouse;
    private MovementCharacterController movement;
    private Status status;
    private PlayerAnimatorController animator;
    private WeaponAssaultRifle weapon;

    private void Awake()
    {
        //마우스 커서를 보이지 않게 설정하고, 현재위치에 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0) // 이동중일때 ( 걷기 or 뛰기)
        {
            bool isRun = false;
            
            isRun = Input.GetKey(keyCodeRun);
            
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
        }
        else
        {
            {
                movement.MoveSpeed = 0;
                animator.MoveSpeed = 0;
            }
        }
        
        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump(); 
        }
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
}
