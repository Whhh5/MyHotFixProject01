using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public class Mgr_Scene_Inet : A_Mode_Singleton_Mono<Mgr_Scene_Inet>
{
    public override async void OnStart()
    {
        await A_Mgr_UI.Instance.ChangePageAsync<UIPage_Lobby>();
    }
}
