using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using BXB.Core;
using Cysharp.Threading.Tasks;

public class UIDialog_LevelSelect : A_UIDialog
{
    [SerializeField] A_Component_Button btn_nextLevel;

    protected override async UniTask HideAsync(params object[] param)
    {
        await AsyncDefault();
    }

    protected override async UniTask InitializationAsync(params object[] param)
    {
        await btn_nextLevel.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                await A_Mgr_Resource.Instance.LoadSceneAsync(SceneType.Scene_Lobby, LoadSceneMode.Additive);
            }
        });
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        await AsyncDefault();
    }
}
