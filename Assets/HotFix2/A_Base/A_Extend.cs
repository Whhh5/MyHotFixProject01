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
            public static Transform Normalized(this Transform transform)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
                transform.localScale = Vector3.one;
                return transform;
            }
            public static RectTransform Normalized(this RectTransform rect)
            {
                rect.anchoredPosition3D = Vector3.zero;
                rect.sizeDelta = Vector2.zero;
                rect.localRotation = Quaternion.Euler(Vector3.zero);
                rect.localScale = Vector3.one;
                return rect;
            }
        }
    }
}
        