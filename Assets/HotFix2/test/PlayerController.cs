using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerController : A_Mono
{
    [SerializeField] NavMeshAgent navig;
    [SerializeField] LineRenderer line;
    [SerializeField] Transform hint;
    [SerializeField] MeshRenderer select;
    
    
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


        var parameter = new NavMeshPath();
        navig.CalculatePath(point, parameter);
    }

    private void Update()
    {
        DrawPath();
    }


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
    
    public void DrawPath()
    {
        if (!object.Equals(navig,null) && !Equals(line,null))
        {
            var poss = navig.path.corners;
            line.positionCount = (poss.Length);
            line.SetPositions(poss);
        }
    }
}
