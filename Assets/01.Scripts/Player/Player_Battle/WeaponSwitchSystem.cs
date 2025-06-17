using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    // [SerializeField]
    // private PlayerHUD playerHUD;
    [SerializeField] private ItemSO pistol;
    [SerializeField] private ItemSO knife;
    
    [SerializeField]
    private WeaponBase[] weapons;

    private WeaponBase currentWeapon;
    private WeaponBase previousWeapon;

    private void Awake()
    {
        //무기 정보 출력을 위해 현재 소지중인 모든 무기 이벤트 등록
        // playerHUD.SetupAllWeapons(weapons);

        //현재 소지중인 모든 무기를 보이지 않게 설정
        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        //ItemManager.instance.RegisterItem(pistol,pistol.ItemID);
        //ItemManager.instance.RegisterItem(knife,knife.ItemID);
    }

    private void Start()
    {
        SwitchingWeapon(WeaponType.Main);

    }

    private void Update()
    {
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if(!Input.anyKeyDown) return;
        
        //에임 모드 시 무기 변경 불가
        if (currentWeapon.Animator.AimModeIs) return;


        int inputIndex = 0;
        if (int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex <= weapons.Length))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }

    }

    private void SwitchingWeapon(WeaponType weaponType)
    {
        // 교체 가능한 무기가 없으면 리턴
        if (weapons[(int)weaponType] == null) return;
        
        //현재 사용중인 무기가 있으면 이전 무기 정보에 저장
        if (currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }
        
        //무기 교체
        currentWeapon = weapons[(int)weaponType];

        //현재 사용중인 무기로 교체하려고 하면 리턴
        if (currentWeapon == previousWeapon)
        {
            return;
        }
        
        //무기를 사용하는 PlayerController, PlayerHUD에 현재 무기 정보 전달
        playerController.SwitchingWeapon(currentWeapon);
        //playerHUD.SwitchingWeapon(currentWeapon);

        if (previousWeapon != null)
        {
            previousWeapon.gameObject.SetActive(false);
        }
        currentWeapon.gameObject.SetActive(true);
        
        
        if (currentWeapon == weapons[0])
        {
            if (UiManager.Instance != null)
            {
                UiManager.Instance.Show<BulletUi>(true);
                UiManager.Instance.Get<InventoryUi>().SlotSelection(SlotType.FirstSlot);

            }
        }
        else if (currentWeapon == weapons[1])
        {
            if (UiManager.Instance != null)
            {
                UiManager.Instance.Show<BulletUi>(false);
                UiManager.Instance.Get<InventoryUi>().SlotSelection(SlotType.SecondSlot);

            }
        }
    }
}
