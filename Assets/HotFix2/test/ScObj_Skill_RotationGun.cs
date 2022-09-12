using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BXB.Core;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewAsset_Skill-Rotation Gun", menuName = "Asset/Roles/Skill/Rotation Gun")]
public class ScObj_Skill_RotationGun : Base_ScriptableObject_Skill
{
    [SerializeField] float time_ParsistPlay = 5.0f; //运行时间
    [SerializeField] float time_AttackInterval = 0.25f;
    [SerializeField] float slowDown_Speed = 0.3f;
    [SerializeField] float radius = 10.0f;
    [SerializeField] LayerMask layerMask_Target;
    [SerializeField] string buttleName = "Buttle";
    [SerializeField] float height = 0.5f;
    [SerializeField, Range(0.1f, 0.5f)] float topSpeed = 0.3f;
    [SerializeField] float harm = 999;
    [SerializeField] Material mat_Random;
    public async override UniTask PlayAsync(SkillParamater paras, Func<CallBackPara, UniTask> callback, params object[] parameters)
    {
        Debug.Log($"Start skill {name_skill} play ......");
        bool isEnd = false;
        var callbackParams = new CallBackPara();
        callbackParams.time_Residue = 0.0f;
        Transform attackRandom = null;
        var transform = paras.tr;
        var naviAgent = paras.anv;
        DOTween.To(() => time_ParsistPlay, (x) => callbackParams.time_Residue = x, 0.0f, time_ParsistPlay).SetEase(Ease.Linear)
            .OnStart(()=>
            {
                attackRandom = GetSphereAccackRandom(radius);
            })
            .OnUpdate(()=>
            {
                attackRandom.position = transform.position;
            })
            .OnComplete(() =>
            {
                GameObject.Destroy(attackRandom.gameObject);
                isEnd = true; 
            });
        if (paras.componentList.TryGet("Main", out transform))
        {
            var tempPos = transform.transform.position.y;
            transform.transform.DOMoveY(tempPos + height, time_ParsistPlay * topSpeed)
                .SetEase(Ease.InSine)
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(time_ParsistPlay * (1 - topSpeed * 2)));
                    transform.transform.DOMoveY(tempPos, time_ParsistPlay * topSpeed);
                });
            var tempRot = transform.transform.localRotation.eulerAngles;

            DOTween.To(() => tempRot.y, value =>
             {
                 transform.transform.localRotation = Quaternion.Euler(tempRot + new Vector3(0, value, 0));
             }, 360.0f * time_ParsistPlay, time_ParsistPlay)
                .SetEase(Ease.InSine)
                .OnComplete(() => { transform.transform.localRotation = Quaternion.Euler(tempRot); });
        }
        naviAgent.speed *= slowDown_Speed;
        while (!isEnd)
        {
            var center = transform.position;
            var radius = this.radius;
            var targets = new List<GameObject>();


            var caolliders = Physics.OverlapSphere(center, radius, layerMask_Target.value);
            List<UniTask> tasks = new List<UniTask>();
            //Gizmos.DrawWireSphere(center, radius);
            foreach (var item in caolliders)
            {
                if (Mathf.Pow(2, item.gameObject.layer) == layerMask_Target)
                {
                    var data = item;
                    targets.Add(data.gameObject);
                    await UniTask.Create(async () =>
                    {
                        var hint = await A_Mgr_Resource.Instance.LoadElement3DAsync<Pb_CM_Number3D>(harm.ToString(), data.transform);
                        await hint.OnShowAsync();
                        await hint.PlayAsync();
                    });
                }
            }

            callbackParams.targets = targets;
            await callback.Invoke(callbackParams);

            float schale = Mathf.Sin(Mathf.PI * (float)((callbackParams.time_Residue / time_ParsistPlay)));
            int gunCount = (int)(10 * (1 - schale) + 60 * schale);
            //Debug.Log($"--------------  {schale}      {gunCount}    {(float)((callbackParams.time_Residue / time_ParsistPlay))}");
            float intervalTime = time_AttackInterval / gunCount;
            for (int i = 0; i < gunCount; i++)
            {
                var end_Pos = GetSpherePointRange(radius);
                end_Pos = transform.TransformPoint(end_Pos);
                var t = intervalTime * gunCount * radius * time_AttackInterval / 10.0f / 0.4f;
                var moveTime = Random.Range(intervalTime * gunCount / 8, intervalTime * gunCount / 4);
                await CreateButtle(transform.position, end_Pos, moveTime);
                await UniTask.Delay(TimeSpan.FromSeconds(intervalTime));
            }
        }

        naviAgent.speed /= slowDown_Speed;
        Debug.Log($"End skill {name_skill} play ...... ");
    }

    public async UniTask<Transform> CreateButtle(Vector3 pos_start, Vector3 pos_end, float time)
    {
        await UniTask.Delay(0);
        var ret = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>(buttleName, null);
        ret.gameObject.SetActive(false);
        ret.position = pos_start;
        ret.forward = pos_end - pos_start;
        ret.DOMove(pos_end, time)
            .OnStart(() =>
            {
                ret.gameObject.SetActive(true);
            })
            .OnComplete(() =>
            {
                GameObject.Destroy(ret.gameObject);
            });
        return ret;
    }

    public Vector3 GetSpherePointRange(float radius)
    {
        Vector3 ret;
        int[] quadrant = new int[2]
        {
            1,-1
        };

        var radius_half = radius * 0.66f;
        var x = Random.Range(0.0f, radius_half);
        var z = Mathf.Pow(Mathf.Pow(radius_half, 2) - Mathf.Pow(x, 2), 0.5f);
        var y = Random.Range(0.0f, height);


        var quadrantX = quadrant[Random.Range(0, 2)];
        var quadrantY = quadrant[Random.Range(0, 2)];
        var quadrantZ = quadrant[Random.Range(0, 2)];

        //var porportion = Random.Range(0.66f, 1.0f);
        //ret = new Vector3(x, y, z);
        ret = new Vector3(x * quadrantX, y * quadrantY, z * quadrantZ);// / porportion;
        return ret;
    }

    public Transform GetSphereAccackRandom(float radius)
    {
        Transform ret;
        GameObject obj = new GameObject($"{GetType()}: Sphere attack random");
        var meshFilter = obj.AddComponent<MeshFilter>();
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = mat_Random;
        var mesh = new Mesh();
        int intervalAngle = 10;
        int halfCount = 90 / intervalAngle + 1;
        var vertices = new Vector3[(halfCount - 1) * 4 + 1];
        var triangles = new int[(halfCount - 1) * 3 * 4];

        vertices[0] = Vector3.zero;


        var map = new Dictionary<int, Vector3>
        {
            { 0, new Vector3(1,1,1)},
            { 1, new Vector3(-1,1,1)},
            { 2, new Vector3(-1,1,-1)},
            { 3, new Vector3(1,1,-1)},
        };
        for (int i = 1; i < halfCount; i++)
        {
            float x, y, z;
            int pos1, pos2, pos3;
            var red = Mathf.PI * 0.5f * ((float)(i-1) / (halfCount-1));
            z = Mathf.Sin(red);
            x = Mathf.Pow(1 - Mathf.Pow(z, 2), 0.5f);
            y = 0;


            for (int j = 0; j < 4; j++)
            {

                var m = map[j];
                var k = i + j * (halfCount - 1);
                float tX = x, tY = y, tZ = z;
                if (j % 2 != 0)
                {
                    float t = tX;
                    tX = tZ;
                    tZ = t;
                }
                pos1 = 0;
                pos2 = k;
                pos3 = k + 1;
                pos3 = pos3 < (halfCount - 1) * 4 + 1 ? pos3 : 1;

                triangles[(k - 1) * 3] = pos1;
                triangles[(k - 1) * 3 + 1] = pos2;
                triangles[(k - 1) * 3 + 2] = pos3;

                vertices[k] = new Vector3(tX * m.x, tY * m.y, tZ * m.z) * radius;
            }
        }


        //反转法线
        var normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        for (int i = 0; i < triangles.Length; i += 3)
        {
            (triangles[i], triangles[i + 2]) = (triangles[i + 2], triangles[i]);
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        meshFilter.mesh = mesh;
        ret = obj.transform;
        return ret;
    }
}


public class CallBackPara
{
    public float time_Residue;
    public List<GameObject> targets = new List<GameObject>();
}
public class SkillParamater
{
    public SkillParamater(Transform tr)
    {
        this.tr = tr;
        if (tr.TryGetComponent(out NavMeshAgent data1))
        {
            anv = data1;
        }
        if (tr.TryGetComponent(out ComponentList data2))
        {
            componentList = data2;
        }
    }
    public Transform tr;
    public NavMeshAgent anv;
    public ComponentList componentList;
    public (Transform left, Transform right) IKHand;
    public (Transform left, Transform right) IKFoot;
    public float par_float1;
    public float par_float2;
}