using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BXB.Core;

namespace UnityToolbarExtender.Examples
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 100,
            };
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static async void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Main", "Start Scene 1"), ToolbarStyles.commandButtonStyle))
            {
                if (EditorApplication.isPlaying || EditorApplication.isPaused)
                {
                    EditorApplication.isPlaying = false;
                }
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

                var sceneToOpen = "main";
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene($"{scenePath}");
                }
                EditorApplication.isPlaying = true;
            }

            if (GUILayout.Button(new GUIContent("2", "Start Scene 2"), ToolbarStyles.commandButtonStyle))
            {
                Debug.Log("2");
            }
        }
        public static void StartScene2()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                EditorApplication.isPlaying = false;
            }
            AssetDatabase.SaveAssets();
            var scenePath = SceneManager.GetSceneAt(0);
            Debug.Log(scenePath.path);
            EditorSceneManager.OpenScene($"{scenePath.path}", OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

    }
}