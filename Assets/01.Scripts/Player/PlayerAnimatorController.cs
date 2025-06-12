using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        get => animator.GetFloat("MovementSpeed");
        set => animator.SetFloat("MovementSpeed", value);
    }
    
    //OnReload 트리거 작동 및 애니메이션 재생
    public void OnReload()
    {
        animator.SetTrigger("OnReload");
    }
    
    public bool AimModeIs
    {
        get => animator.GetBool("isAimMode");
        set => animator.SetBool("isAimMode", value);
    }
    
    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    /// <summary>
    /// 매개변수로 받아온 애니메이션 name이 현재 재생중인지 그 결과 반환
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CuurrentAnimationsIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
