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
        key_KeyDown += action;
    }
    public void AddAnyKeyEvent(Func<KeyCode, UniTask> action)
    {
        key_AnyKey += action;
    }

    public void RemoveKeyEvent(Func<KeyCode, UniTask> action)
    {
        Delegate[] arr = key_KeyDown.GetInvocationList();
        Delegate[] arr2 = key_AnyKey.GetInvocationList();

        if (Array.IndexOf(arr, action) != -1)
        {
            key_KeyDown -= action;
        }
        if (Array.IndexOf(arr2, action) != -1)
        {
            key_AnyKey -= action;
        }
    }

    public void RemoveAllKeyEvent(Func<KeyCode, UniTask> action)
    {
        key_KeyDown = async (key) => { await AsyncDefault(); };
    }
}


