using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("开始加载场景");
        Addressables.DownloadDependenciesAsync("Scene_UI_Prefab").Completed += (asyncOperationsHandle) =>
        {
            if (asyncOperationsHandle.Status != AsyncOperationStatus.Failed)
            {
                Debug.Log("下载包成功");
                Addressables.LoadSceneAsync("Scene_Ui", LoadSceneMode.Single).Completed += (handle) =>
                {
                    if (handle.Status != AsyncOperationStatus.Failed)
                    {
                        Debug.Log("加载 ui 场景成功");
                    }
                    else
                    {
                        Debug.Log("加载 ui 场景失败");
                    }
                };
            }
            else
            {
                Debug.Log("下载包失败");
            }
        };
    }
}
