using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

[CreateAssetMenu(fileName = "Asset_UITextMeshProStyle", menuName = "Asset/UI/UI TextMeshPro Style")]
public class A_Asset_UITextMeshProStyle : SerializedScriptableObject
{
    public enum Color
    {
        None,
        Red,
        Count,
    }
    public enum OutLine
    {
        None,
        One,
        Count,
    }
    [SerializeField] private Color color;
    [SerializeField] private OutLine outLine;

    public void SetStyle(TextMeshProUGUI textMesh)
    {

    }
}
