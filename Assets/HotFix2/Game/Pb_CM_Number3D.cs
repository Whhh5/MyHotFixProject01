using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using DG.Tweening;

public class Pb_CM_Number3D : Element3D
{
    [SerializeField] TextMesh text = null;
    [SerializeField] Transform position;
    [SerializeField] string message;

    public override async UniTask Initializationasync(params object[] parameters)
    {
        await AsyncDefault();
        gameObject.SetActive(false);

        message = (string)parameters[0];
        position = ((Transform)parameters[1]);
    }

    public override async UniTask OnShowAsync()
    {
        await AsyncDefault();
        gameObject.SetActive(true);
    }

    public override async UniTask PlayAsync(params object[] parameters)
    {
        await AsyncDefault();

        text.text = message;
        transform.position = position.position;
        transform.forward = transform.position - GameManager.Instance._mainCamera.transform.position;

        Vector3 moveTime = new Vector3(0.5f, 0.2f);
        float height = 2.0f;
        float maxScale = 3.0f;

        var localPosition = _main.localPosition;
        var localScale = _main.localScale;

        DOTween.To(() => 0, value =>
        {
            var proprotion = value * 2f;
            var alpha = proprotion > 1 ? 1 : proprotion;
            var scale = proprotion > 1 ? 1 : proprotion;
            scale = 1 + (maxScale - 1.0f) * (1 - scale);

            var tscale = Vector3.one * scale;
            var color = text.color;
            color.a = alpha;

            text.color = color;
            _main.localScale = tscale;
        }, 1, moveTime.x)
            .OnComplete(() =>
            {
                DOTween.To(() => 0, value =>
                {
                    var proprotion = 1 - value;
                    var alpha = proprotion < 0 ? 0 : proprotion;
                    var scale = proprotion < 0 ? 0 : proprotion;
                    scale = maxScale * proprotion;
                    var heightY = height * value;

                    var color = text.color;

                    color.a = alpha;
                    var tScale = (proprotion + value * 0.5f) * Vector3.one;

                    _main.localPosition = localPosition + new Vector3(0, heightY, 0);
                    text.color = color;
                    _main.localScale = tScale;

                }, 1, moveTime.y)
                    .OnComplete(async () =>
                    {
                        await RecyleAsync();
                    });
            });
    }
}
