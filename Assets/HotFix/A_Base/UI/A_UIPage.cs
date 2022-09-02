using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;

public abstract class A_UIPage : A_
{
    private A_List2<A_UIDialog> dialogStack = new A_List2<A_UIDialog>();
    public A_UIDialog dialog = null;

    protected Dictionary<string, A_UIDialog> dialog_Dic = new Dictionary<string, A_UIDialog>();

    public abstract UniTask InitializationAsync();
    public abstract UniTask<List<string>> GetDialogsType();
    public async UniTask OpenAsync()
    {
        A_Mgr_InputKey.Instance.AddKeyEvent(Event_Key_EscapeAsync);
        var dialogs = await GetDialogsType();

        var parent = await A_Mgr_UI.Instance.GetUIApp(A_Mgr_UI.UIApp.App1);
        foreach (var item in dialogs)
        {
            var Dialog_Test1 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<A_UIDialog>(item, parent);
            dialog_Dic.Add(item, Dialog_Test1);
        }
        await InitializationAsync();
    }
    public async UniTask LoadDialogAsync(A_UIDialog dialog, params object[] param)
    {
        await dialog.OnInitializationAsync(param);
    }
    public async UniTask ChangeDialogAsync(string dialog, params object[] param)
    {
        if (this.dialog != null)
        {
            await this.dialog.OnHideAsync();
            var oldLenght = dialogStack.countP;
            if (!dialogStack.TryPush(this.dialog))
            {
                A_LogToColor(Color.red, "add dialog stack defeated");
            }
            A_LogToColor(Color.black,$"dialog stack length change :   {oldLenght} -> {dialogStack.countP}");
        }
        if (dialog_Dic.TryGetValue(dialog, out A_UIDialog uiDialog))
        {
            await ShowUIDialogAsync(uiDialog, param);
            this.dialog = uiDialog;
        }
        else
        {
            A_LogToColor(Color.red, $"ui dialog not extend    name  ->  {dialog}");
        }
    }
    private async UniTask ShowUIDialogAsync(A_UIDialog dialog, params object[] param)
    {
        await dialog.OnShowAsync(param);
    }
    public async UniTask Event_Key_EscapeAsync(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Escape && 
            dialogStack.TryPop(out A_UIDialog dialogType))
        {
            var name = dialogType.name;
            await ChangeDialogAsync(name);
        }
    }
    public async UniTask CloseAsync()
    {
        await AsyncDefault();
        A_Mgr_InputKey.Instance.RemoveKeyEvent(Event_Key_EscapeAsync);
    }
}
