using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            public abstract void OnShow();
            public abstract void OnDestroyGameObject();
        }
        public interface ILifeCycleAsync
        {
            public abstract Task OnAwakeAsync();
            public abstract Task OnStartAsync();
            public abstract Task OnShowAsync();
            public abstract Task OnDestroyGameObjectAsync();
        }
        public interface IAsyncDefault
        {
            public Task AsyncDefault();
        }

        public interface IMgr_Resource
        {
            public Task<TReturn> LoadAssetAsync<TLoad, TReturn>(string name) 
                where TLoad : UnityEngine.Object
                where TReturn : UnityEngine.Object;
            public Task<TReturn> LoadAssetInstantiateAsync<TLoad, TReturn>(string name, Transform parent) 
                where TReturn : UnityEngine.Object
                where TLoad : UnityEngine.Object;
        }
        public interface IMgr_Pool
        {
            public Task Get<T>();
            public Task GetAsync<T>();
        }

        public interface IA_List<TType>
            where TType : UnityEngine.Object
        {
            public bool TryGetToIndex(uint index, out TType value);
            public bool ExtendList();
            public bool TryAdd(TType value, out uint index);
            public void Update();
            public bool TryGet(out TType value);
            public uint GetCount();
            public bool TryFind(TType obj, out TType value, out uint index);
            public bool TryReplaceToValue(TType original, TType newvalue, out TType oldValue);
            public bool TryReplaceToIndex(uint index, TType newvalue, out TType oldValue);
            public bool TryRemoveAtIndex(uint index, out TType oldValue);
            public bool TryRemoveAtValue(TType value, out TType oldValue);
            public List<TType> Clear();
        }
    }
}