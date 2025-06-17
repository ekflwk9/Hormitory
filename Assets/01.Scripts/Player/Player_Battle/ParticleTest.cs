using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {
        Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity);
    }
}
