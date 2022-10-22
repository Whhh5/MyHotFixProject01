using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace BXB
{
    namespace Core
    {
        public enum SceneType
        {
            None,
            main,
            Scene_Inlet,
            Scene_UI,
            Scene_Lobby,
            Scene_Battle_Classic,
        }

        public class A_Mgr_Resource : A_Mode_Singleton<A_Mgr_Resource>
        {
            Dictionary<string, AsyncOperationHandle> map_Scene = new Dictionary<string, AsyncOperationHandle>();
            public async UniTask LoadOperationHandleAsync<T>(string name, Action<AsyncOperationHandle<T>> callback = null)
            {
                AsyncOperationHandle<T> ret = default;
                var handle = Addressables.LoadAssetAsync<T>(name);
                handle.Completed += (handle) =>
                {
                    ret = handle;
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        LogColor(Color.black, $"addressables load finish     name   ->    {name}");
                        callback?.Invoke(handle);
                    }
                };
                await handle.Task;
            }
            public async UniTask<TType> LoadAssetAsync<TType>(string name)
                where TType : UnityEngine.Object
            {
                TType ret = null;
                try
                {
                    await LoadOperationHandleAsync<TType>(name, (handle) =>
                     {
                         ret = handle.Result;
                     });
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, exp.Message);
                }
                return ret;
            }
            public async UniTask<TComponent> LoadAssetInstantiateAsync<TComponent>(string name, Transform parent = null)
                where TComponent : class
            {
                TComponent ret = null;
                try
                {
                    await LoadOperationHandleAsync<GameObject>(name, (handle) =>
                    {
                        var obj = GameObject.Instantiate(handle.Result, parent);
                        ret = obj.GetComponent<TComponent>();
                    });
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, $"LoadAssetInstantiateAsync {name}  ->  {exp.Message}");
                }
                LogColor(Color.red, $"这里存在执行顺序问题    {ret != null}   name -> {name}");
                //await UniTask.Delay(1);
                await UniTask.WaitUntil(() =>
                {
                    return ret != null;
                });
                return ret;
            }

            public async UniTask DownLoadAddressables()
            {
                await AsyncDefault();
            }

            public async UniTask LoadSceneAsync(SceneType scene, LoadSceneMode mode)
            {
                await AsyncDefault();
                var name = scene.ToString();

                var sceneCount = SceneManager.sceneCount;
                for (int i = 0; i < sceneCount; i++)
                {
                    var activeScenes = SceneManager.GetSceneAt(i);
                    if (!object.Equals(activeScenes, "Scene_UI"))
                    {
                        await UnLoadSceneAsync(activeScenes.name);
                    }
                    if (object.Equals(name, activeScenes.name))
                    {
                        LogColor(Color.red, $"load scene defeated,   scene is exist    name -> {name}");
                        return;
                    }
                }


                Addressables.LoadSceneAsync(name, mode).Completed += (handle) =>
                {
                    if (handle.Status != AsyncOperationStatus.Failed)
                    {
                        LogColor(Color.black, $"locad scene finish    name -> {name}");
                        map_Scene.Add(handle.Result.Scene.name, handle);
                    }
                };
            }
            public async UniTask UnLoadSceneAsync(string scene)
            {
                await AsyncDefault();
                var name = scene;
                if (map_Scene.TryGetValue(name, out AsyncOperationHandle handle))
                {
                    var oprattion = Addressables.UnloadSceneAsync(handle, true);
                    oprattion.Completed += (handle) =>
                    {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            LogColor(Color.yellow, $"hint: unload scene finish   name -> {name}");
                        }
                        else
                        {
                            LogColor(Color.red, $"hint: unload scene defeated   name -> {name}");
                        }
                    };
                    map_Scene.Remove(name);
                }
                else
                {
                    LogColor(Color.red, $"no exist scene   name -> {name}");
                }
            }

            public async UniTask<T> LoadElement3DAsync<T>(params object[] parameters)
                where T : Element3D
            {
                T ret = null;
                var temp1 = typeof(T).ToString().Split('.');
                var prefabName = temp1[temp1.Length - 1];
                try
                {
                    var obj = await LoadAssetInstantiateAsync<T>(prefabName);
                    LogColor(Color.grey, obj != null);
                    await obj.Initializationasync(parameters);
                    ret = obj;  
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, $"load element3D defeated,  name and class -> {prefabName}  [->]  {exp}");
                }
                return ret;
            }
            public async UniTask<PoolObjectBase> LoadUIElementAsync<T>(RectTransform parent, params object[] parameters)
                where T: PoolObjectBase
            {
                PoolObjectBase ret = null;
                var temp1 = typeof(T).ToString().Split('.');
                var prefabName = temp1[temp1.Length - 1];
                try
                {
                    var pool = GameManager.Instance._public_Pool;
                    var handle = Addressables.LoadAssetAsync<GameObject>(prefabName);
                    handle.Completed += async (handle) =>
                    {
                        if (Equals(handle.Status, AsyncOperationStatus.Succeeded))
                        {
                            if (pool.TryGet(handle.Result, out ret))
                            {
                                ret.gameObject.SetActive(false);
                                ret.SetOriginal(handle.Result);
                                var rect = ret.GetComponent<RectTransform>();
                                rect.SetParent(parent);
                                rect.Normalized();
                                await ret.InitAsync(parameters);
                                await ret.PlayAsync(parameters);
                            }
                            else
                            {
                                LogColor(Color.red, $"object not load     name -> {prefabName}");
                            }
                        }
                    };
                    await handle.Task;
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, $"load element3D defeated,  name and class -> {prefabName}  [->]  {exp}");
                }
                return ret;
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