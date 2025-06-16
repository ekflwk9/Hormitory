using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(CameraManager.Instance != null)
            CameraManager.Instance.SetCamera(GetComponent<Camera>());
    }

    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.B))
    //         StartCoroutine(DeathFallEffect());
    // }
    //
    // IEnumerator DeathFallEffect()
    // {
    //     float t = 0f;
    //     Quaternion startRot = transform.localRotation;
    //     Quaternion endRot = Quaternion.Euler(80, 0, 0); // 아래로 고개 떨어짐
    //
    //     while (t < 1f)
    //     {
    //         t += Time.deltaTime;
    //         transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
    //         yield return null;
    //     }
    // }
}
