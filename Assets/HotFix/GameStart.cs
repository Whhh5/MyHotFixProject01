using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("��ʼ���س���");
        Addressables.DownloadDependenciesAsync("Scene_UI_Prefab").Completed += (asyncOperationsHandle) =>
        {
            if (asyncOperationsHandle.Status != AsyncOperationStatus.Failed)
            {
                Debug.Log("���ذ��ɹ�");
                Addressables.LoadSceneAsync("Scene_Ui", LoadSceneMode.Single).Completed += (handle) =>
                {
                    if (handle.Status != AsyncOperationStatus.Failed)
                    {
                        Debug.Log("���� ui �����ɹ�");
                    }
                    else
                    {
                        Debug.Log("���� ui ����ʧ��");
                    }
                };
            }
            else
            {
                Debug.Log("���ذ�ʧ��");
            }
        };
    }
}
