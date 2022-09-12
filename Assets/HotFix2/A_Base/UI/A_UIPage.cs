using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class A_UIPage : A_
{
    private A_List2<A_UIDialog> _dialogStack = null;
    public A_UIDialog _dialog = null;
    public SpriteAtlas _spriteAtles = null;
    protected object[] inputParameters = null;

    protected Dictionary<string, (A_UIDialog dialog, object[] parameter)> _dialog_Dic = null;
    private List<AsyncOperationHandle> _asyncOperationHamdles = null;
    public abstract UniTask InitializationAsync();
    protected abstract UniTask<Dictionary<string, object[]>> Init_Dialog_DicAsync();

    public async UniTask<Sprite> GetSprite(string name)
    {
        await AsyncDefault();
        Sprite ret = null;
        if (_spriteAtles != null)
        {
            try
            {
                ret = _spriteAtles.GetSprite(name);
            }
            catch (Exception exp)
            {
                LogColor(Color.red, $"get sprite defeated   sprite name ->{name}   {exp}");
            }
        }
        else
        {
            LogColor(Color.red, $"sprite atles is a nil   sprite name ->{GetType()}");
        }
        return ret;
    }
    protected async UniTask Init_Asset()
    {
        await AsyncDefault();
    }
    protected async UniTask<T> LoadAsync<T>(string name)
        where T : UnityEngine.Object
    {
        T ret = null;
        return ret;
        await A_Mgr_Resource.Instance.LoadOperationHandleAsync<T>(name, (handle) =>
        {
            ret = handle.Result;
            _asyncOperationHamdles.Add(handle);
        });
    }
    protected virtual async UniTask OnAwake()
    {
        await AsyncDefault();
        _dialog_Dic = new Dictionary<string, (A_UIDialog dialog, object[] parameter)>();
        _asyncOperationHamdles = new List<AsyncOperationHandle>();
        _dialogStack = new A_List2<A_UIDialog>();


        var name = $"SpriteAtlas_{GetType()}";
        try
        {
            _spriteAtles = await LoadAsync<SpriteAtlas>(name);
        }
        catch (Exception exp)
        {
            LogColor(Color.red, exp);
        }
    }
    private async UniTask OnStart()
    {
        await AsyncDefault();
        var datas = await Init_Dialog_DicAsync();
        foreach (var item in datas)
        {
            _dialog_Dic.Add(item.Key, (null, item.Value));
        }
    }

    public async UniTask OpenAsync(params object[] parameters)
    {
        inputParameters = parameters;
        await OnAwake();
        await OnStart();
        A_Mgr_InputKey.Instance.AddKeyDownEvent(Event_Key_EscapeAsync);

        List<UniTask> missions = new List<UniTask>();
        Dictionary<string, (A_UIDialog dialog, object[] parameter)> t_dialog_Dic = new Dictionary<string, (A_UIDialog dialog_t, object[] parameter)>();
        var parent = await A_Mgr_UI.Instance.GetUIApp(A_Mgr_UI.UIApp.App1);
        foreach (var item in _dialog_Dic)
        {
            var t_item = item;
            UniTask task = UniTask.Create(async () =>
            {
                var Dialog_Test1 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<A_UIDialog>(t_item.Key, parent);
                await Dialog_Test1.SetUIPageAsync(this);
                await Dialog_Test1.OnHideAsync();
                await Dialog_Test1.OnInitializationAsync(t_item.Value.parameter);
                t_dialog_Dic.Add(t_item.Key, (Dialog_Test1, t_item.Value.parameter));
            });
            missions.Add(task);
        }
        await UniTask.WhenAll(missions.ToArray());
        _dialog_Dic = t_dialog_Dic;
        await InitializationAsync();
    }
    public async UniTask ChangeDialogAsync(string dialog, params object[] param)
    {
        if (_dialog != null)
        {
            await this._dialog.OnHideAsync();
            var oldLenght = _dialogStack.countP;
            if (!_dialogStack.TryPush(this._dialog))
            {
                LogColor(Color.red, "add dialog stack defeated");
            }
            LogColor(Color.black, $"dialog stack length change :   {oldLenght} -> {_dialogStack.countP}");
        }
        await ShowUIDialogAsync(dialog, param);
    }
    private async UniTask ShowUIDialogAsync(string dialog, params object[] param)
    {
        if (_dialog_Dic.TryGetValue(dialog, out (A_UIDialog, object[]) data))
        {
            await data.Item1.OnShowAsync(param);
            if (this._dialog)
            {
                await this._dialog.OnHideAsync();
            }
            this._dialog = data.Item1;
        }
        else
        {
            LogColor(Color.red, $"ui dialog not extend    name  ->  {dialog}");
        }
    }
    public async UniTask Event_Key_EscapeAsync(KeyCode keyCode)
    {
        var oldLenght = _dialogStack.countP;
        if (keyCode == KeyCode.Escape &&
            _dialogStack.TryPop(out A_UIDialog dialogType))
        {
            var name = dialogType.name.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0];
            await ShowUIDialogAsync(name);
            LogColor(Color.black, $"dialog stack length change :   {oldLenght} -> {_dialogStack.countP}");
        }
    }
    public async UniTask CloseAsync()
    {
        await AsyncDefault();
        A_Mgr_InputKey.Instance.RemoveKeyEvent(Event_Key_EscapeAsync);
        List<UniTask> hideTasks = new List<UniTask>();
        List<UniTask> destoryTasks = new List<UniTask>();
        List<UniTask> operationHandle = new List<UniTask>();
        foreach (var item in _dialog_Dic)
        {
            var dialog = item.Value.dialog;
            var hideTask = UniTask.Create(async () =>
            {
                await dialog.OnHideAsync();
            });
            var destoryTask = UniTask.Create(async () =>
            {
                await AsyncDefault();
                GameObject.Destroy(item.Value.dialog.gameObject);
            });
            hideTasks.Add(hideTask);
            destoryTasks.Add(destoryTask);
        }
        _dialog_Dic = null;
        foreach (var item in _asyncOperationHamdles)
        {
            var data = item;
            var task = UniTask.Create(async () =>
            {
                await AsyncDefault();
                Addressables.Release(item);
                LogColor(Color.yellow, $"relese addressables assets  name -> {item.Result.ToString()}");
            });
            operationHandle.Add(task);
        }

        await UniTask.WhenAll(hideTasks);
        await UniTask.WhenAll(destoryTasks);
        await UniTask.WhenAll(operationHandle);
        //Destroy(this);
        //await Addressables.DownloadDependenciesAsync("");
    }
}
