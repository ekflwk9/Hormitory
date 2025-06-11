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
    }

    private void Addressable()
    {

    }
}
#endif
