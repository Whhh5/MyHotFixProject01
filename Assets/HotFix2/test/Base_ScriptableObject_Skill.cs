using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Base_ScriptableObject_Skill : ScriptableObject
{
    public string name_skill;
    public Sprite icon;
    public abstract UniTask PlayAsync(SkillParamater paras, Func<CallBackPara, UniTask> callback, params object[] parameters);
}
