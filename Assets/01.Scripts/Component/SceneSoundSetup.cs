// SceneSoundSetup.cs
using UnityEngine;

public class SceneSoundSetup : MonoBehaviour
{
    [Tooltip("재생할 BGM의 이름 (SoundManager에 등록된 이름)")]
    [SerializeField] private string bgmName;

    void Start()
    {
        // SoundManager에 BGM 재생을 요청
        if (!string.IsNullOrEmpty(bgmName))
        {
            SoundManager.Instance.PlayBgm(bgmName);
        }
    }
}