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

    public bool isShaking = false;
    void Awake()
    {
        if(PlayerManager.Instance != null)
            PlayerManager.Instance.SetCamera(this);

        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    public void Shake(float duration, float strength)
    {
        if (isShaking) return;

        isShaking = true;

        transform.DOShakePosition(duration, strength)
            .OnComplete(ResetPosition);
    }

    private void ResetPosition()
    {
        transform.localPosition = originPosition;
        isShaking = false;
    }

    public void Fall()
    {
        Sequence seq = DOTween.Sequence();

        // Step 1: 왼쪽으로 휘청이며 기울어짐 
        seq.Append(transform.DOLocalRotate(new Vector3(0f, 0f, 15f), 0.7f)
            .SetEase(Ease.OutSine));

        // Step 2: 점점 쓰러짐 
        seq.Append(transform.DOLocalRotate(new Vector3(0f, 0f, 80f), 0.6f)
            .SetEase(Ease.InCubic));

        // Step 3: 쓰러진 상태에서 떨림 + 카메라 떨어짐
        seq.Append(transform.DOShakeRotation(0.4f, 7f, 8, 90f)); 
        seq.Join(transform.DOLocalMove(originPosition + new Vector3(0f, -1f, 0f), 0.6f));
        
    }
}
