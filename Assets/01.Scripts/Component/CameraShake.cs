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
        // 카메라의 원래 위치와 회전값을 저장해 둡니다.
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    /// <summary>
    /// 지정된 시간과 강도로 카메라를 흔듭니다. (피격 효과용)
    /// </summary>
    /// <param name="duration">흔들림 지속 시간</param>
    /// <param name="magnitude">흔들림 강도</param>
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 랜덤한 위치로 카메라를 살짝 흔듦
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림이 끝나면 원래 위치로 복원
        transform.localPosition = originalPos;
    }


    /// <summary>
    /// 지정된 시간 동안 지정된 각도로 카메라를 기울입니다. (사망 연출용)
    /// </summary>
    /// <param name="duration">기울어지는 데 걸리는 시간</param>
    /// <param name="tiltAngle">최종적으로 기울어질 각도</param>
    public void StartDeathTilt(float duration, float tiltAngle)
    {
        StartCoroutine(DeathTiltCoroutine(duration, tiltAngle));
    }

    private IEnumerator DeathTiltCoroutine(float duration, float tiltAngle)
    {
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = originalRot * Quaternion.Euler(0, 0, tiltAngle); // 원래 회전값에 기울임을 추가
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Slerp를 사용해 부드럽게 회전
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRot; // 정확한 목표 각도로 설정
    }
}