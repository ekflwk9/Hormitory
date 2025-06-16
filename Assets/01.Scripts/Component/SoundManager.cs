// SoundManager.cs (���׷��̵� ����)
using UnityEngine;
using System.Collections.Generic; // Dictionary�� ����ϱ� ���� �߰�

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("���� Ŭ�� ���")]
    [SerializeField] private Sound[] bgmSounds;
    [SerializeField] private Sound[] sfxSounds;

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    // ���� �ε��� Ŭ���� ������ ��ųʸ�
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

        // ����� �÷��̾� ����
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        sfxPlayer = gameObject.AddComponent<AudioSource>();

        // --- �ǵ�� 4��: ���� �ε� ���� ---
        // ��ųʸ� �ʱ�ȭ
        bgmClips = new Dictionary<string, AudioClip>();
        sfxClips = new Dictionary<string, AudioClip>();

        foreach (Sound sound in bgmSounds)
        {
            // �̹� ���� �̸��� Ű�� �ִ��� Ȯ��
            if (bgmClips.ContainsKey(sound.name))
            {
                Debug.LogWarning($"SoundManager: BGM ��Ͽ� �̹� '{sound.name}'�̶�� �̸��� Ű�� �����մϴ�.");
                continue; // �ߺ��� Ű�� �ǳʶ�
            }
            bgmClips.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in sfxSounds)
        {
            // �̹� ���� �̸��� Ű�� �ִ��� Ȯ��
            if (sfxClips.ContainsKey(sound.name))
            {
                Debug.LogWarning($"SoundManager: SFX ��Ͽ� �̹� '{sound.name}'�̶�� �̸��� Ű�� �����մϴ�.");
                continue; // �ߺ��� Ű�� �ǳʶ�
            }
            sfxClips.Add(sound.name, sound.clip);
        }
    }

    // --- �ǵ�� 1��: ����� ���� ��� ---
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

    // --- �ǵ�� 2��: ���� ���� ��� ---
    public void SetBgmVolume(float volume)
    {
        // ���� ���� 0�� 1 ���̷� ����
        bgmPlayer.volume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxPlayer.volume = Mathf.Clamp01(volume);
    }

    // --- �ǵ�� 3��: 3D ���� ���� ---
    /// <summary>
    /// 2D ȿ������ ����մϴ�. (UI Ŭ���� ��)
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
    /// ������ ��ġ���� 3D ȿ������ ����մϴ�. (�ѼҸ�, ������ ��)
    /// </summary>
    public void PlaySfxAtPoint(string name, Vector3 position)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            // ������ ��ġ�� ���带 ��� (3D ������ ���� ������ ������)
            AudioSource.PlayClipAtPoint(clip, position);
        }
        else
        {
            Debug.LogWarning("SFX: " + name + " not found!");
        }
    }
}