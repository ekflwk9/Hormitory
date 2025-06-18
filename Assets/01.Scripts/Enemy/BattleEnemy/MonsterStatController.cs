using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatController : MonoBehaviour, IDamagable
{
    [field:SerializeField] public float MonsterHealth { get; private set; }
    public bool isDead = false;
    [SerializeField] private Animator animator;
    [SerializeField] private MonsterAIController monsterAIController;
    
    private void Reset()
    {
        MonsterHealth = 1000f;
        animator = GetComponent<Animator>();
        monsterAIController = GetComponent<MonsterAIController>();
    }

    public void TakeDamage(float damage)
    {
        MonsterHealth = Mathf.Max(MonsterHealth - damage, 0) ;
        if (MonsterHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        monsterAIController.currentAction = null;
    
        monsterAIController.sfxSource.Stop();
        monsterAIController.talkSource.Stop();
        SoundManager.PlaySfx(SoundCategory.Movement, "BattleMonsterDying");
        
        monsterAIController.AllAnimationStop();
        animator.SetBool(monsterAIController.animationData.DeathParameterHash, true);

        // y 위치가 0이 아니면 내려가는 코루틴 실행
        if (transform.position.y != 0f)
        {
            StartCoroutine(MoveToGroundCoroutine());
        }
    }
    
    private IEnumerator MoveToGroundCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, 0f, startPos.z);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (!isDead)
                yield break;

            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        yield return new WaitForSeconds(4.5f);
    }
}
