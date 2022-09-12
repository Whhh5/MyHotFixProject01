using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.OdinInspector;

public class ComponentList : SerializedMonoBehaviour
{
    [Serializable]
    public class ItemFields
    {
        public int index = -1;
        public string name = "";
        public GameObject obj = null;
        public string type = null;
        public int componentEnum = 0;
    }
    [HideInInspector] public List<ItemFields> items = new List<ItemFields>();
    [HideInInspector] public Dictionary<string, int> indexs = new Dictionary<string, int>();

    public bool TryGet<T>(string key, out T component)
        where T:Component
    {
        bool ret = false;
        component = null;
        if (indexs.TryGetValue(key, out int index))
        {
            var item = items[index];

            if (typeof(T).ToString() == item.type)
            {
                var t = typeof(T);
                var com = item.obj.GetComponent(t);
                component = com as T;
                ret = true;
            }
            else
            {
                Debug.Log($"{GetType()}: Type is UMMatch  -> input type:{key}, obj type:{item.type}");
            }
        }
        else
        {
            Debug.Log($"{GetType()}: key is exist, input key -> {key}");
        }

        return ret;
    }
}