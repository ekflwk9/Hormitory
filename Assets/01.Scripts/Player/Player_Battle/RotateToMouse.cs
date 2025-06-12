using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    
    [SerializeField] private float rotCamXAxisSpeed = 5f; //카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamXAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
        
        eulerAngleX = Mathf.Clamp(eulerAngleX, limitMinX, limitMaxX);
        
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        
        return Mathf.Clamp(angle, min, max);
    }
}

