using BXB.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Asset_Skill-Ka_R", menuName = "Asset/Roles/Skill/Ka R")]
public class AssetSkill_Ka_R : Base_ScriptableObject_Skill
{
    [SerializeField] float intervalTime = 0.1f;
    [SerializeField] float radius = 20.0f;
    [SerializeField] uint bulletCount = 50;
    [SerializeField] LayerMask layerMast_Attack;
    [SerializeField] float slowDown_Speed = 0.3f;
    [SerializeField] float height = 5.0f;


    public override async UniTask PlayAsync(SkillParamater paras, Func<CallBackPara, UniTask> callback, params object[] parameters)
    {
        var transform = paras.tr;
        var playTime = bulletCount * intervalTime;
        paras.anv.speed *= slowDown_Speed;
        if (paras.componentList.TryGet("Main", out Transform transfor_main))
        {
            var originalHeight = transfor_main.localPosition;
            DOTween.To(() => 0.0f, x =>
               {
                   var tempHeight = Mathf.Sin(Mathf.PI * x) * height;
                   var tempPos = originalHeight;
                   tempPos.y = tempHeight;
                   transfor_main.localPosition = tempPos;
               }, 1, playTime);
            List<UniTask> tasks = new List<UniTask>();
            for (int i = 0; i < bulletCount; i++)
            {
                UniTask task = UniTask.Create(async () =>
                {
                    var obj = await A_Mgr_Resource.Instance.LoadElement3DAsync<Bullet>();
                    obj.gameObject.SetActive(false);
                    obj.transform.position = transfor_main.position;
                    var direction = transfor_main.position + new Vector3(UnityEngine.Random.Range(-radius, radius), UnityEngine.Random.Range(-radius, radius), UnityEngine.Random.Range(-radius, radius));
                    var delta = Random.Range(0, playTime);
                    await UniTask.Delay(TimeSpan.FromSeconds(delta));
                    var targets = Physics.OverlapSphere(transform.position, radius, layerMast_Attack);
                    var rangeEnemy = UnityEngine.Random.Range(0, targets.Length);
                    Transform target = null;
                    if (rangeEnemy < targets.Length)
                    {
                        target = targets[rangeEnemy].transform;
                    }
                    else
                    {
                        target = transfor_main;
                    }
                    await obj.MoveAsync(transfor_main, obj.transform.position, direction, target);
                });
                tasks.Add(task);
            }
            await UniTask.WhenAll(tasks);
            paras.anv.speed /= slowDown_Speed;
        }
    }
}
