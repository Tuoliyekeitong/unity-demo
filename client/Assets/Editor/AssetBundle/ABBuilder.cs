using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ABBuilder
{
    private static BuildTarget _buildTarget = BuildTarget.StandaloneOSX;
    private static Dictionary<string, AssetInfo> _assetInfos;
    public static void BuildAll()
    {
        _buildTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildAssetBundlesPreWork();
    }

    private static void BuildAssetBundlesPreWork()
    {
        string outputPath = Path.Combine(Application.dataPath, EditorStrings.ABOutputPath);
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        _assetInfos = new Dictionary<string, AssetInfo>();
        ClearABMark();
        ClearOutputPath();
        BuildAssetBundles();
    }

    private static void ClearStreamingAssets()
    {
        string abSApath = Path.Combine(Application.streamingAssetsPath, EditorStrings.ABOutputPath.Split('/')[1]);
        if (Directory.Exists(abSApath))
        {
            Directory.Delete(abSApath);
        }
        Directory.CreateDirectory(abSApath);
    }

    private static void ClearOutputPath()
    {
        string outputPath = Path.Combine(Application.dataPath, EditorStrings.ABOutputPath);
        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);
    }

    private static void BuildAssetBundles()
    {
        BuildAssetBundleOptions buildOptions = 
            BuildAssetBundleOptions.IgnoreTypeTreeChanges |
            BuildAssetBundleOptions.DeterministicAssetBundle |
            BuildAssetBundleOptions.None;
        CollectAllAsset();
        MarkAll();
        MoveABToStreamingAssets();
    }

    private static void ClearABMark()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string[] files = Directory.GetFiles(Path.Combine(Application.dataPath, EditorStrings.ABAssetPath), "*.*",
            SearchOption.AllDirectories);
        string[] dirs = Directory.GetDirectories(Path.Combine(Application.dataPath, EditorStrings.ABAssetPath), "*.*",
            SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if(assetImporter == null) continue;
            if (!assetImporter.assetBundleName.Equals("None"))
            {
                assetImporter.assetBundleName = null;
                AssetDatabase.RemoveAssetBundleName(path, true);
            }
        }
        for (int i = 0; i < dirs.Length; i++)
        {
            string path = dirs[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if(assetImporter == null) continue;
            if (!assetImporter.assetBundleName.Equals("None"))
            {
                assetImporter.assetBundleName = null;
                AssetDatabase.RemoveAssetBundleName(path, true);
            }
        }
        files = Directory.GetFiles(Path.Combine(Application.dataPath, EditorStrings.ABScenePath), "*.*",
            SearchOption.AllDirectories);
        dirs = Directory.GetDirectories(Path.Combine(Application.dataPath, EditorStrings.ABScenePath), "*.*",
            SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null) continue;
            if (!assetImporter.assetBundleName.Equals("None"))
            {
                assetImporter.assetBundleName = null;
                AssetDatabase.RemoveAssetBundleName(path, true);
            }
        }
        for (int i = 0; i < dirs.Length; i++)
        {
            string path = dirs[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null) continue;
            if (!assetImporter.assetBundleName.Equals("None"))
            {
                assetImporter.assetBundleName = null;
                AssetDatabase.RemoveAssetBundleName(path, true);
            }
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
    }

    private static void CollectAllAsset()
    {
        CollectScenes();
    }

    private static void MarkAll()
    {
        string outputSceneFolder = Path.Combine(Application.dataPath, EditorStrings.ABOutputPath,
            EditorStrings.ABScenePath);
        foreach (var assetInfo in _assetInfos)
        {
            if (assetInfo.Value.assetType == AssetType.scene)
            {
                string[] levels = {assetInfo.Value.path};
                //打包场景使用BuildPipeline.BuildPlayer进行，
                //第一个参数是存放要打包场景路径的字符串数组，
                //第二个参数是打包以后存放资源包的路径及资源包名称，可不带扩展名，若带了扩展名，下载此包时也要带上扩展名才能正确下载到此包，
                //第三个参数是目标平台，第四个参数是其它选项，在打包场景时此处必须选择BuildAdditionalStreamedScenes
                string outputSceneName = Path.Combine(outputSceneFolder, Path.GetFileName(assetInfo.Value.bundleName));
                BuildPipeline.BuildPlayer(levels, outputSceneName, _buildTarget,
                    BuildOptions.BuildAdditionalStreamedScenes);
            }
            else
            {
                AssetImporter importer = AssetImporter.GetAtPath(assetInfo.Value.path);
                importer.assetBundleName = assetInfo.Value.bundleName;
            }
        }
    }

    private static void CollectScenes()
    {
        string folder = Path.Combine(Application.dataPath, EditorStrings.ABScenePath);
        string[] files = Directory.GetFiles(folder, "*.unity", SearchOption.AllDirectories);
        BuildOptions buildOptions = BuildOptions.BuildAdditionalStreamedScenes;
        string outputPath = Path.Combine(Application.dataPath, EditorStrings.ABOutputPath, EditorStrings.ABScenePath);
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        int idx = 1;
        foreach (var file in files)
        {
            AssetInfo assetInfo = new AssetInfo();
            assetInfo.name = Path.GetFileName(file).Replace('.', '_').ToLower();
            assetInfo.path = file.Replace("\\", "/").Replace(Application.dataPath, "Assets/");
            EditorUtility.DisplayProgressBar("正在收集场景资源信息", assetInfo.path, (float) idx / (float) files.Length);
            assetInfo.assetType = AssetType.scene;
            assetInfo.bundleName = $"{assetInfo.name}.{BundleInfo.extension}";
            if(!_assetInfos.ContainsKey(assetInfo.path))
                _assetInfos.Add(assetInfo.path, assetInfo);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void CollectUI()
    {
        string uiFolderPath = Path.Combine(Application.dataPath, EditorStrings.ABAssetPath, EditorStrings.ABUIPath);
        string prefabsFolderPath = Path.Combine(uiFolderPath, "Prefabs");
        string[] prefabs = Directory.GetFiles(prefabsFolderPath, "*.prefab", SearchOption.AllDirectories);
    }

    private static void MoveABToStreamingAssets()
    {
        string outputPath = Path.Combine(Application.dataPath, EditorStrings.ABOutputPath);
        string abSApath = Path.Combine(Application.streamingAssetsPath, EditorStrings.ABOutputPath.Split('/')[1]);
        if (Directory.Exists(abSApath))
        {
            Directory.Delete(abSApath, true);
        }
        Directory.Move(outputPath, abSApath);
        AssetDatabase.Refresh();
    }
}
