using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        public class TestManager : A_Mode_Singleton<TestManager>
        {
            public A_Mgr_Pool<Bullet> _pool = new A_Mgr_Pool<Bullet>();



            public void ClearPool()
            {
                
            }
        }
    }
}