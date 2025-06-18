using UnityEngine;
using System.Collections.Generic;

public enum SoundCategory
{
    Aiming, BGM, Casings_And_Shells, Explosions, Grenade_Throw,
    Gun_Reloads, Impacts, Misc, Movement, Shoot, Interaction
}

public static class SoundManager
{
    private static AudioSource bgmPlayer;
    private static List<AudioSource> sfxPlayerPool;
    private static int sfxPlayerPoolSize = 5;
    private static bool isInitialized = false;

    private static Dictionary<string, AudioClip> bgmCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, AudioClip> sfxCache = new Dictionary<string, AudioClip>();

    public static float sfxVolume = 1.0f; // 효과음 볼륨 저장

    private static void Init()
    {
        if (isInitialized) return;
        GameObject soundPlayerObject = new GameObject("SoundPlayer");
        Object.DontDestroyOnLoad(soundPlayerObject);
        bgmPlayer = soundPlayerObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        sfxPlayerPool = new List<AudioSource>();
        for (int i = 0; i < sfxPlayerPoolSize; i++)
        {
            sfxPlayerPool.Add(soundPlayerObject.AddComponent<AudioSource>());
        }
        isInitialized = true;
    }

    public  static AudioClip GetBgmClip(string name)
    {
        string path = $"Sounds/BGM/{name}";
        if (bgmCache.TryGetValue(path, out AudioClip clip)) return clip;
        clip = Resources.Load<AudioClip>(path);
        if (clip != null) bgmCache.Add(path, clip);
        else Debug.LogWarning("BGM: " + path + " not found!");
        return clip;
    }

    public static AudioClip GetSfxClip(SoundCategory category, string name)
    {
        string path = $"Sounds/{category}/{name}";
        if (sfxCache.TryGetValue(path, out AudioClip clip)) return clip;
        clip = Resources.Load<AudioClip>(path);
        if (clip != null) sfxCache.Add(path, clip);
        else Debug.LogWarning("SFX: " + path + " not found!");
        return clip;
    }

    private static AudioSource GetAvailableSfxPlayer()
    {
        foreach (var player in sfxPlayerPool)
        {
            if (!player.isPlaying) return player;
        }
        return sfxPlayerPool[0];
    }

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
            AudioSource player = GetAvailableSfxPlayer();
            player.PlayOneShot(clip, sfxVolume);
        }
    }

    public static void PlaySfxAtPoint(SoundCategory category, string name, Vector3 position)
    {
        Init();
        AudioClip clip = GetSfxClip(category, name);
        if (clip != null)
        {
            // 3D 사운드 재생 시에도 볼륨을 적용
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
        }
    }

    public static AudioSource PlaySfxControllable(SoundCategory category, string name)
    {
        Init();
        AudioClip clip = GetSfxClip(category, name);
        if (clip != null)
        {
            AudioSource player = GetAvailableSfxPlayer();
            player.clip = clip;
            player.loop = true;
            player.volume = sfxVolume;
            player.Play();
            return player;
        }
        return null;
    }

    public static void StopSfx(AudioSource playerToStop)
    {
        if (playerToStop != null && playerToStop.isPlaying)
        {
            playerToStop.Stop();
        }
    }

    public static void SetBgmVolume(float volume)
    {
        Init();
        bgmPlayer.volume = Mathf.Clamp01(volume);
    }

    public static void SetSfxVolume(float volume)
    {
        Init();
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var player in sfxPlayerPool)
        {
            player.volume = sfxVolume;
        }
    }
}