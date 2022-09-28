using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LevelBarUnit
{
	public RawImage activePanel, pastLevel;
	public TextMeshProUGUI levelText;
}
public class MainPanel : UIPanelBase
{
	public List<LevelBarUnit> levelBarUnits;
	[SerializeField]private TextMeshProUGUI coinText;
	public void OnEnable()
	{
		SceneController.Instance.OnSceneLoaded.AddListener(ShowPanel);
		LevelManager.Instance.OnLevelStart.AddListener(HidePanel);
	}
	public void OnDisable()
	{
		SceneController.Instance.OnSceneLoaded.RemoveListener(ShowPanel);
		LevelManager.Instance.OnLevelStart.RemoveListener(HidePanel);
	}
	private void ShowPanel()
	{
		base.ShowPanel();
		coinText.SetText(PlayerPrefs.GetInt(PlayerPrefKeys.Coin, 0).ToString());
		int fakeLevel = PlayerPrefs.GetInt(PlayerPrefKeys.FakeLevel, 1);

		for (int i = 0; i < 5; i++)
		{
			levelBarUnits[i].activePanel.enabled = false;
			levelBarUnits[i].pastLevel.enabled = false;
		}
		if (fakeLevel <= 1)
		{
			levelBarUnits[0].activePanel.enabled = true;
			levelBarUnits[0].pastLevel.enabled = false;
		}
		if (fakeLevel == 2)
		{
			levelBarUnits[0].activePanel.enabled = false;
			levelBarUnits[0].pastLevel.enabled = true;

			levelBarUnits[1].activePanel.enabled = true;
			levelBarUnits[1].pastLevel.enabled = false;
		}
		if (fakeLevel >= 3)
		{
			for (int i = 0; i < 2; i++)
			{
				levelBarUnits[i].activePanel.enabled = false;
				levelBarUnits[i].pastLevel.enabled = true;
				levelBarUnits[i].levelText.text = (fakeLevel - (2 - i)).ToString();
			}
			levelBarUnits[2].activePanel.enabled = true;
			levelBarUnits[2].pastLevel.enabled = false;
			levelBarUnits[2].levelText.text = fakeLevel.ToString();

			for (int i = 3; i < 5; i++)
			{
				levelBarUnits[i].activePanel.enabled = false;
				levelBarUnits[i].pastLevel.enabled = false;
				levelBarUnits[i].levelText.text = (fakeLevel + (i - 2)).ToString();
			}
		}
	}
}
