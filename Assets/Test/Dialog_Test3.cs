using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;

public class Dialog_Test3 : A_UIDialog
{
    public override void OnAwake()
    {
        Debug.Log("UIDialog3");
    }

    public override void OnStart()
    {

    }

    protected override async UniTask HideAsync(params object[] param)
    {
        await AsyncDefault();
    }

    protected override async UniTask InitializationAsync(params object[] param)
    {
        await AsyncDefault();
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        await AsyncDefault();
    }
}
