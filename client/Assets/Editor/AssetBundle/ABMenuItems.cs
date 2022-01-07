using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ABMenuItems
{
    [MenuItem(EditorStrings.OpenAssetBundleBuilderWindow, false)]
    public static void OpenABBWindow()
    {
        ABBEditorWindow.Open();
    }
}
