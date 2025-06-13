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

    [Header("HP")] [SerializeField] private int maxHP = 100;
    private float currentHP;
    
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public float CurrentHP => currentHP;
    public int MaxHP => maxHP;


    private void Awake()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool DecreasHP(float damage)
    {
        float previousHP = currentHP;
        
        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;
        
        onHPEvent.Invoke(previousHP, currentHP);
        
        if (currentHP == 0)
        {
            return true;
        }

        return false;
    }
}
