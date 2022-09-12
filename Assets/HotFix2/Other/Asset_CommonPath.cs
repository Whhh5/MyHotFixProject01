using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu( fileName = "Asset_CommonPath", menuName = "Asset/Common/Common Path")]
public class Asset_CommonPath : SerializedScriptableObject
{
    [SerializeField] Dictionary<string, string> _data = new Dictionary<string, string>();
    public string Get(string str)
    {
        string ret = "";
        if (!_data.TryGetValue(str, out ret))
        {
            Debug.Log($"common path    key ->  {str}      is a nil");
        }
        return ret;
    }
}
