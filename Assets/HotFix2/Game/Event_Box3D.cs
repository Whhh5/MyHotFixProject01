using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.OdinInspector;
using System;
using System.Reflection;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(BoxCollider))]
public class Event_Box3D : A_Mono
{
    [SerializeField, ReadOnly] Collider trigger = null;
    [SerializeField] ulong id;
    [SerializeField] LayerMask triggerLayerMask;
    Action action = null;
    Dictionary<string, Func<UniTask>> events = null;
    public override void OnAwake()
    {
        var eventName = $"Event_{id}";
        var mgr = Mgr_Event.Instance;
        Type type = mgr.GetType();
        var method = type.GetMethod(eventName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);

        Dictionary<string, Func<UniTask>> data = null;
        object[] objs = new object[] { data };
        method.Invoke(mgr, objs);
        events = objs[0] as Dictionary<string, Func<UniTask>>;
    }

    public override void OnStart()
    {

    }

    private void Reset()
    {
        trigger = GetComponent<Collider>();
        trigger.isTrigger = true;
    }
    private async void OnTriggerEnter(Collider other)
    {
        Debug.Log("1    {(int)Mathf.Pow(2, other.gameObject.layer)}     {triggerLayerMask.value}   {triggerLayerMask.ToString()}   {triggerLayerMask}");
        if ((int)Mathf.Pow(2, other.gameObject.layer) == triggerLayerMask && events != null && events.Count > 0)
        {
            await A_Mgr_UI.Instance.ChangePageAsync<UIPage_EventMenu>(events);
        }
    }

    private async void OnTriggerExit(Collider other)
    {
        Debug.Log($"2     {(int)Mathf.Pow(2, other.gameObject.layer)}     {triggerLayerMask.value}   {triggerLayerMask.ToString()}   {triggerLayerMask}");
        if ((int)Mathf.Pow(2, other.gameObject.layer) == triggerLayerMask && events != null && events.Count > 0)
        {
            await A_Mgr_UI.Instance.ChangePageAsync<UIPage_Default>();
        }
    }
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(transform.position, 3.0f);
    }
}
