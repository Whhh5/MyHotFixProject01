using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;

public class A_Mgr_UI : A_Mode_Singleton<A_Mgr_UI>
{
    public enum UIApp
    {
        App1,
        App2,
    }
    private A_List2<A_UIPage> pageStack = new A_List2<A_UIPage>();
    private A_UIPage nowPage = null;
    private async UniTask<T> OpenUIPageAsync<T>()
        where T:A_UIPage, new()
    {
        await AsyncDefault();
        var ret = new T();
        await ret.OpenAsync();
        nowPage = ret;
        return ret;
    }

    public async UniTask<T> ChangePageAsync<T>()
        where T:A_UIPage, new()
    {
        await AsyncDefault();
        if (nowPage != null)
        {
            var oldLenght = pageStack.countP;
            if (!pageStack.TryPush(nowPage))
            {
                A_LogToColor(Color.red, "add ui page stack defeated");
            }
            A_LogToColor(Color.black, $"dialog stack length change :   {oldLenght} -> {pageStack.countP}");
            await nowPage.CloseAsync();
        }
        T ret = await OpenUIPageAsync<T>();
        return ret;
    }
    public async UniTask CloseAllUIPageAsync()
    {
        await AsyncDefault();
        if (nowPage != null)
        {
            await nowPage.CloseAsync();
        }
        nowPage = null;
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
