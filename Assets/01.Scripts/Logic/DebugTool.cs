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
        
        if (GUILayout.Button("어드레서블 로드")) Addressable();

        GUILayout.Space(15f);
        if (GUILayout.Button("메뉴 On")) OnMenu();
        else if (GUILayout.Button("대화 On")) OnTalk();
        else if (GUILayout.Button("미션 On")) OnMission();
        else if (GUILayout.Button("총알 추가")) OnBullet(true);
        else if (GUILayout.Button("총알 감소")) OnBullet(false);
        else if (GUILayout.Button("피격 쉐이더")) HitView();
        else if (GUILayout.Button("좌물쇠테스트")) MatchPuzzle();
        else if (GUILayout.Button("타이밍 맞추기")) Timing();
        else if (GUILayout.Button("아이템 획득")) ItemGet();

    }

    private void Addressable()
    {

    }


    //
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

    private void MatchPuzzle()
    {
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

}
#endif
