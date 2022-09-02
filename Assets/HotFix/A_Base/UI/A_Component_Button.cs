using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BXB.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public enum ButtonStatus
{
    None,
    Up,
    Down,
    Enter,
    Exit,
    Click,
}
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class A_Component_Button : A_MonoAsync, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField, ReadOnly] ButtonStatus _state;
    private Func<ButtonStatus, UniTask> _buttonEvent = null;

    [SerializeField, ReadOnly] RectTransform _Rect = null;

    Vector3 scale_Down = new Vector3(0.8f, 0.8f, 0.8f);
    Vector3 scale_Up = new Vector3(1, 1, 1);
    Vector3 scale_Enter = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 scale_Exit = new Vector3(1, 1, 1);

    private void Reset()
    {
        InitParameter();
    }
    public override async UniTask OnAwakeAsync()
    {
        await AsyncDefault();
        InitParameter();
    }
    [Button]
    private void InitParameter()
    {
        _state = ButtonStatus.None;
        _Rect = GetComponent<RectTransform>();
        _buttonEvent = async (s) => { await AsyncDefault(); };
    }
    public override async UniTask OnStartAsync()
    {
        await AsyncDefault();
    }
    public override async UniTask OnShowAsync()
    {
        await AsyncDefault();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonEvent?.Invoke(ButtonStatus.Click);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _Rect.DOKill(false);
        _Rect.DOScale(scale_Down + Vector3.one * -0.1f, 0.2f)
            .OnComplete(()=>
            {
                _Rect.DOScale(scale_Down, 0.1f);
            });
        _buttonEvent?.Invoke(ButtonStatus.Down);
        _state &= ButtonStatus.Down;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _Rect.DOKill(false);
        _Rect.DOScale(scale_Enter + Vector3.one * 0.1f, 0.2f)
            .OnComplete(()=>
            {
                _Rect.DOScale(scale_Enter, 0.2f);
            });
        try
        {
            _state |= ButtonStatus.Enter;
            if (!A_Mgr_Lock.Instance.buttonEvent)
            {
                A_Mgr_Lock.Instance.buttonEvent = true;
                _buttonEvent?.Invoke(ButtonStatus.Enter);
                A_Mgr_Lock.Instance.buttonEvent = false;
            }
            else
            {
                A_LogToColor(Color.red, "[error]  have event is playing, please waitting");
            }
        }
        catch (System.Exception exp)
        {
            A_LogToColor(Color.red, exp);
            A_Mgr_Lock.Instance.buttonEvent = false;
        }
        finally
        {
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _Rect.DOKill(false);
        _Rect.DOScale(scale_Exit + Vector3.one * -0.1f, 0.2f)
            .OnComplete(()=>
            {
                _Rect.DOScale(scale_Exit, 0.2f);
            });
        _buttonEvent?.Invoke(ButtonStatus.Exit);
        _state ^= ButtonStatus.Up;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _Rect.DOKill(false);
        _Rect.DOScale(scale_Up + Vector3.one * 0.1f, 0.2f)
            .OnComplete(()=>
            {
                _Rect.DOScale(scale_Exit, 0.2f);
            });
        _buttonEvent?.Invoke(ButtonStatus.Up);
        _state ^= ButtonStatus.Enter;
    }



    public async UniTask AddClick(Func<ButtonStatus, UniTask> click)
    {
        await AsyncDefault();
        var arr = _buttonEvent.GetInvocationList();
        foreach (var item in arr)
        {
            if (item == (Delegate)click)
            {
                A_LogToColor(Color.red, "already existed delegate");
                return;
            }
        }
        _buttonEvent += click;
    }
    public async UniTask RemoveClick(Func<ButtonStatus, UniTask> click)
    {
        await AsyncDefault();
        var arr = _buttonEvent.GetInvocationList();
        foreach (var item in arr)
        {
            if (item == (Delegate)click)
            {
                _buttonEvent -= click;
                break;
            }
        }
    }
    public async UniTask<ButtonStatus> GetButtonState()
    {
        await AsyncDefault();
        return _state;
    }
}
