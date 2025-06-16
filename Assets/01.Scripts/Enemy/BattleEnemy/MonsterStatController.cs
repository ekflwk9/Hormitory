using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatController : MonoBehaviour, IDamagable
{
    public float MonsterHealth { get; private set; }
    public bool isDead = false;
    [SerializeField] private Animator animator;

    private void Reset()
    {
        MonsterHealth = 100f;
        animator = GetComponent<Animator>();
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
        animator.SetBool("BiteAttack", false);
        animator.SetBool("TailAttack", false);
        animator.SetBool("CrawlForward", false);
        animator.SetBool("Fly", false);
        animator.SetBool("TakeOff", false);
        
        isDead = true;
        
    }
}
