// CameraShake.cs
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPos;
    private Quaternion originalRot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ī�޶��� ���� ��ġ�� ȸ������ ������ �Ӵϴ�.
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    /// <summary>
    /// ������ �ð��� ������ ī�޶� ���ϴ�. (�ǰ� ȿ����)
    /// </summary>
    /// <param name="duration">��鸲 ���� �ð�</param>
    /// <param name="magnitude">��鸲 ����</param>
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // ������ ��ġ�� ī�޶� ��¦ ���
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ��鸲�� ������ ���� ��ġ�� ����
        transform.localPosition = originalPos;
    }


    /// <summary>
    /// ������ �ð� ���� ������ ������ ī�޶� ����Դϴ�. (��� �����)
    /// </summary>
    /// <param name="duration">�������� �� �ɸ��� �ð�</param>
    /// <param name="tiltAngle">���������� ������ ����</param>
    public void StartDeathTilt(float duration, float tiltAngle)
    {
        StartCoroutine(DeathTiltCoroutine(duration, tiltAngle));
    }

    private IEnumerator DeathTiltCoroutine(float duration, float tiltAngle)
    {
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = originalRot * Quaternion.Euler(0, 0, tiltAngle); // ���� ȸ������ ������� �߰�
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Slerp�� ����� �ε巴�� ȸ��
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRot; // ��Ȯ�� ��ǥ ������ ����
    }
}