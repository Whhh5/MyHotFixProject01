using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneManager_UI : MonoBehaviour
{
    public RectTransform app1;
    public RectTransform app2;
    void Start()
    {
        Debug.Log("Scene manager ui loaded");

        Addressables.LoadAssetAsync<GameObject>("UIDialog_LevelSelect").Completed += (handle) =>
        {
            if (handle.Status != AsyncOperationStatus.Failed)
            {
                var obj = Instantiate(handle.Result, app1);
                var rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition3D = Vector3.zero;
                rect.rotation = Quaternion.Euler(0, 0, 0);
                rect.localScale = Vector3.one;

            }
            
        };
        //Slider slider = null;
        //Uri uir = new Uri("https://wangjh-hn1-sz.oss-cn-shenzhen.aliyuncs.com/StandaloneWindows64");
        //StartCoroutine(DownloadVideoFile01(uir, "", slider));

    }

    public IEnumerator DownloadVideoFile01(Uri uri, string downloadFileName, Slider sliderProgress)
    {
        using (UnityWebRequest downloader = UnityWebRequest.Get(uri))
        {
            downloader.downloadHandler = new DownloadHandlerFile(downloadFileName);

            print("开始下载");
            downloader.SendWebRequest();
            print("同步进度条");
            while (!downloader.isDone)
            {
                //print(downloader.downloadProgress);
                //sliderProgress.value = downloader.downloadProgress;
                //sliderProgress.GetComponentInChildren<Text>().text = (downloader.downloadProgress * 100).ToString("F2") + "%";
                Debug.Log(downloader.downloadProgress);
                yield return null;
            }

            if (downloader.error != null)
            {
                Debug.LogError(downloader.error);
            }
            else
            {
                print("下载结束");
                //sliderProgress.value = 1f;
                //sliderProgress.GetComponentInChildren<Text>().text = 100.ToString("F2") + "%";
            }
            var result = downloader.result;
            Debug.Log(result);
        }
    }
    public IEnumerator DownloadFile(string url, string contentName)
    {
        string downloadFileName = "";
#if UNITY_EDITOR
        downloadFileName = Path.Combine(Application.dataPath, contentName);
#elif UNITY_ANDROID
        downloadFileName = Path.Combine(Application.persistentDataPath, contentName);
#endif
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                DownloadHandler fileHandler = webRequest.downloadHandler;
                using (MemoryStream memory = new MemoryStream(fileHandler.data))
                {
                    byte[] buffer = new byte[1024 * 1024];
                    FileStream file = File.Open(downloadFileName, FileMode.OpenOrCreate);
                    int readBytes;
                    while ((readBytes = memory.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, readBytes);
                    }
                    file.Close();
                }
            }
        }
    }
}
