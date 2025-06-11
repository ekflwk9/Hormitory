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
        //itemId = EditorGUILayout.IntField("획득할 아이템 아이디", itemId);

        if (GUILayout.Button("어드레서블 로드")) Addressable();

        GUILayout.Space(15f);
        if (GUILayout.Button("메뉴 On")) OnMenu();
        if (GUILayout.Button("대화 On")) OnTalk();
        if (GUILayout.Button("미션 On")) OnMission();
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
}
#endif
