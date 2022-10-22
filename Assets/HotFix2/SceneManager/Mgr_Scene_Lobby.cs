using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public class Mgr_Scene_Lobby : A_Mode_Singleton_Mono<Mgr_Scene_Lobby>
{
    [SerializeField] CameraController _mainCamera = null;
    [SerializeField] GameObjectBase3D _tempPlayer = null;
    public override async void OnStart()
    {
        var camera = _mainCamera.GetComponent<Camera>();
        await A_Mgr_UI.Instance.ChangePageAsync<UIPage_Default>();
        await GameManager.Instance.SetMainCameraAsync(_mainCamera);
        await GameManager.Instance.AddCameraStackAsync(camera);
        await GameManager.Instance.RemoveCameraStackAsync(camera);
        await GameManager.Instance.SetPlayerAsync(_tempPlayer);
    }
    private void OnDestroy()
    {
        //var camera = _mainCamera.GetComponent<Camera>();
        //await GameManager.Instance.RemoveCameraStackAsync(camera);
    }
}
