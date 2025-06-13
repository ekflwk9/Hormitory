using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour, IDamagable
{
    [Header("Interaction Object")] [SerializeField]
    protected float maxHP = 100;
    protected float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }


    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }
}
