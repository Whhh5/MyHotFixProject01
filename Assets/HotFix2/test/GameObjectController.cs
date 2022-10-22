using BXB.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class GameObjectController : A_Mono
{
    [SerializeField] MeshRenderer select;

    LineRenderer line;
    NavMeshAgent navig;
    public override void OnAwake()
    {
        navig = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        var color = select.material.color;
        color.a = 0;
        select.material.color = color;
    }
    public override void OnStart()
    {
    }

    public async UniTask SetDestination(Vector3 point)
    {
        await AsyncDefault();
        LogColor(Color.yellow, $"{name} move to point ...... {transform.position} => {point}");
        navig.SetDestination(point);

        //var parameter = new NavMeshPath();
        //navig.CalculatePath(point, parameter);
    }

    //private void Update()
    //{
    //    DrawPath();
    //}

    Tween tween_Select = null;
    public async UniTask SetSclectStateAsync(float endValue)
    {
        await AsyncDefault();
        var color = select.material.color;
        tween_Select?.Kill();
        var startValue = color.a;
        var time = Mathf.Abs(endValue - startValue);
        tween_Select = DOTween.To(() => startValue, x =>
        {
            color.a = x;
            select.material.color = color;
        }, endValue, time);
        tween_Select.Play();
    }

    public async UniTask SetPathLineAsync(List<Vector3> points)
    {
        await AsyncDefault();
        List<Vector3> list = new List<Vector3>();
        List<Vector3> ret = new List<Vector3>();
        list.Add(transform.position);
        list.AddRange(points);
        for (int i = 1; i < list.Count; i++)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(list[i - 1], list[i], int.MaxValue, path);
            ret.AddRange(path.corners);
        }

        line.positionCount = ret.Count;
        line.SetPositions(ret.ToArray());
    }

    public void DrawPath()
    {
        if (!object.Equals(navig, null) && !Equals(line, null))
        {
            var poss = navig.path.corners;
            line.positionCount = (poss.Length);
            line.SetPositions(poss);
        }
    }
}
