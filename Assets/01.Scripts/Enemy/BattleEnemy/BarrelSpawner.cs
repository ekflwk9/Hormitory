using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelSpawner : MonoBehaviour
{
    public GameObject barrelPrefab;
    public int initialBarrelCount = 3;
    public float spawnHeight = 5f;
    public float dropDuration = 1.5f;
    public float barrelOffset = 0.5f;
    public LayerMask overlapMask;
        
    private List<Transform> wallTransforms = new List<Transform>();

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
        int maxTries = 10;
        int tries = 0;

        while (tries < maxTries)
        {
            Transform wall = wallTransforms[Random.Range(0, wallTransforms.Count)];

            // 드럼통이 안쪽으로 떨어지게 하려면 벽의 노멀 방향 반대쪽으로 약간 이동
            Vector3 inwardOffset = -wall.forward * 1f + wall.right * Random.Range(-2f, 2f);

            Vector3 spawnPos = wall.position + inwardOffset;
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
