using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUi : MonoBehaviour
{
    private void Start()
    {
        //SoundManager.PlayBgm("");
        //SoundManager.PlayBgm("");
    }

    private void StartFade()
    {
        UiManager.Instance.Get<FadeUi>().OnFade(EndAction);
    }

    private void EndAction()
    {
        SceneManager.LoadScene("Intro");
        UiManager.Instance.Get<FadeUi>().OnFade();
    }
}
