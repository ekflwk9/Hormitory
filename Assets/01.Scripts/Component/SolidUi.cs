using UnityEngine;
using _01.Scripts.Component;

public class SolidUi : MonoBehaviour
{
    [SerializeField] private float scale = 0.5f;

    private Transform rotateTarget;
    private Transform target;

    private void Start()
    {
        rotateTarget = this.TryFindChild("Rotate").transform;
        target = PlayerManager.Instance.MainCamera.transform;
    }

    private void LateUpdate()
    {
        var thisPos = this.transform.position;
        var targetPos = target.position;
        var targetDistance = Vector3.Distance(thisPos, targetPos);

        rotateTarget.transform.position = (targetPos + thisPos) * 0.5f;
        rotateTarget.transform.forward = target.transform.forward;
        this.transform.localScale = Vector3.one * (targetDistance * scale);
    }
}