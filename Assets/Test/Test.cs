using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await A_Mgr_UI.Instance.ChangePageAsync<UIDialog_Lobby_Page>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
