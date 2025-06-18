using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUi : MonoBehaviour
{
    private AudioSource source;

    private void Start()
    {
        source = this.GetComponent<AudioSource>();
        if (source != null) DestroyImmediate(source);

        source = this.AddComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = true;
        source.volume = SoundManager.sfxVolume;

        SoundManager.PlayBgm("music_box_Sound");
        source.clip = SoundManager.GetSfxClip(SoundCategory.BGM, "Girl_singing_Sound");
        source.Play();
    }

    private void StartFade()
    {
        UiManager.Instance.Get<FadeUi>().OnFade(EndAction, 0.5f);
    }

    private void EndAction()
    {
        SceneManager.LoadScene("Intro");
        UiManager.Instance.DestrotyManager();
    }
}
