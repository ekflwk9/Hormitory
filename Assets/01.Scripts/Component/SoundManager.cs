// SoundManager.cs (업그레이드 버전)
using UnityEngine;
using System.Collections.Generic; // Dictionary를 사용하기 위해 추가

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("사운드 클립 목록")]
    [SerializeField] private Sound[] bgmSounds;
    [SerializeField] private Sound[] sfxSounds;

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    // 사전 로딩된 클립을 저장할 딕셔너리
    private Dictionary<string, AudioClip> bgmClips;
    private Dictionary<string, AudioClip> sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 오디오 플레이어 설정
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        sfxPlayer = gameObject.AddComponent<AudioSource>();

        // --- 피드백 4번: 사전 로딩 구현 ---
        // 딕셔너리 초기화
        bgmClips = new Dictionary<string, AudioClip>();
        sfxClips = new Dictionary<string, AudioClip>();

        foreach (Sound sound in bgmSounds)
        {
            // 이미 같은 이름의 키가 있는지 확인
            if (bgmClips.ContainsKey(sound.name))
            {
                Debug.LogWarning($"SoundManager: BGM 목록에 이미 '{sound.name}'이라는 이름의 키가 존재합니다.");
                continue; // 중복된 키는 건너뜀
            }
            bgmClips.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in sfxSounds)
        {
            // 이미 같은 이름의 키가 있는지 확인
            if (sfxClips.ContainsKey(sound.name))
            {
                Debug.LogWarning($"SoundManager: SFX 목록에 이미 '{sound.name}'이라는 이름의 키가 존재합니다.");
                continue; // 중복된 키는 건너뜀
            }
            sfxClips.Add(sound.name, sound.clip);
        }
    }

    // --- 피드백 1번: 배경음 제어 기능 ---
    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void PauseBgm()
    {
        bgmPlayer.Pause();
    }

    public void UnpauseBgm()
    {
        bgmPlayer.UnPause();
    }

    public void PlayBgm(string name)
    {
        if (bgmClips.TryGetValue(name, out AudioClip clip))
        {
            if (bgmPlayer.clip == clip && bgmPlayer.isPlaying) return;
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
        else
        {
            Debug.LogWarning("BGM: " + name + " not found!");
        }
    }

    // --- 피드백 2번: 볼륨 조절 기능 ---
    public void SetBgmVolume(float volume)
    {
        // 볼륨 값은 0과 1 사이로 제한
        bgmPlayer.volume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxPlayer.volume = Mathf.Clamp01(volume);
    }

    // --- 피드백 3번: 3D 사운드 지원 ---
    /// <summary>
    /// 2D 효과음을 재생합니다. (UI 클릭음 등)
    /// </summary>
    public void PlaySfx(string name)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            sfxPlayer.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX: " + name + " not found!");
        }
    }

    /// <summary>
    /// 지정된 위치에서 3D 효과음을 재생합니다. (총소리, 폭발음 등)
    /// </summary>
    public void PlaySfxAtPoint(string name, Vector3 position)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            // 지정된 위치에 사운드를 재생 (3D 사운드의 가장 간단한 구현법)
            AudioSource.PlayClipAtPoint(clip, position);
        }
        else
        {
            Debug.LogWarning("SFX: " + name + " not found!");
        }
    }
}