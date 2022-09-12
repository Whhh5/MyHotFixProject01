using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BXB
{
    namespace Core
    {
        public interface ILog
        {
            public void Log(params object[] messages);
            public void LogColor(Color color, params object[] messages);
        }

        public interface ILifeCycle
        {
            public abstract void OnAwake();
            public abstract void OnStart();
            public abstract void OnDestroyGameObject();
        }
        public interface ILifeCycleAsync
        {
            public abstract UniTask OnAwakeAsync();
            public abstract UniTask OnStartAsync();
            public abstract UniTask OnShowAsync();
            public abstract UniTask OnDestroyGameObjectAsync();
        }
        public interface IAsyncDefault
        {
            public UniTask AsyncDefault();
        }

        public interface IMgr_Resource
        {
            public UniTask<TTyoe> LoadAssetAsync<TTyoe>(string name)
                where TTyoe : UnityEngine.Object;
            public UniTask<TComponent> LoadAssetInstantiateAsync<TComponent>(string name, Transform parent, bool isAwait = false, Action<AsyncOperationHandle<GameObject>> callback = null)
                where TComponent : UnityEngine.Object;
        }
        public interface IMgr_Pool
        {
            public UniTask Get<T>();
            public UniTask GetAsync<T>();
        }

        public interface IA_List<TType>
            where TType : UnityEngine.Object
        {
            public List<TType> list { get; }
            public ushort unitCount { get; }
            public uint countP { get; }
            public A_LinkList<uint> nicks { get; }


            public bool TryGetValueToIndex(uint index, out TType value);
            public bool TryGetIndexToValue(TType value, out uint index);
            public bool TryGetAll(out Dictionary<uint, TType> values);
            public bool TryAdd(TType value, out uint index);
            public bool TryFind(TType obj, out uint index);
            public bool TryRemoveAtIndex(uint index, out TType oldValue);
            public bool TryRemoveAtValue(TType value, out uint oldValue);
            public bool TryClear(out List<TType> oldList);
        }

        public interface IA_List2<TType>
            where TType : UnityEngine.Object
        {

        }

        public interface IA_Mgr_Pool<TValue>
        {

        }

        public interface IPoolObjectBase
        {

        }
    }
}