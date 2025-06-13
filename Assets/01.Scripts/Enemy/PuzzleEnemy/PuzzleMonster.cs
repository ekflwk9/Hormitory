using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMonster : MonoBehaviour
{
    [SerializeField] private MonsterAnimationData animationData;
    public MonsterAnimationData AnimationData => animationData;
    public Animator Animator { get; private set; }

    private MonsterStateMachine stateMachine;
    void Awake()
    {
        AnimationData.Initialize();
        
        Animator = GetComponentInChildren<Animator>();
        stateMachine = new MonsterStateMachine();
    }
}
