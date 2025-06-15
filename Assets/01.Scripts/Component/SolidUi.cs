using UnityEngine;
using _01.Scripts.Component;

public class SolidUi : MonoBehaviour
{
    [Header("고정 크기 값")]
    [SerializeField] private float scale;
    private Transform target;

    private void Start()
    {
        target = CameraManager.Instance.MainCamera.transform;
    }

    private void LateUpdate()
    {
        var distance = Vector3.Distance(transform.position, target.position);
        this.transform.localScale = Vector3.one * (distance * scale);

        this.transform.forward = target.transform.forward;
    }
}