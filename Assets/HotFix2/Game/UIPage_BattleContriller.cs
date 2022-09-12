using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;

public class UIPage_BattleContriller : A_UIPage
{
    enum DialogType
    {
        Dialog_BattleContriller,
    }
    Dictionary<DialogType, object[]> dialog_param = null;
    public (Dictionary<string, Func<UniTask>> main, int) dislogs_Show_Parameters;
    protected override async UniTask OnAwake()
    {
        dialog_param = new Dictionary<DialogType, object[]>
        {
            { DialogType.Dialog_BattleContriller, null},
        };
        await base.OnAwake();
    }
    public override async UniTask InitializationAsync()
    {
        await ChangeDialogAsync(DialogType.Dialog_BattleContriller.ToString(), dislogs_Show_Parameters.main);
    }

    protected override async UniTask<Dictionary<string, object[]>> Init_Dialog_DicAsync()
    {
        Dictionary<string, object[]> ret = new Dictionary<string, object[]>(0);

        //var strs = Enum.GetValues(typeof(DialogType));

        foreach (var item in dialog_param)
        {
            ret.Add(item.Key.ToString(), item.Value);
        }

        return ret;
    }
}
