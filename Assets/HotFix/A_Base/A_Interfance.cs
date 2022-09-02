using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace BXB
{
    namespace Core
    {
        public interface ILog
        {
            public void A_Log(params object[] messages);
            public void A_LogToColor(Color color, params object[] messages);
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
            public Task<TReturn> LoadAssetAsync<TReturn>(string name)
                where TReturn : UnityEngine.Object;
            public Task<TReturn> LoadAssetInstantiateAsync<TReturn>(string name, Transform parent)
                where TReturn : UnityEngine.Object;
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