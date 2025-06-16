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
    /// ������ ī�װ��� ȿ������ ����մϴ�.
    /// </summary>
    /// <param name="category">����� ������ ī�װ�</param>
    /// <param name="name">����� ���� �̸�</param>
    public static void PlaySfx(SoundCategory category, string name)
    {
        Init();

        // BGM ī�װ��� �Ǽ��� ������ ��� �޽��� ���
        if (category == SoundCategory.BGM)
        {
            Debug.LogWarning("BGM�� PlayBgm �޼��带 ������ּ���.");
            return;
        }

        // �������� ���ڿ��� ��ȯ�Ͽ� �������� ��� ����
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
    /// ������� ����մϴ�.
    /// </summary>
    /// <param name="name">BGM ���� �Ʒ��� ���� �̸�</param>
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

        // ���� ��� ���� BGM�� �ٸ� ��쿡�� ���� ���
        if (bgmPlayer.clip == clip) return;

        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }
}