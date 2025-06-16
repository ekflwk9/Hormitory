using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyerByTime : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        
    }
    
    void Update()
    {
        if (particle.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
