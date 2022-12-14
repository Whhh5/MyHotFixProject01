using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class Bullet : Element3D
{
    [SerializeField] LayerMask targetLayer;
    public async UniTask MoveAsync(Transform sponset, Vector3 start, Vector3 midPoint, Transform target)
    {
        var delta = UnityEngine.Random.Range(0, 0.0f);
        gameObject.SetActive(true);
        //yield return new WaitForSeconds(delta);
        DOTween.To(() => 0, x =>
          {
              transform.position = sponset.position;
          }, 1, delta);
        await UniTask.Delay(TimeSpan.FromSeconds(delta));
        start = transform.position;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Vector3 p1 = Vector3.Lerp(start, midPoint, i);
            Vector3 p2 = Vector3.Lerp(midPoint, target.position, i);
            Vector3 p = Vector3.Lerp(p1, p2, i);
            var speed = UnityEngine.Random.Range(100f, 1000f);
            await MoveToPointAsync(p, speed);
        }
        GameObject.Destroy(gameObject);
    }

    public async UniTask MoveToPointAsync(Vector3 p, float speed)
    {
        while (Vector3.Distance(transform.position, p) > 0.1f)
        {
            Vector3 dir = p - transform.position;
            transform.up = dir;
            transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            await UniTask.Yield();
        }
    }

    public override async UniTask Initializationasync(params object[] parameters)
    {
        await AsyncDefault();
    }
    public override async UniTask OnShowAsync()
    {
        await AsyncDefault();
    }
    public override async UniTask PlayAsync(params object[] parameters)
    {
        await AsyncDefault();
    }
}

//public class Bullet : PoolObjectBase
//{
//    [SerializeField] LayerMask targetLayer;
//    float speed = 0.0f;
//    private void Start()
//    {

//    }
//    public IEnumerator Move(Vector3 start, Vector3 midPoint, Transform target)
//    {
//        var delta = UnityEngine.Random.Range(0, 3.0f);
//        yield return new WaitForSeconds(delta);
//        for (float i = 0; i < 1; i += Time.deltaTime)
//        {
//            Vector3 p1 = Vector3.Lerp(start, midPoint, i);
//            Vector3 p2 = Vector3.Lerp(midPoint, target.position, i);
//            Vector3 p = Vector3.Lerp(p1, p2, i);
//            var speed = UnityEngine.Random.Range(100f, 1000f);
//            yield return StartCoroutine(MoveToPoint(p, speed));
//        }
//        Destroy(TestManager.Instance._pool);
//    }

//    public IEnumerator MoveToPoint(Vector3 p, float speed)
//    {
//        yield return null;
//        this.speed = speed;
//        while (Vector3.Distance(transform.position, p) > 0.1f)
//        {
//            Vector3 dir = p - transform.position;
//            transform.up = dir;
//            transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
//            yield return null;
//        }
//    }
//}
