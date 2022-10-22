using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    [SerializeField] TestScript link = null;
    [SerializeField] LayerMask triggerLayer;

    [SerializeField] float sendObjectTime = 5.0f;
    [SerializeField] bool isSend = false;
    private void OnTriggerEnter(Collider other)
    {
        var lay = other.gameObject.layer;
        var layer = (int)Mathf.Pow(2, lay);
        if ((layer & triggerLayer.value) == layer && !Equals(link, null) && other.TryGetComponent(out GameObjectBase3D component) && !isSend)
        {
            component._navMeshAgent.Warp(link.transform.position);
            Send();
            link.Receive();
        }
    }

    public void Send()
    {
        isSend = true;
        var meshRenderer = GetComponent<MeshRenderer>();
        var tempAlpha = meshRenderer.material.color.a;
        DOTween.To(() => 0.0f, value =>
        {
            var color = meshRenderer.material.color;
            color.a = (1 - value) * tempAlpha;
            meshRenderer.material.color = color;
        }, 1, 0.5f)
            .OnComplete(() =>
            {
                DOTween.To(() => 0.0f, value =>
                {
                    var color = meshRenderer.material.color;
                    color.a = value * tempAlpha;
                    meshRenderer.material.color = color;
                }, 1, sendObjectTime)
                .OnComplete(() =>
                    {
                        isSend = false;
                    });
            });
    }
    public void Receive()
    {
        isSend = true;
        DOTween.To(() => 0.0f, value =>
        {

        }, 1, 0.5f)
            .OnComplete(() =>
            {
                DOTween.To(() => 0.0f, value =>
                {
                }, 1, sendObjectTime)
                .OnComplete(() =>
                {
                    isSend = false;
                });
            });
    }
}
