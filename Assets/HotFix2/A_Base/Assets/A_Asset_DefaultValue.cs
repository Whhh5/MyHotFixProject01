using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New DefauleAsset", menuName = "System/DefaultAsset")]
public class A_Asset_DefaultValue : SerializedScriptableObject
{
    [SerializeField]
    DefaultValueKey key;
    [SerializeField] 
    string type;
    [SerializeField]
    Object value;
    [SerializeField] Dictionary<DefaultValueKey, int> data = new Dictionary<DefaultValueKey, int>();
    [SerializeField] Dictionary<DefaultValueKey, Object> data2 = new Dictionary<DefaultValueKey, Object>();
    public object GetAsync(DefaultValueKey key)
    {
        object ret = null;
        if (data2.TryGetValue(key, out Object obj))
        {
            ret = obj;
        }
        return ret;
    }
    [Button]
    public void Add()
    {
        data2.Add(key, value);
    }
    public enum DefaultValueKey
    {
        A,
        B,
        C,
        D
    }
}
