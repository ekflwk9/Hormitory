using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGreanadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")] [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]private float explosionRadius = 10.0f;
    [SerializeField] private float explosionForce = 500.0f;
    [SerializeField] private float throwForce = 1000.0f;

    private int explosionDamage;
    private new Rigidbody rigidbody;

    public void Setup(int damage, Vector3 rotation)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rotation * throwForce);
        
        explosionDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        //폭발 범위에 있는 모든 오브젝트의 Collider 정보를 받아와 폭발 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if(hit != null && hit.GetComponent<IDamagable>() != null)
                hit.GetComponent<IDamagable>().TakeDamage(explosionDamage);
            
            // 중력을 가지고 있는 오브젝트면 밀려나도록
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        //수류탄 삭제
        Destroy(gameObject);
    }

}
