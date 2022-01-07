using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript
{
    [MenuItem("Test/version")]
    public static void ChangeVersion()
    {
        PlayerSettings.bundleVersion = "3.3.3";
    }
}
