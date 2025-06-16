using UnityEngine;


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
    Shoot
}

public static class SoundManager
{
    private static AudioSource bgmPlayer;
    private static AudioSource sfxPlayer;
    private static bool isInitialized = false;

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

    /// <summary>
    /// 지정된 카테고리의 효과음을 재생합니다.
    /// </summary>
    /// <param name="category">재생할 사운드의 카테고리</param>
    /// <param name="name">오디오 파일 이름</param>
    public static void PlaySfx(SoundCategory category, string name)
    {
        Init();

        // BGM 카테고리가 실수로 들어오면 경고 메시지 출력
        if (category == SoundCategory.BGM)
        {
            Debug.LogWarning("BGM은 PlayBgm 메서드를 사용해주세요.");
            return;
        }

        // 열거형을 문자열로 변환하여 동적으로 경로 생성
        string path = $"Sounds/{category}/{name}";
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            Debug.LogWarning("SFX: " + path + " not found!");
            return;
        }

        sfxPlayer.PlayOneShot(clip);
    }

    /// <summary>
    /// 배경음을 재생합니다.
    /// </summary>
    /// <param name="name">BGM 폴더 아래의 파일 이름</param>
    public static void PlayBgm(string name)
    {
        Init();

        string path = $"Sounds/BGM/{name}";
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            Debug.LogWarning("BGM: " + path + " not found!");
            return;
        }

        // 현재 재생 중인 BGM과 다른 경우에만 새로 재생
        if (bgmPlayer.clip == clip) return;

        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }
}