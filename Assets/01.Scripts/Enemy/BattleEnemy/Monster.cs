using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterAIController monsterAIController;
    public MonsterStatController monsterStatController;
    
    void Reset()
    {
        monsterAIController = GetComponent<MonsterAIController>();
        monsterStatController = GetComponent<MonsterStatController>();
    }

    void Update()
    {
        
    }
}
