using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class MonsterAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string chaseParameterName = "Chase";
    [SerializeField] private string searchParameterName = "Search";
    [SerializeField] private string CaptureParameterName = "Capture";
    
    public int IdleParameterHash { get; private set;}
    public int WalkParameterHash { get; private set;}
    public int ChaseParameterHash { get; private set;}
    public int SearchParameterHash { get; private set;}
    public int CaptureParameterHash { get; private set;}

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        ChaseParameterHash = Animator.StringToHash(chaseParameterName);
        SearchParameterHash = Animator.StringToHash(searchParameterName);
        CaptureParameterHash = Animator.StringToHash(CaptureParameterName);
    }
}
