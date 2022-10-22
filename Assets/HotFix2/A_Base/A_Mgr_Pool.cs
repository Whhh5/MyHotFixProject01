using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BXB
{
    namespace Core
    {
        public class A_Mgr_Pool<TValue> : A_, IA_Mgr_Pool<TValue>
            where TValue : PoolObjectBase
        {
            private Dictionary<GameObject, A_List2<TValue>> _pool = new Dictionary<GameObject, A_List2<TValue>>();
            private Transform root = null;
            public A_Mgr_Pool(Transform root = null)
            {
                if (Equals(root, null))
                {
                    var obj = new GameObject("pool_root");
                    GameObject.DontDestroyOnLoad(obj);
                    root = obj.transform;
                }
                this.root = root;
            }
            public void SetRoot(Transform root)
            {
                this.root = root;
            }
            public bool TryGet(GameObject original, out TValue value, bool noneIsNew = true)
            {
                bool ret = false;
                value = null;
                try
                {
                    if (!_pool.ContainsKey(original))
                    {
                        _pool.Add(original, new A_List2<TValue> { });
                    }
                    if (_pool[original].TryPop(out TValue f_value))
                    {
                        value = f_value;
                        ret = true;
                    }
                    else
                    {
                        if (noneIsNew)
                        {
                            value = GameObject.Instantiate(original).GetComponent<TValue>();
                            value.SetOriginal(original);
                            ret = true;
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, exp.Message.ToString());
                }
                return ret;
            }

            public void ReplaceAsync(TValue obj)
            {
                try
                {
                    if (obj.TryGetOriginalObject(out GameObject original))
                    {
                        if (!_pool.ContainsKey(original))
                        {
                            _pool.Add(original, new A_List2<TValue> { });
                        }
                        if (!object.ReferenceEquals(obj, null) &&
                            _pool[original].TryPush(obj))
                        {
                            obj.gameObject.SetActive(false);
                            obj.transform.SetParent(root);
                        }
                        else
                        {
                            LogColor(Color.yellow, " Ìí¼ÓÊ§°Ü");
                        }
                    }
                    else
                    {
                        LogColor(Color.yellow, $"not set original   name -> {obj.name}    child Index -> {obj.transform.GetSiblingIndex()}");
                    }
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, exp.Message.ToString());
                }
            }
            public void ClearPool()
            {
                var temporary = _pool;
                _pool = new Dictionary<GameObject, A_List2<TValue>>();
                try
                {
                    foreach (var item in temporary)
                    {
                        while (item.Value.TryPop(out TValue obj))
                        {
                            GameObject.Destroy(obj.gameObject);
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogColor(Color.red, exp.Message.ToString());
                }
                
            }
        }
    }
}
