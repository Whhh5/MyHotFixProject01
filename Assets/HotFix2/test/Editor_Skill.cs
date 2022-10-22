using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class Editor_Skill : A_Mono
{
    [SerializeField]public List<KeyCode> key = new List<KeyCode>();
    [SerializeField]public List<Base_ScriptableObject_Skill> value = new List<Base_ScriptableObject_Skill>();
    public Dictionary<int, int> dic = new Dictionary<int, int>();
    [SerializeField] public bool lock_Skill = false;
    [SerializeField] public bool isAttack = false;
    public override void OnAwake()
    {
        lock_Skill = false;
    }

    public override void OnStart()
    {

    }

    public async UniTask<Base_ScriptableObject_Skill> TryGetValueAsync(KeyCode key)
    {
        await AsyncDefault();
        var index = this.key.IndexOf(key);
        Base_ScriptableObject_Skill ret = null;

        if (!(index < 0))
        {
            ret = value[index];
        }
        else
        {
            Debug.Log("------------------------");
        }
        return ret;
    }
}
