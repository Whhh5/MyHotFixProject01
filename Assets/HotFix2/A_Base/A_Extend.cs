using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BXB
{
    namespace Core
    {
        public static class A_Extend
        {
            public static bool TryToInt(this uint original, out int value)
            {
                bool ret = false;
                value = 0;
                try
                {
                    value = (int)original;
                    ret = true;
                }
                catch (Exception exp)
                {
                    Debug.Log($"<color=#FFFFFF>{exp.Message}</color>");
                }
                return ret;
            }
            public static bool TryToUInt(this int original, out uint value)
            {
                bool ret = false;
                value = 0;
                try
                {
                    value = (uint)original;
                    ret = true;
                }
                catch (Exception exp)
                {
                    Debug.Log($"<color=#FFFFFF>{exp.Message}</color>");
                }
                return ret;
            }
        }
    }
}
        