using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatController : MonoBehaviour, IDamagable
{
    public float MonsterHealth { get; private set; } = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        Service.Log($"공격받음{MonsterHealth}");
        MonsterHealth = Mathf.Max(MonsterHealth - damage, 0) ;
        if (MonsterHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Service.Log("사망");
        // 사망 로직
    }
}
