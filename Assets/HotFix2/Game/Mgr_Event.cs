using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using Cysharp.Threading.Tasks;

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
            LogColor(Color.gray, "event_0");
        });
    }

    public void Event_100101(out Dictionary<string, Func<UniTask>> data)
    {
        data = new Dictionary<string, Func<UniTask>>();
        data.Add("event_100101", async () =>
        {
            await A_Mgr_Resource.Instance.LoadSceneAsync( SceneType.Scene_Battle_Classic, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        });
    }
}
