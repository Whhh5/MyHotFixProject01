using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class Mgr_Event : A_Mode_Singleton<Mgr_Event>
{
    public enum EventType
    {

    }
    public Dictionary<EventType, Action> _events = new Dictionary<EventType, Action>();
    public void ResponseEvent(EventType eventType)
    {
        if (_events.TryGetValue(eventType, out Action action))
        {
            action.Invoke();
        }
    }
    public void RegisterEvent(EventType eventType, Action action)
    {
        if (!_events.ContainsKey(eventType))
        {
            _events.Add(eventType, () => { });
        }
        else
        {
            _events[eventType] += action;
        }
    }

    public void Event_0(out Dictionary<string, Func<UniTask>> data)
    {
        data = new Dictionary<string, Func<UniTask>>();
        data.Add("event_0", async ()=>
        {
            await AsyncDefault();
            LogColor(Color.gray, "Start Battle");
        });
    }

    //Battle
    public void Event_100101(out Dictionary<string, Func<UniTask>> data)
    {
        data = new Dictionary<string, Func<UniTask>>();
        data.Add("Start Battle", async () =>
        {
            await A_Mgr_Resource.Instance.LoadSceneAsync( SceneType.Scene_Battle_Classic, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        });
    }
    //Shops-Item
    public void Event_100102(out Dictionary<string, Func<UniTask>> data)
    {
        data = new Dictionary<string, Func<UniTask>>();
        data.Add("Event_100102", async () =>
        {
            var parent = await A_Mgr_UI.Instance.GetUIApp(A_Mgr_UI.UIApp.System);
            await A_Mgr_Resource.Instance.LoadUIElementAsync<Dialog_SystemHint>(parent, "please wait open ...... ");
        });
    }
    //Shops-Forging
    public void Event_100103(out Dictionary<string, Func<UniTask>> data)
    {
        data = new Dictionary<string, Func<UniTask>>();
        data.Add("Event_100103", async () =>
        {
            var parent = await A_Mgr_UI.Instance.GetUIApp(A_Mgr_UI.UIApp.System);
            await A_Mgr_Resource.Instance.LoadUIElementAsync<Dialog_SystemHint>(parent, "please wait open ...... ");
        });

    }
}
