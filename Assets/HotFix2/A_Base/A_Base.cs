using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Serialization;

namespace BXB
{
    namespace Core
    {
        public abstract class A_MonoBase : SerializedMonoBehaviour, ILog, IAsyncDefault
        {
            public void Log(params object[] messages)
            {
                string str;
                str = string.Join(" + ", messages);
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {str}");
            }
            public void LogColor(Color color, params object[] messages)
            {
                string msgs;
                string[] msgArray = new string[messages.Length];
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                var msgColor = ColorUtility.ToHtmlStringRGBA(color);
                for (int i = 0; i < messages.Length; i++)
                {
                    msgArray[i] = $"<color=#{msgColor}>{messages[i].ToString()}</color>";
                }
                msgs = string.Join(" + ", msgArray);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {msgs}");
            }
            public async UniTask AsyncDefault()
            {
                await UniTask.Delay(0);
            }
        }
        public abstract class A_ : ILog, IAsyncDefault
        {
            public async UniTask AsyncDefault()
            {
                await UniTask.Delay(0);
            }
            #region Log
            public void Log(params object[] messages)
            {
                string str;
                str = string.Join(" + ", messages);
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {str}");
            }

            public void LogColor(Color color, params object[] messages)
            {
                string msgs;
                string[] msgArray = new string[messages.Length];
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                var msgColor = ColorUtility.ToHtmlStringRGBA(color);
                for (int i = 0; i < messages.Length; i++)
                {
                    msgArray[i] = $"<color=#{msgColor}>{messages[i].ToString()}</color>";
                }
                msgs = string.Join(" + ", msgArray);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {msgs}");
            }

            #endregion
        }

        public abstract class A_Mono : A_MonoBase, ILifeCycle
        {
            private void Awake()
            {
                OnAwake();
            }
            private void Start()
            {
                OnStart();
            }
            public abstract void OnAwake();
            public abstract void OnStart();
            public virtual void OnDestroyGameObject()
            {
                Object.Destroy(gameObject);
            }
        }
        public abstract class A_MonoAsync : A_MonoBase, ILifeCycleAsync
        {
            private async void Awake()
            {
                await OnAwakeAsync();
            }
            private async void Start()
            {
                await OnStartAsync();
            }
            public abstract UniTask OnAwakeAsync();
            public abstract UniTask OnStartAsync();
            public abstract UniTask OnShowAsync();
            public virtual async UniTask OnDestroyGameObjectAsync()
            {
                await AsyncDefault();
                Object.Destroy(gameObject);
            }
        }
        public abstract class A_Mode_Singleton<T> : A_ where T : class, new()
        {
            public static T Instance = new T();
        }
        public abstract class A_Mode_Singleton_Mono<T> : A_Mono where T : A_Mode_Singleton_Mono<T>
        {
            public static T Instance = null;
            
            public sealed override void OnAwake()
            {
                //if (Instance != null)
                //{
                //    Debug.Log($"Ïú»Ù   {Instance.GetType()} ");
                //    Debug.Log(Instance.name);
                //    Destroy(this);
                //}
                //else
                //{
                    Instance = (T)this;
                    Debug.Log($"ÊµÀý»¯   name->    {Instance.GetType()}");
                //}
            }
        }
    }
}