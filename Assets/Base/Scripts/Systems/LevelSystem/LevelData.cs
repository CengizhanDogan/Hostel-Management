using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif


public enum LevelType { Default }

[System.Serializable]
public class Level
{
	public Level(LevelData _levelData)
	{
		levelData = _levelData;
	}

	[BoxGroup("LevelType")]
	[ListDrawerSettings(AlwaysAddDefaultValue = true)]
	public List<LevelType> LevelTypes = new List<LevelType>();

	[ValueDropdown("LevelNames")]
	public string LoadLevelID;


	private LevelData levelData;


	#region EditorUtils

#if UNITY_EDITOR

	public List<string> LevelNames
	{
		get
		{
			List<string> levelNames = new List<string>();
			for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
			{
				var level = EditorBuildSettings.scenes[i];

				if (level.path.Contains("Level") || level.path.Contains("Test"))
				{

					int slash = level.path.LastIndexOf('/');
					string name = level.path.Substring(slash + 1);
					name = name.Replace(".unity", string.Empty);
					levelNames.Add(name);
				}
			}
			return levelNames;
		}
	}

	public Dictionary<string, string> LevelProperties
	{
		get
		{
			Dictionary<string, string> levelProperties = new Dictionary<string, string>();
			List<string> levelNames = new List<string>();
			for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
			{
				var level = EditorBuildSettings.scenes[i];

				if (level.path.Contains("Level") || level.path.Contains("Test"))
				{

					int slash = level.path.LastIndexOf('/');
					string name = level.path.Substring(slash + 1);
					name = name.Replace(".unity", string.Empty);
					levelNames.Add(name);
					levelProperties.Add(name, level.path);
				}
			}
			return levelProperties;
		}
	}

#endif
	#endregion

}



public class LevelData : ScriptableObject
{
	[ListDrawerSettings(CustomAddFunction = "CreateLevel")]
	[OnValueChanged("UpdateHyperGUI")]
	public List<Level> Levels = new List<Level>();


	private Level CreateLevel()
	{
		return new Level(this);
	}
	private void UpdateHyperGUI()
	{
		EventManager.OnLevelDataChange.Invoke();
	}
}
