using UnityEngine;
using UnityEngine.SceneManagement;
using BXB.Core;

public class GameStart : MonoBehaviour
{
    public async void Start()
    {
        Debug.Log("开始加载场景");
        await A_Mgr_Resource.Instance.LoadSceneAsync(SceneType.Scene_UI, LoadSceneMode.Single);


        return;
        //Addressables.DownloadDependenciesAsync("Scene_UI_Prefab").Completed += (asyncOperationsHandle) =>
        //{
        //    if (asyncOperationsHandle.Status != AsyncOperationStatus.Failed)
        //    {
        //        Debug.Log("下载包成功");
                
        //        Addressables.LoadSceneAsync("Scene_Ui", LoadSceneMode.Single).Completed += (handle) =>
        //        {
        //            if (handle.Status != AsyncOperationStatus.Failed)
        //            {
        //                Debug.Log("加载 ui 场景成功");
        //            }
        //            else
        //            {
        //                Debug.Log("加载 ui 场景失败");
        //            }
        //        };
        //    }
        //    else
        //    {
        //        Debug.Log("下载包失败");
        //    }
        //};
    }
}
