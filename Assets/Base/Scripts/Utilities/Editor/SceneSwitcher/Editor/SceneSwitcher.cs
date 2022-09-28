using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.ToolbarButtons
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
				fontStyle = FontStyle.Bold
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

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button(new GUIContent("1", "Start Scene 1"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene(0);
			}

			if (GUILayout.Button(new GUIContent("2", "Start Scene 2"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene(1);
			}

			if (GUILayout.Button(new GUIContent("3", "Start Scene 2"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene(2);
			}

		}
	}

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(int sceneIndex)
		{
			if (EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				EditorApplication.OpenScene(EditorBuildSettings.scenes[sceneIndex].path);

		}

	}
}