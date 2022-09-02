using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog : A_UIDialog
{
    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {

    }

    protected override async UniTask InitializationAsync(params object[] param)
    {
        await AsyncDefault();
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        await AsyncDefault();
    }

    protected override async UniTask HideAsync(params object[] param)
    {
        await AsyncDefault();
    }
}
