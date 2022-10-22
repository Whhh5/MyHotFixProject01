using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class MiniMap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Camera miniCamera;
    bool miniMap_Toggle = false;
    [SerializeField] LayerMask mask;
    [SerializeField] KeyCode key;

    [SerializeField] Transform target;

    [SerializeField] A_Component_Button button_SetPath;
    [SerializeField] A_Component_Button button_ReturnUpPoint;
    [SerializeField] A_Component_Button button_Cancel;

    [SerializeField] bool isDawPath = false;
    [SerializeField] List<Vector3> path = new List<Vector3>();
    private async void Start()
    {
        await button_SetPath.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                isDawPath = !isDawPath;
                if (!isDawPath && path.Count > 0)
                {
                    await GameManager.Instance._nowPlayer.SetPathAsync(path);
                    path = new List<Vector3>();
                }
            }
        });
        await button_ReturnUpPoint.AddClick(async (state) =>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                if (path.Count > 0)
                {
                    path.RemoveAt(path.Count - 1);
                    await UpDatePathAsync();
                }
            }
        });
        await button_Cancel.AddClick(async (state)=>
        {
            if ((state & ButtonStatus.Click) == ButtonStatus.Click)
            {
                isDawPath = false;
                path = new List<Vector3>();
            }
        });
    }

    async UniTask UpDatePathAsync()
    {
        await GameManager.Instance._nowPlayer._controller.SetPathLineAsync(path);
    }
    public void MiniMapToggle(bool state)
    {
        float endValue = 0.3f;
        switch (state)
        {
            case true:
                endValue = 1.0f;
                break;
            default:
                break;
        }
        var rect = GetComponent<RectTransform>();
        rect.DOKill();
        rect.DOScale(endValue, 0.3f);
    }
    public async void OnPointerClick(PointerEventData eventData)
    {
        var ray = miniCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            if (isDawPath)
            {
                path.Add(hit.point);
                await UpDatePathAsync();
                Debug.Log($"add point -> {hit.point}        path count -> {path.Count}");
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            miniMap_Toggle = !miniMap_Toggle;
            MiniMapToggle(miniMap_Toggle);
            path = new List<Vector3>();
        }
        if (GameManager.Instance._nowPlayer)
        {
            miniCamera.transform.position = Vector3.Lerp(miniCamera.transform.position, GameManager.Instance._nowPlayer.transform.position + new Vector3(0,300,0), 10 * Time.deltaTime);
        }
        if (miniMap_Toggle)
        {
            miniCamera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * 10;
        }
    }
}
