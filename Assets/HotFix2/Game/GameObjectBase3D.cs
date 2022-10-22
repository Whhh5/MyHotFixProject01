using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BXB.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Editor_Skill))]
[RequireComponent(typeof(Editor_Skeleton))]
[RequireComponent(typeof(GameObjectController))]
[RequireComponent(typeof(ItemList))]
public class GameObjectBase3D : A_MonoAsync
{
    public GameObjectController _controller { get; private set; } = null;
    public Editor_Skill _skill { get; private set; } = null;
    public Editor_Skeleton _skeleton { get; private set; } = null;
    public NavMeshAgent _navMeshAgent { get; private set; } = null;
    public ItemList _itemList { get; private set; } = null;

    public List<Vector3> _navPath { get; private set; } = new List<Vector3>();
    Tween AIMoveTween = null;
    public int this[int i]
    {
        get { return 1; }
        set { }
    }
    public override async UniTask OnAwakeAsync()
    {
        await AsyncDefault();
        _skill = GetComponent<Editor_Skill>();
        _skeleton = GetComponent<Editor_Skeleton>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<GameObjectController>();
        _itemList = GetComponent<ItemList>();
    }

    public override async UniTask OnShowAsync()
    {
        await AsyncDefault();
    }

    public override async UniTask OnStartAsync()
    {
        await AsyncDefault();
    }


    public async UniTask SetPathAsync(List<Vector3> path)
    {
        AIMoveTween.Kill();
        _navMeshAgent.isStopped = true;
        _navPath = path;
        StopCoroutine(PlayAIMove());
        

        StartCoroutine(PlayAIMove());
    }


    private IEnumerator PlayAIMove()
    {
        _navMeshAgent.isStopped = false;
        foreach (var point in _navPath)
        {
            yield return null;
            _navMeshAgent.SetDestination(point);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, point) < _navMeshAgent.stoppingDistance);
        }
    }
}
