// SceneSoundSetup.cs
using UnityEngine;

public class SceneSoundSetup : MonoBehaviour
{
    [Tooltip("����� BGM�� �̸� (SoundManager�� ��ϵ� �̸�)")]
    [SerializeField] private string bgmName;

    void Start()
    {
        // SoundManager�� BGM ����� ��û
        if (!string.IsNullOrEmpty(bgmName))
        {
            SoundManager.Instance.PlayBgm(bgmName);
        }
    }
}