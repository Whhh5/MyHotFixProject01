using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;

public class A_Mgr_InputKey : A_Mode_Singleton_Mono<A_Mgr_InputKey>
{
    private Func<KeyCode, UniTask> key_KeyDown = null;
    private Func<KeyCode, UniTask> key_AnyKey = null;

    public override void OnStart()
    {
        base.OnAwake();
        key_KeyDown = async (key) => { await AsyncDefault(); };
        key_AnyKey = async (key) => { await AsyncDefault(); };
    }
    void Update()
    {

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    key_KeyDown?.Invoke(keyCode);
                }
            }
        }


        if (Input.anyKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    key_AnyKey?.Invoke(keyCode);
                }
            }
        }

    }

    public void AddKeyDownEvent(Func<KeyCode, UniTask> action)
    {
        Delegate[] arr = key_KeyDown.GetInvocationList();
        var index1 = Array.IndexOf(arr, action);
        if (index1 != -1)
        {
            return;
        }
        key_KeyDown += action;
        LogColor(Color.black, $"now key down event count -> {arr.Length}");
    }
    public void AddAnyKeyEvent(Func<KeyCode, UniTask> action)
    {
        Delegate[] arr2 = key_AnyKey.GetInvocationList();

        var index2 = Array.IndexOf(arr2, action);
        if (index2 != -1)
        {
            return;
        }
        key_AnyKey += action;
        LogColor(Color.black, $"now key down event count -> {arr2.Length}");
    }

    public void RemoveKeyEvent(Func<KeyCode, UniTask> action)
    {
        Delegate[] arr = key_KeyDown.GetInvocationList();
        Delegate[] arr2 = key_AnyKey.GetInvocationList();

        var index1 = Array.IndexOf(arr, action);
        var index2 = Array.IndexOf(arr2, action);
        if (index1 != -1)
        {
            key_KeyDown -= action;
        }
        if (index2 != -1)
        {
            key_AnyKey -= action;
        }

        LogColor(Color.yellow, $"----------------index1 -> {index1},    index2 -> {index2} ---------------- {action != null}");
    }

    public void RemoveAllKeyEvent(Func<KeyCode, UniTask> action)
    {
        key_KeyDown = async (key) => { await AsyncDefault(); };
    }
}


