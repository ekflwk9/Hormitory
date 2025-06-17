using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component; // CameraManager가 있는 네임스페이스
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(CameraManager.Instance != null)
        {
            // 수정 전:
            CameraManager.Instance.SetCamera(GetComponent<Camera>());
            // 수정 후:
            //CameraManager.Instance.SetCamera(this);
        }
    }
}
