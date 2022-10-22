using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using BXB.Core;
using Cysharp.Threading.Tasks;

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
        key = 0;
        defaultvalue = name;

        A_Mgr_CommonAsset.GetAssets<Asset_DefaultValue>(A_Mgr_CommonAsset.AssetType.Asset_DefaultValue, (handle) =>
        {
            style = handle.A_Asset_TextMeshProUGUI;
        });
        
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
        //component.text = message;
    }
    public async UniTask SetTextAsync(object messgae)
    {
        await AsyncDefault();
        try
        {
            component.text = messgae.ToString();
        }
        catch (System.Exception exp)
        {
            LogColor(Color.red, exp);
        }
    }

    public override void OnAwake()
    {
        Initialization();
    }

    public override void OnStart()
    {
        
    }
}

