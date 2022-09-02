using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;

public class UIDialog_Lobby_Page : A_UIPage
{
    public enum DialogType
    {
        Dialog_Test1,
        Dialog_Test2,
        Dialog_Test3,
    }
    public override async UniTask InitializationAsync()
    {
        await AsyncDefault();
    }

    public override async UniTask<List<string>> GetDialogsType()
    {
        await AsyncDefault();
        List<string> ret = new List<string>();
        var fields = Enum.GetValues(typeof(DialogType));
        foreach (var item in fields)
        {
            ret.Add(item.ToString());
        }
        return ret;
    }
}
