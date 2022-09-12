using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[CustomEditor(typeof(Editor_Skill))]
public class CustomEditor_Editor_Skill : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (GUILayout.Button("Open"))
        {
            var w = new Win();
            w.Show();
            w.minSize = new Vector2(1000, 700);
        }
    }
}
public class Win: EditorWindow
{
    private void OnGUI()
    {
        //target
        if (GUILayout.Button("Open"))
        {
            Debug.Log(1);
        }
    }
}