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
    None = 1,
    Up = 2,
    Down = 4,
    Enter = 8,
    Exit = 16,
    Click = 32,
    Long = 64,
}

[RequireComponent(typeof(RectTransform))]
public class A_Component_Button : A_MonoAsync, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField, ReadOnly] ButtonStatus _state;
    private Func<ButtonStatus, UniTask> _buttonEvent = null;
    [SerializeField, ReadOnly] RectTransform _Rect = null;
    [SerializeField] KeyCode keyboardShortcut;


    private void Reset()
    {
        InitParameter();
    }
    public override async UniTask OnAwakeAsync()
    {
        await AsyncDefault();
    }
    [Button]
    private void InitParameter()
    {
        _state = ButtonStatus.None;
        _Rect = GetComponent<RectTransform>();
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
        UniTask.Void(SetTween);
        _buttonEvent?.Invoke(ButtonStatus.Down);
        _state |= ButtonStatus.Down;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UniTask.Void(SetTween);
        try
        {
            _state |= ButtonStatus.Enter;
            _state |= ButtonStatus.Long;
            if (!A_Mgr_Lock.Instance.buttonEvent)
            {
                A_Mgr_Lock.Instance.buttonEvent = true;
                _buttonEvent?.Invoke(ButtonStatus.Enter);
                A_Mgr_Lock.Instance.buttonEvent = false;
            }
            else
            {
                LogColor(Color.red, "[error]  have event is playing, please waitting");
            }
        }
        catch (System.Exception exp)
        {
            LogColor(Color.red, exp);
            A_Mgr_Lock.Instance.buttonEvent = false;
        }
        finally
        {
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UniTask.Void(SetTween);
        _buttonEvent?.Invoke(ButtonStatus.Exit);
        _state -= ButtonStatus.Enter;
        _state -= ButtonStatus.Long;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UniTask.Void(SetTween);
         _buttonEvent?.Invoke(ButtonStatus.Up);
        _state -= ButtonStatus.Down;
    }

    private async UniTaskVoid SetTween()
    {
        await AsyncDefault();
        _Rect.DOKill(false);
        Vector3 scale = Vector3.one;
        float plusOrMinus = 0;
        if ((_state & ButtonStatus.Down) != 0)
        {
            scale = Vector3.one * 0.9f;
            plusOrMinus = -1f;
        }
        else if ((_state & ButtonStatus.Enter) != 0)
        {
            scale = Vector3.one * 1.1f;
            plusOrMinus = 1f;
        }
        _Rect.DOScale(scale + Vector3.one * 0.1f * plusOrMinus, 0.1f)
            .OnComplete(() =>
            {
                _Rect.DOScale(scale, 0.1f);
            });
    }


    public async UniTask AddClick(Func<ButtonStatus, UniTask> click)
    {
        await AsyncDefault();
        if (_buttonEvent == null)
        {
            _buttonEvent = async (x) => { await AsyncDefault(); };
        }
        var arr = _buttonEvent.GetInvocationList();
        foreach (var item in arr)
        {
            if (item == (Delegate)click)
            {
                LogColor(Color.red, "already existed delegate");
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

    public async UniTask SetKeyboardShortcutAsync(KeyCode keyCode)
    {
        await AsyncDefault();
        keyboardShortcut = keyCode;
    }

    private void Update()
    {
        if ((_state & ButtonStatus.Long) == ButtonStatus.Long)
        {
            _buttonEvent?.Invoke(ButtonStatus.Long);
        }
        if (Input.GetKeyDown(keyboardShortcut))
        {
            OnPointerClick(null);
        }
    }

    public async UniTask OnDestroyAsync()
    {
        await AsyncDefault();
    }

    private async void OnDestroy()
    {
        await OnDestroyAsync();
    }
}
