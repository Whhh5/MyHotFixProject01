using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;
using System.Reflection;

public class A_Mgr_UI : A_Mode_Singleton<A_Mgr_UI>
{
    public enum UIApp
    {
        App1,
        App2,
    }
    private A_List2<A_UIPage> _uiPageStack = new A_List2<A_UIPage>();
    private A_UIPage _nowPage = null;
    private bool _lock_pageOpen = false;
    private bool _lock_pageclose = false;

    private async UniTask<T> OpenUIPageAsync<T>(params object[] parameters)
        where T:A_UIPage, new()
    {
        await AsyncDefault();
        T ret = null;
        ret = new T();
        LogColor(Color.red, ret != null);
        await ret.OpenAsync(parameters);
        _nowPage = ret;
        LogColor(Color.red,ret != null);
        return ret;
    }

    public async UniTask<T> ChangePageAsync<T>(params object[] parameters)
        where T:A_UIPage, new()
    {
        T ret = null;
        await UniTask.WaitUntil(() => !_lock_pageOpen);
        _lock_pageOpen = true;
        try
        {
            if (_nowPage != null)
            {
                //var oldLenght = _uiPageStack.countP;
                //if (!_uiPageStack.TryPush(_nowPage))
                //{
                //    LogColor(Color.red, "add ui page stack defeated");
                //}
                //LogColor(Color.black, $"dialog stack length change :   {oldLenght} -> {_uiPageStack.countP}");
                await _nowPage.CloseAsync();
            }
            ret = new T();
            await ret.OpenAsync(parameters);
            _nowPage = ret;
        }
        catch (Exception exp)
        {
            LogColor(Color.red, $"ui page is exist error -> {exp}");
        }
        _lock_pageOpen = false;
        return ret;
    }
    public async UniTask CloseAllUIPageAsync()
    {
        await AsyncDefault();
        if (_nowPage != null)
        {
            await _nowPage.CloseAsync();
        }
        _nowPage = null;
    }
    public async UniTask<RectTransform> GetUIApp(UIApp app)
    {
        await AsyncDefault();
        var appPath = $"Canvas/{app}";
        var retObject = GameObject.Find(appPath);
        var ret = retObject.GetComponent<RectTransform>();
        return ret;
    }
}
