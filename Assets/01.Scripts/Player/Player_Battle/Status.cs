using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<float, float>{}

public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();
    
    [Header("Walk Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("HP")] [SerializeField] private int maxHP = 1000;
    private float currentHP;
    
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public float CurrentHP => currentHP;
    public int MaxHP => maxHP;


    private void Awake()
    {
        currentHP = maxHP;
    }

    public void DecreasHP(float damage)
    {
        float previousHP = currentHP;

        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);
    }
}
