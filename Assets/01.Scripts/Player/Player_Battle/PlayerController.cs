using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Input KeyCodes")]
    [SerializeField]private KeyCode keyCodeReload = KeyCode.R;
    private Status status;
    private WeaponBase weapon;


    private void Awake()
    {
        //마우스 커서를 보이지 않게 설정하고, 현재위치에 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        status = GetComponent<Status>();
    }

    private void Update()
    {
        UpdateWeaponAction();
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
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
}
