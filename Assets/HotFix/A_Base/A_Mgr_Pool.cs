using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace BXB
{
    namespace Core
    {
        public class A_Mgr_Pool<TKey, TValue> : A_
            where TKey : UnityEngine.Object
            where TValue: UnityEngine.Object
        {
            Dictionary<TKey, A_List<TValue>> pool = new Dictionary<TKey, A_List<TValue>>();
            public async Task<TValue> GetAsync(TKey original, bool noneIsNew = false)
            {
                await AsyncDefault();
                TValue ret = null;
                //if (pool.ContainsKey(original) && pool[original].Count != 0)
                //{
                //    ret = pool[original].;
                //}
                //else
                //{
                //    switch (noneIsNew)
                //    {
                //        case true:

                //            break;
                //        case false:
                //            break;
                //    }
                //}
                return ret;
            }

            public async Task ReplaceAsync(TKey original, TValue value)
            {
                if (pool.ContainsKey(original))
                {

                }
            }
        }
    }
}
