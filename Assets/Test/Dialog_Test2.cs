using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Test2 : A_UIDialog
{
    public override void OnAwake()
    {
        Debug.Log("UIDialog2");
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
