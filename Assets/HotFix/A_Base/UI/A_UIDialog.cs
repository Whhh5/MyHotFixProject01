using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.Serialization;
using Cysharp.Threading.Tasks;
using UnityEngine.U2D;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public abstract class A_UIDialog : A_Mono
{
    [SerializeField] A_Asset_UIAnimation _anima_Asset;
    [SerializeField] SpriteAtlas _spriteAtles;
    public virtual async UniTask OnInitializationAsync(params object[] param)
    {
        gameObject.SetActive(false);

        /*1.º”‘ÿÕººØ   spriteAtles
         * 
         * 
         * 
         */
        await InitializationAsync(param);
    }

    public async UniTask OnShowAsync(params object[] param)
    {
        A_LogToColor(Color.black, "On Show");
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
        var sprite = _spriteAtles.GetSprite(name);
        return sprite;
    }
    protected abstract UniTask InitializationAsync(params object[] param);
    protected abstract UniTask ShowAsync(params object[] param);
    protected abstract UniTask HideAsync(params object[] param);
}
