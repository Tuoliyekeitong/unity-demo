using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetType
{
    scene,
    effect,
    material,
    model,
    music,
    texture,
    shader,
    ui,
    json,
    font,
    lua,
}
public class AssetInfo
{
    public string name;
    public string path;
    public string bundleName;
    public AssetType assetType;
#if UNITY_EDITOR
    
#else
    public List<GameObject> gameObjects;
    public int count;
#endif
}
