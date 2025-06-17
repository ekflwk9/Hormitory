using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : InteractionObject, IDamagable
{
    [Header("Explosion Barrel")] [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField] private float explosionDelayTime = 0.3f;
    [SerializeField] private float explosionRadius = 10.0f;
    [SerializeField] private float explosionForce = 1000.0f;

    private bool isExplode = false;
    private bool isPrepared = false;
    public void TakeDamage(float damage)
    {
        if (isExplode) return;
        
    }

    public void TakeDamageFromWeapon(float damage, WeaponType weaponType)
    {
        if (isExplode) return;
        
        if (!isPrepared && weaponType == WeaponType.Melee)
        {
            if (currentHP > 0)
            {
                currentHP -= damage;
                currentHP = Mathf.Max(currentHP, 1.0f);
                Debug.Log($"{currentHP}");
            }
            if (currentHP == 1.0f)
            {
                Debug.Log("Prepared");
                isPrepared = true;
                
            }
        }
        
        else if (weaponType == WeaponType.Main)
        {
            if (isPrepared)
            {
                 currentHP -= damage;
                 if (currentHP <= 0)
                 {
                    StartCoroutine("ExplodeBarrel");
                 }
            }
        }
    }
    
    
    private IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        isExplode = true;

        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.center.y, bounds.center.z),
            transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            //폭발 범위에 부딪친 플레이어에게 데미지
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(50);
                continue;
            }

            //폭발 범위에 부딪친 몬스터에게 데미지
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                monster.monsterStatController.TakeDamage(100);
                continue;
            }

            //폭발 범위에 부딪친 오브젝트가 상호작용 오브젝트이면 데미지
            InteractionObject interactionObject = hit.GetComponent<InteractionObject>();
            {
                if (interactionObject != null)
                {
                    IDamagable damageable = hit.GetComponent<IDamagable>();
                    damageable.TakeDamage(100);
                }
            }
            //중력을 가지고 있는 오브젝트면 힘을 받아 밀려나도록
            //몬스터와 플레이어는 return이므로 영향 X
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // 폭발 드럼통 삭제
        Destroy(gameObject);
    }
}