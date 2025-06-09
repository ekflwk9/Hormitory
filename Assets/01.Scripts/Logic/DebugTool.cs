using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugTool : EditorWindow
{
    [MenuItem("Window/DebugTool")]
    public static void ShowWindow()
    {
        GetWindow<DebugTool>("DebugTool");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("어드레서블 로드")) Addressable();
    }

    private void Addressable()
    {

    }
}
