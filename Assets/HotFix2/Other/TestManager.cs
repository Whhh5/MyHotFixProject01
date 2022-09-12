using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        public class TestManager : A_Mode_Singleton<TestManager>
        {
            public A_Mgr_Pool<TTTT> _pool = new A_Mgr_Pool<TTTT>();



            public void ClearPool()
            {
                
            }
        }

        public class TTTT : PoolObjectBase
        {

        }
    }
}