using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.U2D;

public class UIPage_Lobby : A_UIPage
{
    public enum DialogType
    {
        UIDialog_LevelSelect,
    }
    Dictionary<DialogType, object[]> dialog_param = null;
    protected override async UniTask OnAwake()
    {
        dialog_param = new Dictionary<DialogType, object[]>
        {
            { DialogType.UIDialog_LevelSelect, null},
        };
        await base.OnAwake();
    }
    public override async UniTask InitializationAsync()
    {
        await AsyncDefault();
        await ChangeDialogAsync(DialogType.UIDialog_LevelSelect.ToString());
    }

    protected override async UniTask<Dictionary<string, object[]>> Init_Dialog_DicAsync()
    {
        await AsyncDefault();
        Dictionary<string, object[]> ret = new Dictionary<string, object[]>();
        //var fields = Enum.GetValues(typeof(DialogType));
        foreach (var item in dialog_param)
        {
            ret.Add(item.Key.ToString(), item.Value);
        }
        return ret;
    }
}
