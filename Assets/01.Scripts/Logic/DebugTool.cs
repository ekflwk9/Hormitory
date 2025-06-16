using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class DebugTool : EditorWindow
{
    [MenuItem("Window/DebugTool")]
    public static void ShowWindow()
    {
        GetWindow<DebugTool>("DebugTool");
    }

    private void OnGUI()
    {
        //GUILayout.Label("디버그 전용 패널");
        //GUILayout.Space(10f);
        // itemId = EditorGUILayout.IntField("획득할 아이템 아이디", itemId);

        GUILayout.Space(15f);
        if (GUILayout.Button("메뉴 On")) OnMenu();
        else if (GUILayout.Button("대화 On")) OnTalk();
        else if (GUILayout.Button("미션 On")) OnMission();
        else if (GUILayout.Button("총알 추가")) OnBullet(true);
        else if (GUILayout.Button("총알 감소")) OnBullet(false);
        else if (GUILayout.Button("피격 쉐이더")) HitView();
        else if (GUILayout.Button("죽었을때 윈도우")) DeadWindow();
        else if (GUILayout.Button("좌물쇠테스트")) MatchPuzzle();
        else if (GUILayout.Button("타이밍 맞추기")) Timing();
        else if (GUILayout.Button("아이템 획득")) ItemGet();
        else if (GUILayout.Button("인벤토리 On")) ShowInventory(true);
        else if (GUILayout.Button("인벤토리 Off")) ShowInventory(false);
    }

    private void OnMenu()
    {
        UiManager.Instance.Show<MenuUi>(true);
    }

    private void OnTalk()
    {
        UiManager.Instance.Get<TalkUi>().Popup("이런 빌어먹을 녀석 !!");
    }

    private void OnMission()
    {
        UiManager.Instance.Get<MissionUi>().Popup("목표 : 그레니 처치하기");
    }

    private void OnBullet(bool _isUp)
    {
        UiManager.Instance.Get<BulletUi>().BulletView(_isUp);
    }

    private void HitView()
    {
        UiManager.Instance.Get<HitUi>().HitView();
    }

    private void DeadWindow()
    {
        UiManager.Instance.Show<DeadUi>(true);
    }

    private void MatchPuzzle()
    {
        UiManager.Instance.Show<LockUi>(true);
        PuzzleManager.instance.GetPuzzle<CountMatchController>().SetRequiredNum(1364);
    }

    private void Timing()
    {
        PuzzleManager.instance.GetPuzzle<TimingMatch>().StartPuzzle();
    }

    // 원하는 아이템 번호를 자신이 가지고 있는지 확인하기
    private void ItemGet()
    {
        ItemManager.instance.Getitem(1); // ItemManager에 있는 아이템ID 1의 아이템이 있는지 없는지 확인 - 가져옴
    }

    private void ShowInventory(bool _isActive)
    {
        UiManager.Instance.Show<InventoryUi>(_isActive);
    }

    
}
#endif
