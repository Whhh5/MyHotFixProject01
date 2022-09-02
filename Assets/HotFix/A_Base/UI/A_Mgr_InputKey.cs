using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using PlasticGui.WebApi.Responses;

public class A_Mgr_InputKey : A_Mode_Singleton_Mono<A_Mgr_InputKey>
{
    private Func<KeyCode, UniTask> key_Escape = null;

    public override void OnAwake()
    {
        base.OnAwake();
        key_Escape = async (key) => { await AsyncDefault(); };
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        key_Escape?.Invoke(keyCode);
                    }
                }
            }
        }
    }

    public void AddKeyEvent(Func<KeyCode, UniTask> action)
    {
        key_Escape += action;
    }

    public void RemoveKeyEvent(Func<KeyCode, UniTask> action)
    {
        Delegate[] arr = key_Escape.GetInvocationList();
        if (Array.IndexOf(arr, action) != -1)
        {
            key_Escape -= action;
        }
    }

    public void RemoveAllKeyEvent(Func<KeyCode, UniTask> action)
    {
        key_Escape = async (key) => { await AsyncDefault(); };
    }
}