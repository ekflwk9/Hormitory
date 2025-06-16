using UnityEngine;
using System.Collections.Generic; // Dictionary�� ����ϱ� ���� �߰�

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

    // --- ĳ�ø� ���� ��ųʸ� �߰� ---
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

    // --- BGM Ŭ���� �������� ���� �Լ� (ĳ�� ���� ����) ---
    private static AudioClip GetBgmClip(string name)
    {
        string path = $"Sounds/BGM/{name}";

        // 1. ĳ�ÿ� Ŭ���� �ִ��� ���� Ȯ��
        if (bgmCache.TryGetValue(path, out AudioClip clip))
        {
            return clip; // ĳ�ÿ� ������ �ٷ� ��ȯ
        }
        // 2. ĳ�ÿ� ������ Resources���� �ε�
        else
        {
            clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                // 3. �ε忡 �����ϸ� ������ ���� ĳ�ÿ� ����
                bgmCache.Add(path, clip);
            }
            else
            {
                Debug.LogWarning("BGM: " + path + " not found!");
            }
            return clip;
        }
    }

    // --- SFX Ŭ���� �������� ���� �Լ� (ĳ�� ���� ����) ---
    private static AudioClip GetSfxClip(SoundCategory category, string name)
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


    // --- ���� API (ȣ���ϴ� ����� ����) ---

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
    }
}