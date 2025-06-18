using UnityEngine;

public class StartBattle : MonoBehaviour
{
    private void Start()
    {
        SoundManager.PlayBgm("BattleBGM");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UiManager.Instance.Get<MissionUi>().Popup("목표 : 괴물로 변한 잭 처치하기");
            this.gameObject.SetActive(false);
        }
    }
}
