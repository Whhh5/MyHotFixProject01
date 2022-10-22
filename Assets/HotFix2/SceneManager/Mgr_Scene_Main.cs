using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Scene_Main : MonoBehaviour
{
    async void Start()
    {
        await GameManager.Instance.SetBaseCameraAsync(GetComponent<Camera>());
        GameObject.DontDestroyOnLoad(this);
    }
}
