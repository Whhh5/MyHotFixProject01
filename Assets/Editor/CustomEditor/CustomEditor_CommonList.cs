using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(ItemList))]
public class CustomEditor_CommonList : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        ItemList temp = target as ItemList;
        Undo.RecordObject(target, "F");

        var items = temp.items;
        var indexs = temp.indexs;



        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mono List", GUILayout.Width(200));
        if (GUILayout.Button("Update Data"))
        {
            indexs.Clear();
            List<string> log = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                if (!indexs.ContainsKey(items[i].name))
                {
                    indexs.Add(items[i].name, i);
                    items[i].index = i;
                    Debug.Log(items[i].type);
                }
                else
                {
                    log.Add($"{i}:{items[i].name}");
                }
            }
            string str = $"Update finish, key is exist num {log.Count}: ";
            foreach (var item in log)
            {
                str += $"\n\t\t{item}";
            }
            Debug.Log(str);
        }
        EditorGUILayout.EndHorizontal();

        //ObjectField obj = new ObjectField();
        //obj.objectType = typeof(GameObject);
        //obj.allowSceneObjects = false;
        //obj.SetEnabled(true);

        for (int i = 0; i < items.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.TextField(items[i].index.ToString(), GUILayout.Width(20));

            //GUIStyle style = new GUIStyle(GUI.skin.label);
            //style.normal.textColor = Color.red;


            if (indexs.ContainsKey(items[i].name) && i != indexs[items[i].name])
            {
                GUIStyle customStyle = new GUIStyle();
                customStyle.normal.textColor = Color.red;
                customStyle.normal.background = new Texture2D(50, 50);
                items[i].name = EditorGUILayout.TextField(items[i].name, customStyle);
            }
            else
            {
                items[i].name = EditorGUILayout.TextField(items[i].name);
            }

            if (GUILayout.Button("Reset", GUILayout.Width(50)) && items[i].obj != null)
            {
                items[i].name = items[i].obj.name;
            }
            var obj = EditorGUILayout.ObjectField(items[i].obj, typeof(GameObject));
            if (obj != null)
            {
                if (items[i].name == "")
                {
                    items[i].name = obj.name;
                }
                var gameobj = obj as GameObject;
                items[i].obj = gameobj;

                var components = gameobj.GetComponents<Component>();
                var strArr = new string[components.Length];
                for (int j = 0; j < components.Length; j++)
                {
                    strArr[j] = components[j].GetType().ToString();
                }
                items[i].componentEnum = EditorGUILayout.Popup(items[i].componentEnum, strArr);
                items[i].type = strArr[items[i].componentEnum];
            }
            else
            {

            }

            //EditorGUILayout.TextField("", GUILayout.Width(100));


            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                items.RemoveAt(i);
            }



            EditorGUILayout.EndHorizontal();

        }

        if (GUILayout.Button("Add Item"))
        {
            items.Add(new ItemList.ItemFields());
        }

        EditorUtility.SetDirty(target);
    }
}
