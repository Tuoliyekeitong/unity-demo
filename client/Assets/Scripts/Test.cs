using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        StartCoroutine("LoadScene");
    }

    void Update()
    {
        
    }

    private IEnumerator LoadScene()
    {
        string url = $"file:///{Application.streamingAssetsPath}/AssetBundles/Scenes/samplescene_unity.ab";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle");
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        yield return ao;
        Scene scene = SceneManager.GetSceneByName("SampleScene");
        SceneManager.SetActiveScene(scene);
    }
}
