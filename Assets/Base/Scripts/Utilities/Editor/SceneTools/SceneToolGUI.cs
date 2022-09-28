using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class SceneToolGUI : MonoBehaviour
{
	private static int SelectLevel;

	static string[] _options = new string[LevelData.Levels.Count];

	private static int currentTimeScale;

	static string[] timeScaleOption = new string[5] { "0.1", "0.5", "1", "2", "3" };

	private static bool isLevelAreaShowing = true;

	private static float timeSliderValue = 0.05F;

	public static bool panelIsShowing = true;

	private static bool wasMouseRight = false;

	private static bool gameStopped;

	private static float oldTimeSliderValue;


	static SceneToolGUI()
	{
		SceneView.duringSceneGui += OnSceneView;

		EditorApplication.playModeStateChanged += LogPlayModeState;

		SelectLevel = PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel);
		Time.timeScale = 1f;

		for (int i = 0; i < LevelData.Levels.Count; i++)
		{
			_options[i] = "" + (i + 1);
		}

		EventManager.OnLevelDataChange.AddListener(() =>
		{
			_options = new string[LevelData.Levels.Count];
			for (int i = 0; i < LevelData.Levels.Count; i++)
			{
				_options[i] = "" + (i + 1);
			}
		}
		);
		oldTimeSliderValue = timeSliderValue;


	}

	~SceneToolGUI()
	{
		EventManager.OnLevelDataChange.RemoveListener(() =>
		{
			_options = new string[LevelData.Levels.Count];
			for (int i = 0; i < LevelData.Levels.Count; i++)
			{
				_options[i] = "" + (i + 1);
			}
		});
	}


	private static void LogPlayModeState(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.EnteredEditMode && PlayerPrefs.GetString("gamePlayedOnGui") == "true")
		{
			PlayerPrefs.SetString("gamePlayedOnGui", "false");
			EditorApplication.OpenScene(LevelData.Levels[SelectLevel].LevelProperties[LevelData.Levels[SelectLevel].LoadLevelID]);
		}
	}
	public static void OnSceneView(SceneView scene)
	{
		if (!panelIsShowing)
		{
			Handles.BeginGUI();
			GUI.contentColor = Color.white;
			GUI.backgroundColor = Color.cyan;

			if (GUI.Button(new Rect(4, 4, 50, 50), (Texture)AssetDatabase.LoadAssetAtPath("Assets/Base/Graphics/2DGraphics/Scripts/tools.png", typeof(Texture)), EditorStyles.miniButton))
				ShowHidePanel();

			Handles.EndGUI();
			return;
		}
		Handles.BeginGUI();

		GUI.contentColor = Color.cyan;

		GUI.Label(new Rect(20, 20, 250, 50), "Scene Tools", EditorStyles.toolbarButton);

		GUI.contentColor = Color.white;
		GUILayout.BeginArea(new Rect(20, 43, 250, 750));

		var rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect, new GUIContent(""), EditorStyles.helpBox);

		GUI.backgroundColor = Color.white;
		GUI.contentColor = Color.cyan;
		GUI.Label(rect, new GUIContent("Levels"), EditorStyles.toolbarButton);
		GUILayout.Space(20);
		GUI.color = Color.white;
		GUILayout.BeginHorizontal();

		GUI.backgroundColor = new Color(0.2f, 0.803f, 1f);
		GUI.contentColor = Color.cyan;
		GUILayout.FlexibleSpace();

		GUILayout.FlexibleSpace();
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
		{
			wasMouseRight = true;
		}
		else if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
		{
			wasMouseRight = false;
		}

		GUILayout.EndHorizontal();

		if (panelIsShowing)
		{
			DrawLevelArea();
			if (Application.isPlaying)
			{
				DrawTimeArea();
				DrawInGameFuncTions();
			}
		}
		GUILayout.Space(5);

		EditorGUILayout.EndVertical();
		GUILayout.EndArea();

		Handles.EndGUI();
	}

	private static void ShowHidePanel()
	{
		panelIsShowing = !panelIsShowing;
	}

	#region Level Area
	private static void DrawLevelArea()
	{
		EditorGUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.white;
		GUI.contentColor = Color.cyan;

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		int lastSelectLevel = SelectLevel;

		if (isLevelAreaShowing)
			SelectLevel = GUILayout.SelectionGrid(SelectLevel, _options.ToArray(), 5);

		if (SelectLevel != lastSelectLevel)
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				EditorApplication.OpenScene(LevelData.Levels[SelectLevel].LevelProperties[LevelData.Levels[SelectLevel].LoadLevelID]);
		}

		if (!Application.isPlaying && gameStopped)
		{
			gameStopped = false;
			EditorApplication.OpenScene(LevelData.Levels[SelectLevel].LevelProperties[LevelData.Levels[SelectLevel].LoadLevelID]);
		}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Play"))
		{
			PlayScene(SelectLevel);
		}
		if (GUILayout.Button("Show/Hide Panel"))
		{
			ShowHidePanel();
		}
		if (GUILayout.Button("Add Scene"))
		{
			AddCurrentScene(EditorApplication.currentScene);
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();
	}
	#endregion

	[MenuItem("My Commands/Second Command _o")]
	static void PlayCommand()
	{
		wasMouseRight = true;
		PlayScene(SelectLevel);
		if (EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = false;

			gameStopped = true;
		}
	}

	#region Time area
	private static void DrawTimeArea()
	{
		var rect2 = EditorGUILayout.BeginVertical();
		GUI.backgroundColor = Color.white;
		GUI.contentColor = Color.cyan;
		GUI.Box(rect2, GUIContent.none);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Time Settings");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		GUILayout.Space(50);

		timeSliderValue = GUILayout.HorizontalSlider(timeSliderValue, 0.05f, 6.0f);
		GUILayout.Space(75);

		if (oldTimeSliderValue != timeSliderValue)
		{
			SetSpeed(timeSliderValue);
		}

		oldTimeSliderValue = timeSliderValue;

		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();

		var defaultAlignment = GUI.skin.label.alignment;
		GUILayout.Label("0.05");
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUILayout.FlexibleSpace();

		GUILayout.TextField(timeSliderValue.ToString("0.00"));

		GUILayout.Label("3");
		GUI.skin.label.alignment = defaultAlignment;


		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.white;

		int lastSelectedTime = currentTimeScale;
		currentTimeScale = GUILayout.SelectionGrid(currentTimeScale, timeScaleOption, 5);
		if (lastSelectedTime != currentTimeScale)
		{
			SetSpeed(float.Parse(timeScaleOption[currentTimeScale], CultureInfo.InvariantCulture.NumberFormat));
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.white;
		GUI.contentColor = Color.cyan;

		GUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();

	}
	#endregion

	#region InGameFunctions
	private static void DrawInGameFuncTions()
	{
		var rect2 = EditorGUILayout.BeginVertical();
		GUI.color = Color.yellow;
		GUI.Box(rect2, GUIContent.none);

		GUI.color = Color.white;

		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.Label("Game Functions");

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Previous"))
		{
			LevelManager.Instance.LoadPreviousLevel();
		}
		if (GUILayout.Button("Restart"))
		{
			LevelManager.Instance.LoadLastLevel();
		}
		if (GUILayout.Button("Next Scene"))
		{
			LevelManager.Instance.LoadNextLevel();
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.cyan;
		if (GUILayout.Button("Fail"))
		{
			GameManager.Instance.OnStageFail.Invoke();
		}
		if (GUILayout.Button("Success"))
		{
			GameManager.Instance.OnStageSuccess.Invoke();
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();
	}

	#endregion
	private static LevelData LevelData
	{
		get
		{
			var levelDatas = AssetDatabase.FindAssets("t:LevelData");
			return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(levelDatas[0]), typeof(LevelData)) as LevelData;
		}
	}
	private static void AddCurrentScene(string scenePath)
	{

		List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

		foreach (EditorBuildSettingsScene scene in scenes)
		{
			if (scene.path == scenePath)
			{
				AddLeveltoLevelData(scenePath);
				return;
			}
		}

		AddLeveltoLevelData(scenePath);

		EditorBuildSettingsScene newScene = new EditorBuildSettingsScene();
		newScene.path = scenePath;
		newScene.enabled = true;
		scenes.Add(newScene);
		EditorBuildSettings.scenes = scenes.ToArray();

	}
	private static void AddLeveltoLevelData(string scenePath)
	{
		int slash = scenePath.LastIndexOf('/');
		string name = scenePath.Substring(slash + 1);
		name = name.Replace(".unity", string.Empty);

		Level newLevel = new Level(new LevelData());
		newLevel.LoadLevelID = name;
		LevelData.Levels.Add(newLevel);
		EventManager.OnLevelDataChange.Invoke();
	}

	private static void PlayScene(int level)
	{
		PlayerPrefs.SetInt(PlayerPrefKeys.LastLevel, 0);

		if (!EditorApplication.isPlaying)
		{
			if (wasMouseRight)
			{
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					EditorApplication.OpenScene(EditorBuildSettings.scenes[0].path);
				PlayerPrefs.SetInt(PlayerPrefKeys.LastLevel, SelectLevel);

				UnityEditor.EditorApplication.isPlaying = true;
				PlayerPrefs.SetString("gamePlayedOnGui", "true");
			}
			else
			{
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					EditorApplication.OpenScene(LevelData.Levels[level].LevelProperties[LevelData.Levels[level].LoadLevelID]);
			}
		}
		else
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.LastLevel, SelectLevel);
			SceneManager.LoadScene(0);
		}
	}

	private static void SetSpeed(float time)
	{
		Time.timeScale = time;
	}
}