using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public static class A_Mgr_CommonAsset
{
    public enum AssetType
    {
        Asset_DefaultValue,
        Asset_CommonPath,
    }
    public static void GetAssets<T>(AssetType name, Action<T> callback)
    {
        var t_name = name.ToString();
        Addressables.LoadAssetAsync<T>(t_name)
            .Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback(asyncOperationHandle.Result);
                }
            };
    }
}
