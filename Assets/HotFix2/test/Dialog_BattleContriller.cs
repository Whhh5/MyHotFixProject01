using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Dialog_BattleContriller : A_UIDialog
{
    [SerializeField] ComponentList hero1;
    [SerializeField] ComponentList hero2;
    [SerializeField] ComponentList hero3;

    [SerializeField] A_Component_Button button_hero1;
    [SerializeField] A_Component_Button button_hero2;
    [SerializeField] A_Component_Button button_hero3;
    //[SerializeField] Dictionary<A_Component_Button, ComponentList> keyValuePairs = new Dictionary<A_Component_Button, ComponentList>();


    [SerializeField] ComponentList item_SKill;



    [SerializeField] A_Component_Button cameraMove_Left;
    [SerializeField] A_Component_Button cameraMove_Right;
    [SerializeField] A_Component_Button cameraMove_Top;
    [SerializeField] A_Component_Button cameraMove_Bottom;




    List<GameObject> skillList = new List<GameObject>();

    public async UniTask UpdateSkillIcon(ComponentList hero)
    {
        foreach (var item in skillList)
        {
            GameObject.Destroy(item);
        }
        skillList = new List<GameObject>();
        if (hero.TryGet("Skill", out Editor_Skill data))
        {
            List<UniTask> tasks = new List<UniTask>();
            for (int i = 0; i < data.value.Count; i++)
            {
                var index = i;
                var tempData = data.value[index];
                var keyCode = data.key[index];
                UniTask task = UniTask.Create(async () =>
                {
                    LogColor(Color.yellow, keyCode.ToString(), tempData.icon.name, index);
                    var obj = await GetItem_Skill(tempData.name_skill, tempData.icon, keyCode);
                    skillList.Add(obj.gameObject);
                });
                tasks.Add(task);
            }
            await UniTask.WhenAll(tasks);
        }
    }
    private async UniTask<RectTransform> GetItem_Skill(string name, Sprite sprite, KeyCode keyCode)
    {
        RectTransform ret = null;
        if (hero1.TryGet("Skill", out Editor_Skill data))
        {
            var obj = GameObject.Instantiate(item_SKill, item_SKill.transform.parent);
            ret = obj.GetComponent<RectTransform>();
            if (obj.TryGet("Icon", out Image image))
            {
                image.sprite = sprite;
            }
            if (obj.TryGet("NameText", out TextMeshProUGUI text))
            {
                text.text = name;
            }
            if (obj.TryGet("KeyCode", out TextMeshProUGUI keyCodeText))
            {
                keyCodeText.text = keyCode.ToString();
            }
            if (obj.TryGet("NameGroup", out CanvasGroup nameGroup))
            {
                Tween tween = null;
                nameGroup.alpha = 0;
                if (obj.TryGet("Button", out A_Component_Button button))
                {
                    await button.AddClick(async (state) =>
                    {
                        if (Equals((state & ButtonStatus.Enter), ButtonStatus.Enter))
                        {
                            PlayTween(1);
                        }
                    });
                    await button.AddClick(async (state) =>
                    {
                        if (Equals((state & ButtonStatus.Exit), ButtonStatus.Exit))
                        {
                            PlayTween(0);
                        }
                    });
                    void PlayTween(float endValue)
                    {
                        tween?.Kill();
                        var time = Mathf.Abs(endValue - nameGroup.alpha);
                        var startValue = nameGroup.alpha;
                        tween = DOTween.To(() => startValue, x =>
                        {
                            nameGroup.alpha = x;
                        }, endValue, time);
                        tween.Play();
                    }
                }
            }
            obj.gameObject.SetActive(true);
        }
        return ret;
    }
    protected override async UniTask HideAsync(params object[] param)
    {

    }

    protected override async UniTask InitializationAsync(params object[] param)
    {
        Cursor.lockState = CursorLockMode.Confined;
        await button_hero1.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                await Mgr_Scene_Battle_Classic.Instance.SetPlayerAsync(hero1);
                await UpdateSkillIcon(hero1);
            }
        });
        await button_hero2.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                await Mgr_Scene_Battle_Classic.Instance.SetPlayerAsync(hero2);
                await UpdateSkillIcon(hero2);
            }
        });
        await button_hero3.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                await Mgr_Scene_Battle_Classic.Instance.SetPlayerAsync(hero3);
                await UpdateSkillIcon(hero3);
            }
        });

        await cameraMove_Left.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await Mgr_Scene_Battle_Classic.Instance._camera.MoveAsync(CameraController.MoveType.Left);
            }
        });
        await cameraMove_Right.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await Mgr_Scene_Battle_Classic.Instance._camera.MoveAsync(CameraController.MoveType.Right);
            }
        });
        await cameraMove_Top.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await Mgr_Scene_Battle_Classic.Instance._camera.MoveAsync(CameraController.MoveType.Top);
            }
        });
        await cameraMove_Bottom.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await Mgr_Scene_Battle_Classic.Instance._camera.MoveAsync(CameraController.MoveType.Bottom);
            }
        });
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        var obj1 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player");
        var obj2 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player------1");
        var obj3 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player------2");

        hero1 = obj1.GetComponent<ComponentList>();
        hero2 = obj2.GetComponent<ComponentList>();
        hero3 = obj3.GetComponent<ComponentList>();

        await Mgr_Scene_Battle_Classic.Instance.SetPlayerAsync(hero1);
        await UpdateSkillIcon(hero1);
    }
}
