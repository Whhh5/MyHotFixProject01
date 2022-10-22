using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Dialog_SystemHint : PoolObjectBase
{
    CanvasGroup group= null;
    [SerializeField] A_Component_Text message;
    public override async UniTask InitAsync(params object[] parameters)
    {
        await AsyncDefault();
        group = GetComponent<CanvasGroup>();
        var message = (string)parameters[0];
        await this.message.SetTextAsync(message);
    }

    public override async UniTask PlayAsync(params object[] parameters)
    {
        gameObject.SetActive(true);
        if (group != null)
        {
            await PlayTweenAsync();
        }
    }
    private async UniTask PlayTweenAsync()
    {
        await AsyncDefault();
        DOTween.To(() => 0.0f, x =>
        {
            group.alpha = x;
        }, 1, 1)
            .OnComplete(() =>
            {
                DOTween.To(() => 1.0f, x =>
                {
                    group.alpha = x;
                }, 0, 1)
                    .OnComplete(async () =>
                    {
                        await DestroyAsync(GameManager.Instance._public_Pool);
                    });
            });
    }
    public override async UniTask DestroyAsync()
    {
        await AsyncDefault();
    }
}
