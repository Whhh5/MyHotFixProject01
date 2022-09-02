using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BXB
{
    namespace Core
    {
        public class A_Mgr_Resource : A_Mode_Singleton<A_Mgr_Resource>, IMgr_Resource
        {
            public async Task<TReturn> LoadAssetAsync<TReturn>(string name)
                where TReturn : UnityEngine.Object
            {
                TReturn ret = null;
                try
                {
                    var handle = Addressables.LoadAssetAsync<GameObject>(name);
                    handle.Completed += (handle) =>
                    {
                        if (handle.Status != AsyncOperationStatus.Failed)
                        {
                            ret = handle.Result.GetComponent<TReturn>();
                        }
                    };
                    await handle.Task;
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message);
                }
                return ret;
            }
            public async Task<TReturn> LoadAssetInstantiateAsync<TReturn>(string name, Transform parent) 
                where TReturn : UnityEngine.Object
            {
                TReturn ret = null;
                try
                {
                    ret = await LoadAssetAsync<TReturn>(name);
                    ret = UnityEngine.Object.Instantiate(ret, parent);
                }
                catch (Exception exp)
                {
                    A_LogToColor( Color.red, $"{name}  ->  {exp.Message}");
                    //ret = (TReturn)(new GameObject()).AddComponent<A_MonoAsync>();
                }
                return ret;
            }

            public async Task DownLoadAddressables()
            {
                await AsyncDefault();
            }



            public bool TryLoadSystemPoolObject()
            {
                bool ret = false;

                if (true)
                {




                    ret = true;
                }
                return ret;
            }
            public void ClearSystemPool()
            {

            }
        }
    }
}