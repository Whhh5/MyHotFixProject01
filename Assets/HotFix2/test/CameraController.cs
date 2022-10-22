using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CameraController : A_Mono
{
    [SerializeField] bool lock_Move = false;
    [SerializeField] Vector3 targetOffset = new Vector3(0, 20, -20);

    [SerializeField] LayerMask layer1;
    [SerializeField] LayerMask layer2;
    [SerializeField] LayerMask layer3;



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

    public override void OnStart()
    {
        A_Mgr_InputKey.Instance.AddKeyDownEvent(async (keycode) =>
        {
            await AsyncDefault();
            if (keycode == KeyCode.Mouse1)
            {
                var player = GameManager.Instance._nowPlayer;
                if (Equals(player, null)) return;
                var ray = GameManager.Instance._mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                var layer = LayerMask.NameToLayer("Ground");
                //if (EventSystem.current.IsPointerOverGameObject())
                //{

                //}
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer1) && GameManager.Instance.isPlayerMove)
                {
                    var objMask = hit.collider.gameObject.layer;
                    GameManager.Instance._mouse1SelectTarget = null;
                    if (Mathf.Pow(2, objMask) == layer2.value) //ground
                    {
                        await player._controller.SetDestination(hit.point);
                    }
                    if (Mathf.Pow(2, objMask) == layer3.value) //enemy
                    {
                        GameManager.Instance._mouse1SelectTarget = hit.collider.gameObject.transform;
                    }
                }
            }
        });
        A_Mgr_InputKey.Instance.AddAnyKeyEvent(async (keycode) =>
        {
            await AsyncDefault();
            if (keycode == KeyCode.Space)
            {
                var player = GameManager.Instance._nowPlayer;
                if (Equals(player, null)) return;
                var tempPose = player.transform.position + targetOffset;
                var pos = Vector3.Lerp(transform.position, tempPose, 10.0f * Time.deltaTime);
                transform.position = pos;
            }
        });


        A_Mgr_InputKey.Instance.AddKeyDownEvent(async (keyCode) =>
        {
            var tempData = GameManager.Instance._nowPlayer._skill;
            var data = await tempData.TryGetValueAsync(keyCode);
            if (!object.Equals(data, null))
            {
                var player = GameManager.Instance._nowPlayer;
                if (Equals(player, null)) return;
                if (!tempData.lock_Skill)
                {
                    tempData.lock_Skill = true;
                }
                else
                {
                    LogColor(Color.yellow, $"{name} is skilling , please wait ......    ");
                    return;
                }
                await data.PlayAsync(player, async (value) =>
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

    public async UniTask Pos_NormallizedAsync()
    {
        await AsyncDefault();
        var player = GameManager.Instance._nowPlayer;
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


}
