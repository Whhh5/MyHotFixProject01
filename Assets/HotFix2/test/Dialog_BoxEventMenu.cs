using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class Dialog_BoxEventMenu : A_UIDialog
{
    [SerializeField] ComponentList original_Button = null;

    [SerializeField] A_Component_Button button_Close = null;
    protected override async UniTask InitializationAsync(params object[] param)
    {
        //await button_Close.AddClick(async (state) =>
        //{
        //    await AsyncDefault();
        //    if ((state & ButtonStatus.Click) == ButtonStatus.Click)
        //    {
        //        await _uiPage.CloseAsync();
        //    }
        //});
    }

    protected override async UniTask ShowAsync(params object[] param)
    {
        var dic = param[0] as Dictionary<string, Func<UniTask>>;
        await CreateButtonsEventAsync(dic);
    }
    protected override async UniTask HideAsync(params object[] param)
    {
        await AsyncDefault();
    }


    private async UniTask CreateButtonsEventAsync(Dictionary<string, Func<UniTask>> events)
    {
        await AsyncDefault();
        UniTask[] tasks = new UniTask[events.Count];
        int index = 0;
        foreach (var item in events)
        {
            var temp = item;
            var task = UniTask.Create(async () =>
            {
                var obj = Instantiate(original_Button, original_Button.transform.parent);
                if (obj.TryGet("Button", out A_Component_Button com))
                {
                    await com.AddClick(async (value) =>
                    {
                        if ((value & ButtonStatus.Click) == ButtonStatus.Click)
                        {
                            LogColor(Color.gray, "onclick");
                            await temp.Value.Invoke();
                        }
                    });
                }
                if (obj.TryGet("Text", out A_Component_Text com2))
                {
                    await com2.SetTextAsync(temp.Key);
                }
                obj.gameObject.SetActive(true);
            });
            tasks[index] = task;
            index++;
        }
        await UniTask.WhenAll(tasks);
    }
}
