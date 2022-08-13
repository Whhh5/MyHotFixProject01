using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        public abstract class A_ : ILog, IAsyncDefault
        {
            public async Task AsyncDefault()
            {
                await Task.Delay(0);
            }
            #region Log
            public void A_Log(params object[] messages)
            {
                string str;
                str = string.Join(" + ", messages);
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {str}");
            }

            public void A_LogToColor(Color color, params object[] messages)
            {
                string msgs;
                string[] msgArray = new string[messages.Length];
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                var msgColor = ColorUtility.ToHtmlStringRGBA(color);
                for (int i = 0; i < messages.Length; i++)
                {
                    msgArray[i] = $"<color=#{msgColor}>{messages}</color>";
                }
                msgs = string.Join(" + ", msgArray);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {msgs}");
            }

            #endregion
        }

        public abstract class A_Mono : MonoBehaviour, ILog, ILifeCycle, IAsyncDefault
        {
#region Log
            public void A_Log(params object[] messages)
            {
                string str;
                str = string.Join(" + ", messages);
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                Debug.Log($"</color=#{csColor}>{GetType()}</color> - {str}");
            }

            public void A_LogToColor(Color color, params object[] messages)
            {
                string msgs;
                string[] msgArray = new string[messages.Length];
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                var msgColor = ColorUtility.ToHtmlStringRGBA(color);
                for (int i = 0; i < messages.Length; i++)
                {
                    msgArray[i] = $"<color=#{msgColor}>{messages}</color>";
                }
                msgs = string.Join(" + ", msgArray);
                Debug.Log($"</color=#{csColor}>{GetType()}</color> - {msgs}");
            }
            #endregion
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
            public abstract void OnShow();
            public virtual void OnDestroyGameObject()
            {
                Object.Destroy(gameObject);
            }

            public async Task AsyncDefault()
            {
                await Task.Delay(0);
            }
        }
        public abstract class A_MonoAsync : MonoBehaviour, ILog, ILifeCycleAsync, IAsyncDefault
        {
            #region Log
            public void A_Log(params object[] messages)
            {
                string str;
                str = string.Join(" + ", messages);
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {str}");
            }

            public void A_LogToColor(Color color, params object[] messages)
            {
                string msgs;
                string[] msgArray = new string[messages.Length];
                var csColor = ColorUtility.ToHtmlStringRGBA(Color.cyan);
                var msgColor = ColorUtility.ToHtmlStringRGBA(color);
                for (int i = 0; i < messages.Length; i++)
                {
                    msgArray[i] = $"<color=#{msgColor}>{messages}</color>";
                }
                msgs = string.Join(" + ", msgArray);
                Debug.Log($"<color=#{csColor}>{GetType()}</color> - {msgs}");
            }
            #endregion
            private async void Awake()
            {
                await OnAwakeAsync();
            }
            private async void Start()
            {
                await OnStartAsync();
            }
            public abstract Task OnAwakeAsync();
            public abstract Task OnStartAsync();
            public abstract Task OnShowAsync();
            public virtual async Task OnDestroyGameObjectAsync()
            {
                await AsyncDefault();
                Object.Destroy(gameObject);
            }
            public async Task AsyncDefault()
            {
                await Task.Delay(0);
            }
        }
        public abstract class A_Mode_Singleton<T> : A_ where T : class, new()
        {
            public T Instance = new T();
        }
        public abstract class A_Mode_Singleton_Mono<T> : A_Mono where T : A_Mode_Singleton_Mono<T>
        {
            public T Instance = null;
            public override void OnAwake()
            {
                if (Instance != null)
                {
                    Destroy(this);
                }
                else
                {
                    Instance = (T)this;
                }
            }
        }
    }
}