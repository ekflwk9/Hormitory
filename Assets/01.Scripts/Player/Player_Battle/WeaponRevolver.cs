using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using _01.Scripts.Player.Player_Battle;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")] [SerializeField]
    private GameObject muzzleFlashEffect;       //총구 이펙트

    [Header("Spawn Ponts")] [SerializeField]
    private Transform bulletSpawnPoint;
    
    private ImpactMemoryPool impactMemoryPool;
    private Camera mainCamera;


    private void Awake()
    {
        base.Setup();

        impactMemoryPool = GetComponent<ImpactMemoryPool>();

        mainCamera = Camera.main;
        //탄 & 탄창 수 초기화
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
        
        //무기 활성화 될 때 무기 정보 갱신
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    // Start is called before the first frame update
    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && isAttack == false && isReload == false)
        {
            OnAttack();
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
    }

    public override void StartReload()
    {
        //재장전 or 탄창 x => 장전 불가
        if( isReload == true || weaponSetting.currentMagazine <= 0) return;
        
        //무기 액션 도중 장전시도하면 무기 액션 종료 후 장전
        StopWeaponAction();
        
        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if(animator.MoveSpeed > 0.5f) return;

            lastAttackTime = Time.time;
            
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            
            //animator.Play("Fire", -1, 0) --> 같은 애니메이션을 반복할 떄
            //애니메이션을 끊고 처음부터 다시 재생
            
            // string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            // animator.Play(animation, -1, 0);
            animator.Play("Fire", -1, 0);
            //총구 이펙트 재생
            if(animator.AimModeIs == false)StartCoroutine(OnMuzzleFlashEffect());
            //탄피 생성
            // casingMemoryPool.SpawnCasing(casingSpawnPoint.position,transform.right);
            
            //광선을 발사해 원하는 위치 공격 (+Imapct Effect)
            TwoStepRaycast();
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        
        muzzleFlashEffect.SetActive(false);
    }
    private IEnumerator OnReload()
    {
        isReload = true;
        animator.OnReload();
        
        while (true)
        {
            //현재 애니메이션이 Movement이면 장전 애니메이션이 종료되었다는 뜻
            if (animator.CurrentAnimationsIs("Movement"))
            {
                isReload = false;
                
                //현재 탄창 수 1 감소 , 바뀐 탄창 정보 업데이트
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);
                
                //현재 탄 수 최대, 바뀐 탄 수 정보 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo,weaponSetting.maxAmmo);
                
                yield break;
            }

            yield return null;
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;
        
        //화면의 중앙 좌표 (Aim 기준으로 Raycast 연산)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        //공격 사거리 안에 부딪치는 오브젝트 있으면 targetPoint는 광선에 부딪친 위치
        if(Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        // 공격 사거리 안에 부딪치는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red, 10f);
        
        //첫번째 Raycast연산으로 얻어진 targetPoint를 목표지점으로 설정하고
        //총구를 시작지점으로 하여 Raycast 연산
        Vector3 attackDirection = (targetPoint - transform.position).normalized;
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
            
            if (hit.transform.CompareTag("Enemy"))
            {
                IDamagable damageable = hit.transform.GetComponent<IDamagable>();
                damageable.TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
        
    }
    
    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }
}
