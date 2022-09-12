using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPage_Default : A_UIPage
{
    public enum DialogType
    {

    }
    Dictionary<DialogType, object[]> dialog_param = null;
    protected override async UniTask OnAwake()
    {
        dialog_param = new Dictionary<DialogType, object[]>();
        await base.OnAwake();
    }
    public override async UniTask InitializationAsync()
    {
        await AsyncDefault();
    }

    protected override async UniTask<Dictionary<string, object[]>> Init_Dialog_DicAsync()
    {
        await AsyncDefault();
        Dictionary<string, object[]> ret = new Dictionary<string, object[]>();
        //var fields = Enum.GetValues(typeof(DialogType));
        if (dialog_param != null)
        {
            foreach (var item in dialog_param)
            {
                ret.Add(item.Key.ToString(), item.Value);
            }
        }
        return ret;
    }
}
