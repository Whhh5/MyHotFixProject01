using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIDialog_LevelSelect : MonoBehaviour
{
    [SerializeField] Button btn_nextLevel;
    void Start()
    {
        btn_nextLevel.onClick.AddListener(() => 
        {
            Addressables.DownloadDependenciesAsync("Scene_Battle_Classic").Completed += (handle) =>
            {
                if (handle.Status != AsyncOperationStatus.Failed)
                {
                    Addressables.LoadSceneAsync("Scene_Battle_Classic").Completed += (handle) =>
                    {
                        if (handle.Status != AsyncOperationStatus.Failed)
                        {
                            Debug.Log("加载战斗场景成功");
                        }
                    };
                }
            };
        });
    }
}
