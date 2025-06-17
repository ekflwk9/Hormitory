using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component; // CameraManager�� �ִ� ���ӽ����̽�
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(CameraManager.Instance != null)
        {
            // ���� ��:
            CameraManager.Instance.SetCamera(GetComponent<Camera>());
            // ���� ��:
            //CameraManager.Instance.SetCamera(this);
        }
    }
}
