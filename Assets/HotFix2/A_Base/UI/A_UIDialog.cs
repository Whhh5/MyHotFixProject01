using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.Serialization;
using Cysharp.Threading.Tasks;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public abstract class A_UIDialog : A_Mono
{
    [SerializeField] A_Asset_UIAnimation _anima_Asset;
    protected A_UIPage _uiPage = null;
    [SerializeField] List<GameObject> _startHideObjects = new List<GameObject>();
    public override void OnAwake()
    {
        foreach (var item in _startHideObjects)
        {
            item.SetActive(false);
        }
    }
    public override void OnStart()
    {
        
    }
    private void Reset()
    {
         A_Mgr_CommonAsset.GetAssets<Asset_DefaultValue>(A_Mgr_CommonAsset.AssetType.Asset_DefaultValue,(handle) =>
         {
             _anima_Asset = handle.A_Asset_UIAnimation;
         });
        
    }
    public virtual async UniTask OnInitializationAsync(params object[] param)
    {
        gameObject.SetActive(false);
        await InitializationAsync(param);
    }

    public async UniTask OnShowAsync(params object[] param)
    {
        LogColor(Color.black, "On Show");
        var rect = GetComponent<RectTransform>();
        rect.SetSiblingIndex(rect.parent.childCount-1);
        var canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(true);
        _anima_Asset?.PlayAnimation(rect, canvasGroup);
        await ShowAsync(param);
    }
    public async UniTask OnHideAsync(params object[] param)
    {
        gameObject.SetActive(false);
        await HideAsync(param);
    }
    protected async UniTask<Sprite> GetSprite(string name)
    {
        await AsyncDefault();
        var sprite = _uiPage._spriteAtles.GetSprite(name);
        return sprite;
    }
    public async UniTask SetUIPageAsync(A_UIPage page)
    {
        await AsyncDefault();
        _uiPage = page;
    }
    protected abstract UniTask InitializationAsync(params object[] param);
    protected abstract UniTask ShowAsync(params object[] param);
    protected abstract UniTask HideAsync(params object[] param);
}
