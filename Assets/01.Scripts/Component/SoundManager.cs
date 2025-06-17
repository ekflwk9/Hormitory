using UnityEngine;
using System.Collections.Generic; // Dictionary를 사용하기 위해 추가

public enum SoundCategory
{
    Aiming,
    BGM,
    Casings_And_Shells,
    Explosions,
    Grenade_Throw,
    Gun_Reloads,
    Impacts,
    Misc,
    Movement,
    Shoot,
    Interaction
}

public static class SoundManager
{
    private static AudioSource bgmPlayer;
    private static AudioSource sfxPlayer;
    private static bool isInitialized = false;
    public static float sfxVolume {get ; private set;}
    
    // --- 캐시를 위한 딕셔너리 추가 ---
    private static Dictionary<string, AudioClip> bgmCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, AudioClip> sfxCache = new Dictionary<string, AudioClip>();


    private static void Init()
    {
        if (isInitialized) return;

        GameObject soundPlayerObject = new GameObject("SoundPlayer");
        Object.DontDestroyOnLoad(soundPlayerObject);

        bgmPlayer = soundPlayerObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        sfxPlayer = soundPlayerObject.AddComponent<AudioSource>();

        isInitialized = true;
    }

    // --- BGM 클립을 가져오는 내부 함수 (캐싱 로직 포함) ---
    private static AudioClip GetBgmClip(string name)
    {
        string path = $"Sounds/BGM/{name}";

        // 1. 캐시에 클립이 있는지 먼저 확인
        if (bgmCache.TryGetValue(path, out AudioClip clip))
        {
            return clip; // 캐시에 있으면 바로 반환
        }
        // 2. 캐시에 없으면 Resources에서 로드
        else
        {
            clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                // 3. 로드에 성공하면 다음을 위해 캐시에 저장
                bgmCache.Add(path, clip);
            }
            else
            {
                Debug.LogWarning("BGM: " + path + " not found!");
            }
            return clip;
        }
    }

    // --- SFX 클립을 가져오는 내부 함수 (캐싱 로직 포함) ---
    public static AudioClip GetSfxClip(SoundCategory category, string name)
    {
        string path = $"Sounds/{category}/{name}";

        if (sfxCache.TryGetValue(path, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                sfxCache.Add(path, clip);
            }
            else
            {
                Debug.LogWarning("SFX: " + path + " not found!");
            }
            return clip;
        }
    }


    // --- 공개 API (호출하는 방식은 동일) ---

    public static void PlayBgm(string name)
    {
        Init();
        AudioClip clip = GetBgmClip(name);

        if (clip == null) return;
        if (bgmPlayer.clip == clip && bgmPlayer.isPlaying) return;

        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }

    public static void StopBgm()
    {
        Init();
        bgmPlayer.Stop();
    }

    public static void PlaySfx(SoundCategory category, string name)
    {
        Init();
        AudioClip clip = GetSfxClip(category, name);
        if (clip != null)
        {
            sfxPlayer.PlayOneShot(clip);
        }
    }

    //public static void PlaySfxAtPoint(SoundCategory category, string name, Vector3 position)
    //{
    //    Init();
    //    AudioClip clip = GetSfxClip(category, name);
    //    if (clip != null)
    //    {
    //        AudioSource.PlayClipAtPoint(clip, position);
    //    }
    //}

    public static void SetBgmVolume(float volume)
    {
        Init();
        bgmPlayer.volume = Mathf.Clamp01(volume);
    }

    public static void SetSfxVolume(float volume)
    {
        Init();
        sfxPlayer.volume = Mathf.Clamp01(volume);
        sfxVolume = sfxPlayer.volume;
    }
}