using UnityEngine;
using UnityEngine.SceneManagement;
using BXB.Core;

public class GameStart : MonoBehaviour
{
    public async void Start()
    {
        Debug.Log("��ʼ���س���");
        await A_Mgr_Resource.Instance.LoadSceneAsync(SceneType.Scene_UI, LoadSceneMode.Single);


        return;
        //Addressables.DownloadDependenciesAsync("Scene_UI_Prefab").Completed += (asyncOperationsHandle) =>
        //{
        //    if (asyncOperationsHandle.Status != AsyncOperationStatus.Failed)
        //    {
        //        Debug.Log("���ذ��ɹ�");
                
        //        Addressables.LoadSceneAsync("Scene_Ui", LoadSceneMode.Single).Completed += (handle) =>
        //        {
        //            if (handle.Status != AsyncOperationStatus.Failed)
        //            {
        //                Debug.Log("���� ui �����ɹ�");
        //            }
        //            else
        //            {
        //                Debug.Log("���� ui ����ʧ��");
        //            }
        //        };
        //    }
        //    else
        //    {
        //        Debug.Log("���ذ�ʧ��");
        //    }
        //};
    }
}
