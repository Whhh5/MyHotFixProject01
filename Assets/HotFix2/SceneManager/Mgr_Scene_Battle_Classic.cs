using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;

public class Mgr_Scene_Battle_Classic : A_Mode_Singleton_Mono<Mgr_Scene_Battle_Classic>
{
    public CameraController _camera;

    public override async void OnStart()
    {
        try
        {
            await A_Mgr_UI.Instance.ChangePageAsync<UIPage_BattleContriller>();
        }
        catch (System.Exception exp)
        {
            LogColor(Color.red, $"scene  initialization defeated  ....     (����ó�������ڳ����ɺ���)  \n\t message -> {exp}");
        }
    }

    public async UniTask SetPlayerAsync(ComponentList controller)
    {
        await _camera.SetController(controller);
        await _camera.UpdateProperty();
        await _camera.Pos_NormallizedAsync();
    }
}
