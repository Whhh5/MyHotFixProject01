using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using BXB.Core;

[RequireComponent(typeof(TextMeshProUGUI)), DisallowMultipleComponent]
public class A_Component_Text : A_Mono
{
    [SerializeField, ReadOnly] private TextMeshProUGUI component;

    [SerializeField] A_Asset_UITextMeshProStyle style;
    [SerializeField, Delayed] ulong key;
    //[ContextMenuItem("Initialization", "Initialization")]
    [SerializeField] string defaultvalue;

    private void Reset()
    {
        component = GetComponent<TextMeshProUGUI>();
        key = 1111;
        defaultvalue = name;
    }
    [ContextMenu("Init")]
    public void Initialization()
    {
        var isLanguage = false;
        string message;
        if (isLanguage)
        {
            message = "get table data";
        }
        else
        {
            message = defaultvalue;
        }
        component.text = message;
    }

    public override void OnAwake()
    {
        Initialization();
    }

    public override void OnStart()
    {
        
    }
}

