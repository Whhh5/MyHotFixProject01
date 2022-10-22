using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GameManager : A_Mode_Singleton<GameManager>
{
    public Camera _baseCamera { get; private set; } = null;
    public CameraController _mainCamera { get; private set; } = null;
    public GameObjectBase3D _nowPlayer { get; private set; } = null;
    public List<GameObjectBase3D> _nowPlayers { get; private set; } = new List<GameObjectBase3D>();

    public A_Mgr_Pool<PoolObjectBase> _public_Pool = new A_Mgr_Pool<PoolObjectBase>();

    public Transform _mouse1SelectTarget;
    public bool isPlayerMove = true;

    public async UniTask SetPlayerAsync(GameObjectBase3D controller)
    {
        await AsyncDefault();
        _nowPlayer?._controller.SetSclectStateAsync(0);
        _nowPlayer = controller;
        _nowPlayer?._controller.SetSclectStateAsync(1);
        await _mainCamera.Pos_NormallizedAsync();
    }

    public async UniTask SetMainCameraAsync(CameraController camera)
    {
        await AsyncDefault();
        if (!Equals(camera, null))
        {
            _mainCamera = camera;
            LogColor(Color.yellow, $"set main camera is name -> {camera.name}");
        }
        else LogColor(Color.red, "input mian camera is a nil");
    }
    public async UniTask SetBaseCameraAsync(Camera camera)
    {
        await AsyncDefault();
        if (!Equals(camera, null))
        {
            _baseCamera = camera;
            LogColor(Color.yellow, $"set base camera is name -> {camera.name}");
        }
        else LogColor(Color.red, "input base camera is a nil");
    }
    public async UniTask AddCameraStackAsync(Camera camera)
    {
        await AsyncDefault();
        if (_baseCamera == null)
        {
            LogColor(Color.red, "base camera is a nil");
            return;
        }
        var baseCamera = _baseCamera.GetUniversalAdditionalCameraData();
        var cam = baseCamera.cameraStack.IndexOf(camera);
        if (cam < 0)
        {
            baseCamera.cameraStack.Add(camera);
            LogColor(Color.black, $"add camera finish    camera name ->  {camera.name}");
        }
        else
        {
            LogColor(Color.yellow, $"this camera is exist    camera name -> {camera.name}");
        }
    }
    public async UniTask RemoveCameraStackAsync(Camera camera)
    {
        await AsyncDefault();
        if (_baseCamera == null)
        {
            LogColor(Color.red, "base camera is a nil");
            return;
        }
        var baseCamera = _baseCamera.GetUniversalAdditionalCameraData();
        baseCamera.cameraStack.Add(camera);
        var cam = baseCamera.cameraStack.IndexOf(camera);
        if (cam < 0)
        {
            LogColor(Color.yellow, $"this camera is exist    camera name -> {camera.name}");
        }
        else
        {
            LogColor(Color.black, $"add camera finish    camera name ->  {camera.name}");
            baseCamera.cameraStack.Remove(camera);
        }
    }
}
