using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;

public class CameraController : A_Mono
{
    [SerializeField] ComponentList playerController = null;
    [SerializeField] PlayerController player = null;
    [SerializeField] Editor_Skill skill = null;
    [SerializeField] bool lock_Move = false;
    [SerializeField] Vector3 targetOffset = new Vector3(0, 20, -20);

    public enum MoveType
    {
        None = 1,
        Left = 2,
        Right = 4,
        Top = 6,
        Bottom = 8,
    }
    public override void OnAwake()
    {
        lock_Move = false;
    }

    public override async void OnStart()
    {
        await UpdateProperty();
        A_Mgr_InputKey.Instance.AddKeyDownEvent(async (keycode) =>
        {
            await AsyncDefault();
            if (keycode == KeyCode.Mouse1 && player != null)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var layer = LayerMask.NameToLayer("Ground");
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, (int)Mathf.Pow(2, layer)))
                {
                    await player.SetDestination(hit.point);
                }
            }
        });
        A_Mgr_InputKey.Instance.AddAnyKeyEvent(async (keycode) =>
        {
            await AsyncDefault();
            if (keycode == KeyCode.Space && player != null)
            {
                var tempPose = player.transform.position + targetOffset;
                var pos = Vector3.Lerp(transform.position, tempPose, 10.0f * Time.deltaTime);
                transform.position = pos;
            }
        });


        A_Mgr_InputKey.Instance.AddKeyDownEvent(async (keyCode) =>
        {
            var tempData = skill;
            var data = await tempData.TryGetValueAsync(keyCode);
            if (!object.Equals(data, null))
            {
                if (!tempData.lock_Skill)
                {
                    tempData.lock_Skill = true;
                }
                else
                {
                    LogColor(Color.yellow, $"{name} is skilling , please wait ......    ");
                    return;
                }
                SkillParamater paras = new SkillParamater(player.transform);
                await data.PlayAsync(paras, async (value) =>
                {
                    await AsyncDefault();
                    string str = $"{value.time_Residue.ToString()}\t";
                    foreach (var item in value.targets)
                    {
                        str += item.name + '\t';
                    }
                    LogColor(Color.green, str);
                });
                tempData.lock_Skill = false;
            }
        });
    }

    public async UniTask SetController(ComponentList controller)
    {
        playerController = controller;
    }

    public async UniTask UpdateProperty()
    {
        await AsyncDefault();
        if (!object.Equals(playerController, null))
        {
            if (!Equals(player, null))
            {
                await player.SetSclectStateAsync(0);
            }
            if (!playerController.TryGet("Player", out player))
            {
                LogColor(Color.red, $"Get component defeated, player name -> {playerController.name}, attrite name -> Player");
            }
            else
            {
                await player.SetSclectStateAsync(1);
            }
            if (!playerController.TryGet("Skill", out skill))
            {
                LogColor(Color.red, $"Get component defeated, skill name -> {playerController.name}, attrite name -> Skill");
            }
        }
        else
        {
            LogColor(Color.red, "error: controller is a nil ......");
        }
    }
    public async UniTask MoveAsync(MoveType mode)
    {
        if (!lock_Move)
        {
            float x = 0, y = 0, z = 0;

            switch (mode)
            {
                case MoveType.None:
                    break;
                case MoveType.Left:
                    x = -1;
                    break;
                case MoveType.Right:
                    x = 1;
                    break;
                case MoveType.Top:
                    z = 1;
                    break;
                case MoveType.Bottom:
                    z = -1;
                    break;
                default:
                    break;
            }
            transform.position += new Vector3(x, y, z) * Time.deltaTime * 30;
        }
    }
    public async UniTask Pos_NormallizedAsync()
    {
        await AsyncDefault();
        var pos_Target = player.transform.position + targetOffset;
        transform.DOKill();
        transform.DOMove(pos_Target, 2.0f)
            .OnStart(() =>
            {
                lock_Move = true;
            })
            .OnComplete(() =>
            {
                lock_Move = false;
            });
    }
}
