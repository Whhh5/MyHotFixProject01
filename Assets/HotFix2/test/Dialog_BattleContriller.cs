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
    [SerializeField] A_Component_Button original_Button_Hero = null;
    //[SerializeField] Dictionary<A_Component_Button, ComponentList> keyValuePairs = new Dictionary<A_Component_Button, ComponentList>();


    [SerializeField] ItemList item_SKill;
    [SerializeField] A_Component_Button map_Mini = null;
    [SerializeField] bool miniMap_Toggle = false;

    [SerializeField] Camera camera_Mini = null;
    [SerializeField] RawImage miniMap = null;


    [SerializeField] A_Component_Button cameraMove_Left;
    [SerializeField] A_Component_Button cameraMove_Right;
    [SerializeField] A_Component_Button cameraMove_Top;
    [SerializeField] A_Component_Button cameraMove_Bottom;




    List<GameObject> skillList = new List<GameObject>();

    public async UniTask UpdateSkillIcon(GameObjectBase3D hero)
    {
        foreach (var item in skillList)
        {
            GameObject.Destroy(item);
        }
        skillList = new List<GameObject>();
        if (hero._itemList.TryGet("Skill", out Editor_Skill data))
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
        var nowPlayer = GameManager.Instance._nowPlayer;
        if (nowPlayer._itemList.TryGet("Skill", out Editor_Skill data))
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
        await AsyncDefault();
    }

    protected override async UniTask InitializationAsync(params object[] param)
    {
        Cursor.lockState = CursorLockMode.Confined;



        await cameraMove_Left.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await GameManager.Instance._mainCamera.MoveAsync(CameraController.MoveType.Left);
            }
        });
        await cameraMove_Right.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await GameManager.Instance._mainCamera.MoveAsync(CameraController.MoveType.Right);
            }
        });
        await cameraMove_Top.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await GameManager.Instance._mainCamera.MoveAsync(CameraController.MoveType.Top);
            }
        });
        await cameraMove_Bottom.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Long) == ButtonStatus.Long)
            {
                await GameManager.Instance._mainCamera.MoveAsync(CameraController.MoveType.Bottom);
            }
        });
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        var obj1 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player");
        var obj2 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player------1");
        var obj3 = await A_Mgr_Resource.Instance.LoadAssetInstantiateAsync<Transform>("Player------2");

        var  hero1 = obj1.GetComponent<GameObjectBase3D>();
        var hero2 = obj2.GetComponent<GameObjectBase3D>();
        var hero3 = obj3.GetComponent<GameObjectBase3D>();

        List<GameObjectBase3D> heroList = new List<GameObjectBase3D>
        {
            hero1,hero2,hero3
        };
        await InitHeroListAsync(heroList);
        LogColor(Color.yellow, $"hero count {heroList.Count}");
        await GameManager.Instance.SetPlayerAsync(hero1);
        await UpdateSkillIcon(hero1);
    }

    private async UniTask InitHeroListAsync(List<GameObjectBase3D> heros)
    {
        await AsyncDefault();
        var original = original_Button_Hero;
        UniTask[] tasks = new UniTask[heros.Count];
        for (int i = 0; i < heros.Count; i++)
        {
            var button_hero = Instantiate(original, original.transform.parent);
            var tempData = heros[i];
            await button_hero.AddClick(async (state) =>
            {
                if ((state & ButtonStatus.Click) == ButtonStatus.Click)
                {
                    await GameManager.Instance.SetPlayerAsync(tempData);
                    await UpdateSkillIcon(tempData);
                }
            });
            await button_hero.SetKeyboardShortcutAsync((KeyCode)(49 + i));
            button_hero.gameObject.SetActive(true);
        }
        await UniTask.WhenAll(tasks);
    }


}
