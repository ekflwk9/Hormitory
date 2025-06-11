using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int>{}

//무기가 활성화 될때 해당 무기의 탄 수 정보를 갱신한다.
//onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
//이벤트 클래스에 호출할 메소드 등록(외부 클래스)
//weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
//이벤트 클래스에 등록되는 메소드(이벤트 클래스의 Invoke()가 호출될 때 자동 호출)
//private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
// {
//     textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
// }
public class WeaponAssaultRifle : MonoBehaviour
{

    [HideInInspector] public AmmoEvent onAmmoEvent = new AmmoEvent();
    [Header("Fire Effects")] [SerializeField]
    private GameObject muzzleFlashEffect;       //총구 이펙트
    
    [Header("Spawn Points")]
    [SerializeField]private Transform casingSpawnPoint; //탄피 생성 위치

    [Header("Weapon Setting")] [SerializeField]
    private WeaponSetting weaponSetting;                //무기 설정

    private float lastAttackTime = 0;                   //마지막 발사 시간
    private bool isReload = false;                      //재장전 중인지 체크
    
    private PlayerAnimatorController animator; 
    private CasingMemoryPool casingMemoryPool;          //탄피 생성 관리
    

    //외부에서 필요한 정보를 열람하기 위한 프로퍼티
    public WeaponName WeaponName => weaponSetting.WeaponName;
    private void Awake()
    {
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
        //무기가 활성화될 때 해당 무기의 탄 수 정보 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo,weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        //재장전 중일 떄는 무기 액션을 할 수 없다
        if (isReload) return;
        //마우스 왼쪽 클릭( 공격 시작)
        if (type == 0)
        {
            
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        
    }
    public void StartReload()
    {
        //현재 장전 중이면 재장전 불가능
        if (isReload) return;
        
        //무기 액션 도중에 'R'키를 눌러 재장전을 시도하면 액션 종료 후 재장전
        StopWeaponAction();
        
        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        return null;
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            //뛸 때 공격 불가
            if (animator.MoveSpeed > 0.5f)
            {
                return;
            }
            
            lastAttackTime = Time.time;
            //탄 수가 없으면 공격 불가능
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            //animator.Play("Fire", -1, 0);
            //총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");
            //탄피 생성
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position,transform.right);
        }
    }

    private IEnumerator OnReload()
    {
        isReload = true;
        animator.OnReload();
        
        while (true)
        {
            //현재 애니메이션이 Movement이면 장전 애니메이션이 종료되었다는 뜻
            if (animator.CuurrentAnimationsIs("Movement"))
            {
                isReload = false;
                
                //현재 탄 수 최대, 바뀐 탄 수 정보 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo,weaponSetting.maxAmmo);
                
                yield break;
            }

            yield return null;
        }
    }
}
