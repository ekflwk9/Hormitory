using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelSpawner : MonoBehaviour
{
    public static BarrelSpawner instance;
    
    public GameObject barrelPrefab;
    public int initialBarrelCount = 3;
    public float spawnHeight = 5f;
    public float dropDuration = 1.5f;
    public float barrelOffset = 0.5f;
    public LayerMask overlapMask;
    
    private List<Transform> wallTransforms = new List<Transform>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        FindWalls();
        SpawnInitialBarrels();
    }

    private void SpawnInitialBarrels()
    {
        for (int i = 0; i < initialBarrelCount; i++)
        {
            TrySpawnBarrelNearWall();
        }
    }

    private void FindWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            wallTransforms.Add(wall.transform);
        }
    }

    public void SpawnBarrelOnMonsterStun()
    {
        TrySpawnBarrelNearWall();
    }
    
    private void TrySpawnBarrelNearWall()
    {    
        if (wallTransforms == null || wallTransforms.Count == 0)
        {
            Debug.LogWarning("벽 정보 비어있음 → FindWalls() 재시도");
            FindWalls();
        }
        
        int maxTries = 10;
        int tries = 0;

        while (tries < maxTries)
        {
            Transform wallTransform = wallTransforms[Random.Range(0, wallTransforms.Count)];
            GameObject wall = wallTransform.gameObject;

            // 중심 좌표를 collider 기준으로 정확히 잡음
            BoxCollider col = wall.GetComponent<BoxCollider>();
            if (col == null)
            {
                Debug.LogWarning("벽 오브젝트에 BoxCollider가 없습니다.");
                tries++;
                continue;
            }

            Vector3 wallCenter = col.bounds.center;

            // 중심 기준으로 offset 방향 계산
            Vector3 inwardOffset = -wallTransform.forward * 1f + wallTransform.right * Random.Range(-2f, 2f);
            Vector3 spawnPos = wallCenter + inwardOffset;
            spawnPos.y = 0f;

            if (!Physics.CheckSphere(spawnPos, barrelOffset, overlapMask))
            {
                GameObject barrel = Instantiate(barrelPrefab);
                barrel.transform.position = new Vector3(spawnPos.x, spawnPos.y + spawnHeight, spawnPos.z);
                spawnPos.y = 0f;
                StartCoroutine(SmoothDrop(barrel, spawnPos));
                break;
            }

            tries++;
        }
    }

    
    IEnumerator SmoothDrop(GameObject barrel, Vector3 targetPosition)
    {
        float elapsed = 0f;
        Vector3 start = barrel.transform.position;

        while (elapsed < dropDuration)
        {
            barrel.transform.position = Vector3.Lerp(start, targetPosition, elapsed / dropDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        barrel.transform.position = targetPosition;
    }

}
