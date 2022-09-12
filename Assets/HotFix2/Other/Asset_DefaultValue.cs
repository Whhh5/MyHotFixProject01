using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

[CreateAssetMenu(fileName = "Asset_DefaultValue", menuName = "Asset/Common/Default Value")] 
public class Asset_DefaultValue : SerializedScriptableObject
{
    [SerializeField] A_Asset_UIAnimation _A_Asset_UIAnimation = null;
    public A_Asset_UIAnimation A_Asset_UIAnimation
    {
        get
        {
            return _A_Asset_UIAnimation;
        }
    }
    [SerializeField] A_Asset_UITextMeshProStyle _A_Asset_TextMeshProUGUI = null;
    public A_Asset_UITextMeshProStyle A_Asset_TextMeshProUGUI
    {
        get
        {
            return _A_Asset_TextMeshProUGUI;
        }
    }
}
