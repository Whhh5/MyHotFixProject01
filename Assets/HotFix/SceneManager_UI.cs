using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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




    }
}
