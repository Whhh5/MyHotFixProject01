using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NewAsset_Attack-Bow", menuName = "Asset/Roles/Skill/Attack-Bow")]
public class Asset_Attack_Bow : Base_ScriptableObject_Skill
{
    [SerializeField] float _attackInterval = 0.5f;
    [SerializeField] float _harm = 0.5f;
    [SerializeField] float _coolintTime = 0.5f;
    [SerializeField] float _residueTime = 0.0f;
    [SerializeField] float _attackRandom = 20.0f;
    [SerializeField, Range(0, 1)] float _slowDown_Speed = 0.5f;
    [SerializeField] Transform _arrow;
    public override async UniTask PlayAsync(GameObjectBase3D paras, Func<CallBackPara, UniTask> callback, params object[] parameters)
    {
        paras._skill.isAttack = !paras._skill.isAttack;
        Transform _target = GameManager.Instance._mouse1SelectTarget;
        var line = new GameObject("Link");
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startWidth = 0.2f;
        paras._navMeshAgent.speed *= _slowDown_Speed;
        Loop1Async(paras, _target, lineRenderer);
    }

    async UniTask Loop1Async(GameObjectBase3D paras, Transform target, LineRenderer line)
    {
        await UniTask.Delay(1);
        if (paras._skill.lock_Skill || target == null || !paras._skill.isAttack)
        {
            paras._navMeshAgent.speed /= _slowDown_Speed;
            GameObject.Destroy(line.gameObject);
            return;
        }
        var transform = paras.transform;
        var distance = Vector3.Distance(target.position, transform.position);
        var targetVec3 = target.position;
        var dircetion = target.position - transform.position;
        var maxHeight = distance / 10.0f;
        var arrow = GameObject.Instantiate(_arrow, null);
        GameObject.Destroy(arrow.gameObject, 5.0f);
        arrow.gameObject.SetActive(false);
        //10 -> 1
        var upPos = transform.position;
        var xzPlane = transform.position;
        arrow.position = transform.position;

        var porportion = 0.3f / distance;
        var x = dircetion.x * porportion;
        var z = dircetion.z * porportion;
        var y = dircetion.y * porportion;
        var increment = new Vector3(x, y, z);

        Func<UniTask> UpdateLoop = async () =>
        {
            arrow.gameObject.SetActive(true);
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                line.SetPositions(new Vector3[2] { transform.position, target.position });
                if (Vector3.Distance(transform.position, target.position) > _attackRandom)
                {
                    var dir = target.position - transform.position;
                    var dis1 = Vector3.Distance(target.position, transform.position);
                    var tarPos = transform.position + dir * (dis1 - _attackRandom) / dis1;
                    paras._navMeshAgent.SetDestination(tarPos);
                    continue;
                }

                var endPos = xzPlane;
                var temp_increment = increment;
                var dis = Vector3.Distance(xzPlane, target.position);
                var sinValue = Mathf.Sin(Mathf.PI * dis / distance);
                var addY = maxHeight * sinValue;
                temp_increment.y += addY;
                endPos.y += addY;
                arrow.forward = endPos - upPos;
                arrow.position = endPos;
                upPos = arrow.position;
                xzPlane += increment;
                if (Vector3.Distance(arrow.position, targetVec3) < 1)
                {
                    break;
                }
            }
            Debug.Log($"attack destroy ...... ");
        };
        UpdateLoop.Invoke();
        Debug.Log($"attack finsish ...... ");
        await UniTask.Delay(TimeSpan.FromSeconds(_attackInterval));
        await Loop1Async(paras, target, line);
    }
}