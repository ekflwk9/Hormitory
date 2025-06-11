using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Casing : MonoBehaviour
{
    [SerializeField] private float deactivateTime = 5f; //탄피 등장 후 비활성화 되는 시간
    [SerializeField] private float casingSpin = 1.0f;   // 탄피가 회전하는 속력 계수

    private Rigidbody rigidbody3D;
    private MemoryPool memoryPool;
    
    public void Setup(MemoryPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        memoryPool = pool;

        //탄피의 이동 속력과 회전 속력 설정
        rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                Random.Range(-casingSpin, casingSpin),
                                                Random.Range(-casingSpin, casingSpin));
        
        //탄피 자동 비활성화 위한 코루틴 실행
        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //사운드 재생
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        
        memoryPool.DeactivatePoolItem(gameObject);
    }
}
