using BXB.Core;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ComponentList))]
public abstract class Element3D : A_MonoAsync
{
    public ComponentList _componentList = null;
    public Transform _main = null;
    public sealed override async UniTask OnAwakeAsync()
    {
        await InitParams();
    }
    private async void Reset()
    {
        await InitParams();
        var obj = transform.Find("Main");
        if (obj == null)
        {
            var t = new GameObject("Main");
            t.transform.SetParent(transform);
            t.transform.localPosition = Vector3.zero;
            t.transform.localRotation = Quaternion.Euler(Vector3.zero);
            t.transform.localScale = Vector3.one;
            obj = t.transform;
        }
        _main = obj;
    }
    private async UniTask InitParams()
    {
        await AsyncDefault();
        _componentList = GetComponent<ComponentList>();
    }

    public sealed override async UniTask OnStartAsync()
    {
        await AsyncDefault();
    }
    public abstract UniTask Initializationasync(params object[] parameters);
    public abstract override UniTask OnShowAsync();
    public abstract UniTask PlayAsync(params object[] parameters);
    public async UniTask RecyleAsync()
    {
        await AsyncDefault();
        GameObject.Destroy(gameObject);
    }
}
