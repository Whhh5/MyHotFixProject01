using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;


namespace BXB
{
    namespace Core
    {
        public class A_Mgr_Pool<TValue> : A_, IA_Mgr_Pool<TValue>
            where TValue : PoolObjectBase
        {
            private Dictionary<GameObject, A_List2<TValue>> _pool = new Dictionary<GameObject, A_List2<TValue>>();
            public bool Get(GameObject original, out TValue value, bool noneIsNew = false)
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
                    A_LogToColor(Color.red, exp.Message.ToString());
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
                        }
                        else
                        {
                            A_LogToColor(Color.yellow, " ÃÌº” ß∞‹");
                        }
                    }
                }
                catch (Exception exp)
                {
                    A_LogToColor(Color.red, exp.Message.ToString());
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
                    A_LogToColor(Color.red, exp.Message.ToString());
                }
                
            }
        }
    }
}
