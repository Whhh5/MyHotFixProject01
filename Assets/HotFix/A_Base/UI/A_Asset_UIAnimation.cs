using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New UIAnimationAsset", menuName = "UI Extend/Animation Asset")]
public class A_Asset_UIAnimation: SerializedScriptableObject
{
    public enum Enum_AnimationType
    {
        None,
        Random,
        Popup,
        Move,
        Jump,
        Sin,
        Cos,
        Alpha,
        Count,
    }
    public enum Enum_AnimaDircetion
    {
        None,
        Random,
        Left,
        Right,
        Top,
        Bottom,
        Count,
    }
    [SerializeField] private Enum_AnimationType anima_Show;
    [SerializeField] private Enum_AnimaDircetion anima_Dircetion;
    public void PlayAnimation(RectTransform rect, CanvasGroup canvasGroup = null)
    {
        switch (anima_Show)
        {
            case Enum_AnimationType.Popup:
                break;
            case Enum_AnimationType.Move:
                break;
            case Enum_AnimationType.Jump:
                break;
            case Enum_AnimationType.Sin:
                break;
            default:
                break;
        }
        Debug.Log($"{anima_Show}    {anima_Dircetion}");
    }
}
