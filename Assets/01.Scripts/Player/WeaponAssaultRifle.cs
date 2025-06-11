using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    [Header("Fire Effects")] [SerializeField]
    private GameObject muzzleFlashEffect;       //총구 이펙트
    
    [Header("Spawn Points")]
    [SerializeField]private Transform casingSpawnPoint; //탄피 생성 위치

    [Header("Weapon Setting")] [SerializeField]
    private WeaponSetting weaponSetting;                //무기 설정

    private float lastAttackTime = 0;                   //마지막 발사 시간
    private PlayerAnimatorController animator; 
    private CasingMemoryPool casingMemoryPool;          //탄피 생성 관리

    private void Awake()
    {
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
    }

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    public void StartWeaponAction(int type = 0)
    {
        
    }

    public void StopWeaponAction(int type = 0)
    {
        
    }

    private IEnumerator OnAttackLoop()
    {
        return null;
    }

    void Start()
    {
        
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
        
            //animator.Play("Fire", -1, 0);
            //총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");
            //탄피 생성
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position,transform.right);
        }
    }
}
