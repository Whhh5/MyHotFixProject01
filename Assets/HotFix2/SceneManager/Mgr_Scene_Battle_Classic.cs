using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;

public class Mgr_Scene_Battle_Classic : A_Mode_Singleton_Mono<Mgr_Scene_Battle_Classic>
{
    [SerializeField] CameraController _camera = null;
    public override async void OnStart()
    {
        try
        {
            var camera = _camera.GetComponent<Camera>();
            await GameManager.Instance.AddCameraStackAsync(camera);
            await GameManager.Instance.SetMainCameraAsync(_camera);
            await A_Mgr_UI.Instance.ChangePageAsync<UIPage_BattleContriller>();
        }
        catch (System.Exception exp)
        {
            LogColor(Color.red, $"scene  initialization defeated  ....     (如果该场景是入口场景可忽略)  \n\t message -> {exp}");
        }
    }
    private async void OnDestroy()
    {
        var camera = _camera.GetComponent<Camera>();
        await GameManager.Instance.RemoveCameraStackAsync(camera);
    }
}
