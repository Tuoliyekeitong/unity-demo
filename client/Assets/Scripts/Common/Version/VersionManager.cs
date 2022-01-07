using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEditor;

public class VersionManager : Singleton<VersionManager>
{
    private const string versionFilePath = "version.txt";

    public Version appVersion;
    public Version resVersion;
    
    public VersionManager()
    {
        string filePath;
#if UNITY_EDITOR
        filePath = $"{Application.streamingAssetsPath}/{versionFilePath}";
#else
        filePath = $"{Application.persistentDataPath}/{versionFilePath}";
#endif
        if (!File.Exists(filePath))
        {
            Debug.LogError("没有找到版本信息！");
            appVersion = new Version("0.0.0.0");
            resVersion = new Version("0.0.0.0");
            return;
        }
        string txt = File.ReadAllText(filePath);
        JsonData jsonData = JsonMapper.ToObject(txt);
        if (jsonData.HasKey("AppVersion"))
        {
            appVersion = new Version(jsonData["AppVersion"].ToString());
        }
        else
        {
            appVersion = new Version("0.0.0.0");
        }
        if (jsonData.HasKey("ResVersion"))
        {
            resVersion = new Version(jsonData["ResVersion"].ToString());
        }
        else
        {
            resVersion = new Version("0.0.0.0");
        }
    }

    public void SaveVersion(string appVersion, string resVersion)
    {
        this.appVersion = new Version(appVersion);
        this.resVersion = new Version(resVersion);
        SaveFile();
    }

    public void SaveFile()
    {
        JsonData jsonData = new JsonData();
        jsonData["AppVersion"] = appVersion.ToString();
        jsonData["ResVersion"] = resVersion.ToString();
        string filePath;
#if UNITY_EDITOR
        filePath = $"{Application.streamingAssetsPath}/{versionFilePath}";
#else
        filePath = $"{Application.persistentDataPath}/{versionFilePath}";
#endif
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        File.WriteAllText(filePath, jsonData.ToJson());
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}

public class Version
{
    public int ver_1; // 大版本 从1开始
    public int fstVer{ get { return ver_1; }}
    public int ver_2; // 大版本第几个包 从0开始
    public int secVer{ get { return ver_2; }}
    public int ver_3; // 小版本 热更 从0开始
    public int thrVer{ get { return ver_3; }}
    public int ver_4; // 每次打包自增的版本号 热更 从1开始
    public int furVer{ get { return ver_4; }}

    public Version(string versionText)
    {
        string[] vers = versionText.Split('.');
        if (vers.Length == 4)
        {
            ver_1 = Int32.Parse(vers[0]);
            ver_2 = Int32.Parse(vers[1]);
            ver_3 = Int32.Parse(vers[2]);
            ver_4 = Int32.Parse(vers[3]);
        }
        else if (vers.Length == 3)
        {
            ver_1 = Int32.Parse(vers[0]);
            ver_2 = Int32.Parse(vers[1]);
            ver_3 = Int32.Parse(vers[2]);
            ver_4 = 1;
        }
        else
        {
            // TODO
            Debug.LogError("版本号字符串无效！");
            return;;
        }
    }

    public string GetVer()
    {
        return $"{ver_1}.{ver_2}.{ver_3}";
    }
    
    public override string ToString()
    {
        return $"{ver_1}.{ver_2}.{ver_3}.{ver_4}";
    }
}
