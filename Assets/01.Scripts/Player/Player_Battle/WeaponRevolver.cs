using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using _01.Scripts.Player.Player_Battle;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")] [SerializeField]
    private GameObject muzzleFlashEffect;       //총구 이펙트

    [Header("Spawn Ponts")] [SerializeField]
    private Transform bulletSpawnPoint;                 //총알 생성 위치
    [SerializeField]private Transform casingSpawnPoint; //탄피 생성 위치
    float spreadAmount => animator.AimModeIs ? 0f : 0.5f;
    [Header("Aim UI")] [SerializeField] private Image imageAim; // default/aim 모드에 따라 Aim 이미지 활성/비활성
    
    private bool isModeChange = false;                  //모드 전환 여부 체크
    private float defaultModeFOV = 60;                  //기본 모드에서 카메라 FOV
    private float aimModeFOV = 30;                      //AIM모드에서 카메라 FOV
    
    private CasingMemoryPool casingMemoryPool;          //탄피 생성 관리
    private ImpactMemoryPool impactMemoryPool;          //공격 효과 생성 후 활성/비활성화 관리
    private Camera mainCamera;           //광선 발사
    private Coroutine attackCoroutine;

    private void Awake()
    {
        base.Setup();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
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
        if(isReload)return;
        
        //모드 전환중이면 무기 액션 X
        if (isModeChange == true) return;
        //마우스 왼쪽 클릭( 공격 시작)
        if (type == 0)
        {
            //연속 공격
            if (weaponSetting.isAutomaticAttack == true)
            {
                isAttack = true;
                attackCoroutine = StartCoroutine(OnAttackLoop());
            }
            //단발 공격
            else
            {
                OnAttack();
            }
        }
        else
        {
            //공격 중일 때는 모드 전환 X
            if (isAttack == true) return;

            StartCoroutine(OnModeChange());
        }
    
    }

    public override void StopWeaponAction(int type = 0)
    {
        if (type == 0) 
        {
            isAttack = false;
            StopCoroutine(attackCoroutine);
        }
    }

    public override void StartReload()
    {
        //재장전 or 탄창 x => 장전 불가
        if( isReload == true || weaponSetting.currentMagazine <= 0) return;
        
        //무기 액션 도중 장전시도하면 무기 액션 종료 후 장전
        StopWeaponAction();
        
        StartCoroutine(OnReload());
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            yield return null;
        }
    }
    
    public void OnAttack()
    {
        if (isReload) return;
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if(animator.MoveSpeed > 0.5f) return;

            lastAttackTime = Time.time;
            
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            weaponSetting.currentAmmo--;
            UiManager.Instance.Get<BulletUi>().BulletView(false);
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            
            //animator.Play("Fire", -1, 0) --> 같은 애니메이션을 반복할 떄
            //애니메이션을 끊고 처음부터 다시 재생
            
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);
            
            //총구 이펙트 재생
            if(animator.AimModeIs == false)StartCoroutine(OnMuzzleFlashEffect());
            //탄피 생성
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position,transform.right);
            
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

        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            //현재 애니메이션이 Movement이면 장전 애니메이션이 종료되었다는 뜻
            if (animator.CurrentAnimationsIs("Movement"))
            {
                //현재 탄창 수 1 감소 , 바뀐 탄창 정보 업데이트
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);

                
                //현재 탄 수 최대, 바뀐 탄 수 정보 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo,weaponSetting.maxAmmo);
                
                
                UiManager.Instance.Get<BulletUi>().BulletView(true);
                isReload = false;
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
        
        // 탄 정확도 조절
        Vector2 spreadOffset = new Vector2(
            Random.Range(-spreadAmount, spreadAmount),
            Random.Range(-spreadAmount, spreadAmount)
        );
        //에임 모드 시 레이캐스트
        if (animator.AimModeIs)
        {
            //에임 모드 조준점과 일치한 방향으로 레이캐스트
            ray = mainCamera.ViewportPointToRay(new Vector2(0.515f, 0.575f));

            if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
            }
        }
        //에임 모드 아닐시 레이 캐스트
        else
        {
            //화면의 중앙 좌표 (Aim 기준으로 Raycast 연산)
            Ray spreadRay = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f) + spreadOffset);
            
            //공격 사거리 안에 부딪치는 오브젝트 있으면 targetPoint는 광선에 부딪친 위치
            if(Physics.Raycast(spreadRay, out hit, weaponSetting.attackDistance))
            {
                targetPoint = hit.point;
            }
            // 공격 사거리 안에 부딪치는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
            else
            {
                targetPoint = spreadRay.origin + spreadRay.direction * weaponSetting.attackDistance;
            }
            Debug.DrawRay(spreadRay.origin, spreadRay.direction * 5f, Color.red, 20f);
        }

        
        
        
        //첫번째 Raycast연산으로 얻어진 targetPoint를 목표지점으로 설정하고
        //총구를 시작지점으로 하여 Raycast 연산
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
            
            if (hit.transform.CompareTag("Enemy"))
            {
                IDamagable damageable = hit.transform.GetComponent<IDamagable>();
                damageable.TakeDamage(weaponSetting.damage);
            }
            else if (hit.transform.CompareTag("ExplosiveBarrel"))
            {
                hit.transform.GetComponent<ExplosionBarrel>().TakeDamageFromWeapon(weaponSetting.damage, WeaponType.Main);
            }
        }
        //Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue,2f);
        
    }
    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        animator.AimModeIs = !animator.AimModeIs;
        imageAim.enabled = !imageAim.enabled;
        
        float start = mainCamera.fieldOfView;
        float end = animator.AimModeIs == true ? aimModeFOV : defaultModeFOV;
        
        isModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            
            //모드에 따라 카메라 시야각 변경
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);
            
            yield return null;
        }
        isModeChange = false;
    }
    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }

    // private void OnDrawGizmos()
    // {
    //     if (!Application.isPlaying) return;
    //     
    //     Gizmos.color = Color.cyan;
    //     Ray ray;
    //     RaycastHit hit;
    //     Vector3 targetPoint = Vector3.zero;
    //     
    //     // 탄 정확도 조절
    //     Vector2 spreadOffset = new Vector2(
    //         Random.Range(-spreadAmount, spreadAmount),
    //         Random.Range(-spreadAmount, spreadAmount)
    //     );
    //     
    //         Ray spreadRay = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f) + spreadOffset);
    //         //공격 사거리 안에 부딪치는 오브젝트 있으면 targetPoint는 Ray에 부딪친 위치
    //         if(Physics.Raycast(spreadRay, out hit, weaponSetting.attackDistance))
    //         {
    //             targetPoint = hit.point;
    //         }
    //         // 공격 사거리 안에 부딪치는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
    //         else
    //         {
    //             targetPoint = spreadRay.origin + spreadRay.direction * weaponSetting.attackDistance;
    //         }
    //         Gizmos.DrawRay(spreadRay.origin, spreadRay.direction * 5f);
    // }
}
