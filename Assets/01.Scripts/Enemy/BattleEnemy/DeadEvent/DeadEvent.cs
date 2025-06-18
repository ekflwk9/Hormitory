using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadEvent : MonoBehaviour
{
    private void Dead()
    {
        SoundManager.StopBgm();
    }

    private void StartFade()
    {
        UiManager.Instance.Get<FadeUi>().OnFade(Endfade, 0.25f);

        UiManager.Instance.Get<BulletUi>().gameObject.SetActive(false);
        UiManager.Instance.Get<HitUi>().gameObject.SetActive(false);
        UiManager.Instance.Get<InventoryUi>().gameObject.SetActive(false);
    }

    private void Endfade()
    {
        SceneManager.LoadScene("Ending");
        UiManager.Instance.Get<FadeUi>().OnFade(0.25f);
    }
}
