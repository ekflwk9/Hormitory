using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterAIController monsterAIController;
    public MonsterStatController monsterStatController;
    public BattleMonsterAnimationData AnimationData { get; private set; }
    
    void Reset()
    {
        monsterAIController = GetComponent<MonsterAIController>();
        monsterStatController = GetComponent<MonsterStatController>();
    }

    private void Awake()
    {
        AnimationData = new BattleMonsterAnimationData();
        AnimationData.Initialize();
    }
}
