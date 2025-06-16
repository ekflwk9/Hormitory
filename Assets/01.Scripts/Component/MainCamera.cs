using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using DG.Tweening;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;

    private bool isShaking = false;
    void Awake()
    {
        if(CameraManager.Instance != null)
            CameraManager.Instance.SetCamera(GetComponent<Camera>());

        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shake(1, 0.08f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fall(1, 80);
        }
    }

    public void Shake(float duration, float strength)
    {
        if (isShaking) return;

        isShaking = true;

        transform.DOShakePosition(duration, strength)
            .OnComplete(() =>
            {
                transform.localPosition = originPosition;
                isShaking = false;
            });
    }

    public void Fall(float duration, float angle)
    {
        transform.DOLocalRotate(new Vector3(angle, 0f, angle), duration)
            .SetEase(Ease.InOutCubic);
    }
}
