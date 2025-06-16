// CameraShake.cs 파일 상단에 추가

using UnityEngine;
using System.Collections.Generic; // Dictionary 사용을 위해 추가
using System.Collections;

// 1. 카메라 쉐이크의 종류를 정의하는 열거형
public enum CameraShakeType
{
    PlayerHit,
    PlayerDeath,
    SpecialEvent,
    MonsterImpact
}

// 2. 각 쉐이크 타입에 대한 상세 설정을 담는 클래스
[System.Serializable]
public class ShakeProfile
{
    public CameraShakeType type;
    public float duration = 0.2f;
    public float magnitude = 0.1f;

    [Tooltip("체크하면 일반 흔들림 대신 기울이기 효과를 사용합니다 (사망 연출 등)")]
    public bool isTiltEffect = false;
    [Range(0, 90)]
    public float tiltAngle = 45f;
}

// CameraShake.cs (수정 버전)
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("흔들림 효과 프로필 목록")]
    [SerializeField] private ShakeProfile[] shakeProfiles;

    private Dictionary<CameraShakeType, ShakeProfile> shakeProfileDict;
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

        shakeProfileDict = new Dictionary<CameraShakeType, ShakeProfile>();
        foreach (var profile in shakeProfiles)
        {
            if (shakeProfileDict.ContainsKey(profile.type))
            {
                Debug.LogWarning($"CameraShake: 프로필 목록에 이미 '{profile.type}' 타입이 존재합니다.");
                continue;
            }
            shakeProfileDict.Add(profile.type, profile);
        }
    }

    private void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    public void Play(CameraShakeType type)
    {
        if (shakeProfileDict.TryGetValue(type, out ShakeProfile profile))
        {
            if (profile.isTiltEffect)
            {
                StartCoroutine(DeathTiltCoroutine(profile.duration, profile.tiltAngle));
            }
            else
            {
                StartCoroutine(ShakeCoroutine(profile.duration, profile.magnitude));
            }
        }
        else
        {
            Debug.LogWarning("요청한 카메라 쉐이크 타입을 찾을 수 없습니다: " + type);
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            // Time.deltaTime 대신 Time.unscaledDeltaTime 사용
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    private IEnumerator DeathTiltCoroutine(float duration, float tiltAngle)
    {
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = originalRot * Quaternion.Euler(0, 0, tiltAngle);
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);

            // Time.deltaTime 대신 Time.unscaledDeltaTime 사용
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localRotation = targetRot;
    }
}