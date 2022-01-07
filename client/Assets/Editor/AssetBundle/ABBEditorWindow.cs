using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ABBEditorWindow : EditorWindow
{
    private static ABBEditorWindow window;

    private string appVersion;
    private string resVersion;
    private bool addonPackage;
    public static void Open()
    {
        if (window == null)
        {
            Rect windowRect = new Rect(0f, 0f, 400f, 700f);
            window = (ABBEditorWindow) EditorWindow.GetWindowWithRect(typeof(ABBEditorWindow), windowRect, false, EditorStrings.AssetBundleBuild);
        }
        window.Init();
        window.Show();
        window.Focus();
    }

    private void Init()
    {
        appVersion = VersionManager.Instance.appVersion.GetVer();
        resVersion = VersionManager.Instance.resVersion.GetVer();
        addonPackage = false;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        // base version
        EditorGUILayout.LabelField($"{EditorStrings.AppVersion} : {VersionManager.Instance.appVersion}");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(EditorStrings.AppVersion_New);
        GUILayout.FlexibleSpace();
        appVersion = EditorGUILayout.TextField(appVersion);
        EditorGUILayout.EndHorizontal();
        // res version
        EditorGUILayout.LabelField($"{EditorStrings.ResVersion} : {VersionManager.Instance.resVersion}");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(EditorStrings.ResVersion_New);
        GUILayout.FlexibleSpace();
        resVersion = EditorGUILayout.TextField(resVersion);
        EditorGUILayout.EndHorizontal();
        // addon package ?
        addonPackage = EditorGUILayout.Toggle("增量包", addonPackage);

        if (GUILayout.Button("开始打包！"))
        {
            StartBuild();
        }
        
        EditorGUILayout.EndVertical();
    }

    private void StartBuild()
    {
        VersionManager.Instance.SaveVersion(appVersion, resVersion);
        if (!addonPackage)
        {
            ABBuilder.BuildAll();
        }
    }
}
